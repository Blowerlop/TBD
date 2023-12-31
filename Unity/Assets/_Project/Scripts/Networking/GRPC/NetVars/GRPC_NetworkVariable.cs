using System;
using System.IO;
using System.Threading;
using Grpc.Core;
using GRPCClient;
using Newtonsoft.Json;
using Project.Extensions;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Type = System.Type;

namespace Project
{
    public class GRPC_NetworkVariable<T> : NetworkVariable<T>  where T : struct
    {
        private static MainService.MainServiceClient _client => GRPC_Transport.instance.client;
        
        private AsyncClientStreamingCall<GRPC_NetVarUpdate, GRPC_EmptyMsg> _sendStream;
        private CancellationTokenSource _sendStreamCancellationTokenSource;

        private CancellationTokenSource _sendRequireSyncCancellationTokenSource;


        [ShowInInspector] private readonly int _variableHashName;
        private GRPC_GenericType _currentType = GRPC_GenericType.Isnull;
        private int _netId;

        private bool _isGrpcSync;

        
        public GRPC_NetworkVariable(string variableName, T value = default,
            NetworkVariableReadPermission readPerm = DefaultReadPerm,
            NetworkVariableWritePermission writePerm = DefaultWritePerm) : base(value, readPerm, writePerm)
        {
            _variableHashName = variableName.ToLower().ToHashIsSameAlgoOnUnreal();
        }


        public void Initialize()
        {
            NetworkBehaviour networkBehaviour = GetBehaviour();
            if (!networkBehaviour.IsServer && !networkBehaviour.IsHost) return;
            
            if (GRPC_NetworkManager.instance.isConnected)
            {
                GRPC_NetworkVariable_Initialization();
            }

            GRPC_NetworkManager.instance.onClientStartedEvent.Subscribe(this, GRPC_NetworkVariable_Initialization);
            GRPC_NetworkManager.instance.onUnrealClientConnected.Subscribe(this, OnUnrealClientConnected_WaitForNetVarSyncRequest);
            GRPC_NetworkManager.instance.onClientStopEvent.Subscribe(this, OnClientStop);
        }

        public void Reset()
        {
            if (GRPC_NetworkManager.isBeingDestroyed) return;
            
            GRPC_NetworkManager.instance.onClientStartedEvent.Unsubscribe(GRPC_NetworkVariable_Initialization);
            GRPC_NetworkManager.instance.onUnrealClientConnected.Unsubscribe(OnUnrealClientConnected_WaitForNetVarSyncRequest);
            GRPC_NetworkManager.instance.onClientStopEvent.Unsubscribe(OnClientStop);

            // OnClientStop();
        }

        

        private void GRPC_NetworkVariable_Initialization()
        {
             NetworkBehaviour networkBehaviour = GetBehaviour();

            // Server authoritative only 
            // Only the server open NetworkVariable streams
            if (!networkBehaviour.IsServer && !networkBehaviour.IsHost) return;
            _netId = (int)networkBehaviour.GetComponentInParent<NetworkObject>().NetworkObjectId;
            
            _currentType = GetGrpcGenericType();
            
            _sendStream = _client.GRPC_SrvNetVarUpdate();
            _sendStreamCancellationTokenSource = new CancellationTokenSource();
            
            Sync();
            OnValueChanged += OnValueChange_WriteInStream;

            _isGrpcSync = true;
        }

        private void OnValueChange_WriteInStream(T _, T newValue)
        {
            UpdateVariableOnGrpc(newValue);
        }
        
        private async void UpdateVariableOnGrpc(T newValue)
        {
            if (GRPC_NetworkManager.instance.isConnected == false) return;
            
            Debug.Log("Update variable");
            try
            {
                object valueToEncodeInJson;
                bool autoJsonSerialization = true;
                
                if (newValue is FixedString32Bytes or FixedString64Bytes or FixedString128Bytes)
                {
                    if (newValue is FixedString32Bytes string32Bytes)
                    {
                        valueToEncodeInJson = string32Bytes.Value;
                    }
                    else if (newValue is FixedString64Bytes string64Bytes)
                    {
                        valueToEncodeInJson = string64Bytes.Value;
                    }
                    else if (newValue is FixedString128Bytes string128Bytes)
                    {
                        valueToEncodeInJson = string128Bytes.Value;
                    }
                    else
                    {
                        valueToEncodeInJson = null;
                    }
                }
                else if (newValue is NetworkVector3Simplified)
                {
                    autoJsonSerialization = false;
                    NetworkVector3Simplified networkString = (NetworkVector3Simplified)Convert.ChangeType(newValue, typeof(NetworkVector3Simplified));
                    valueToEncodeInJson = $"X={networkString.x},Y={networkString.y},Z={networkString.z}";
                }
                else
                {
                    valueToEncodeInJson = newValue;
                }
                
                string jsonEncode;
                if (autoJsonSerialization)
                {
                    jsonEncode = JsonConvert.SerializeObject(valueToEncodeInJson, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }
                else
                {
                    jsonEncode = (string)valueToEncodeInJson;
                }
                
                GRPC_NetVarUpdate result = new GRPC_NetVarUpdate()
                {
                    NetId = _netId, HashName = _variableHashName, NewValue = new GRPC_GenericValue {Type = _currentType, Value = jsonEncode }
                };

                await _sendStream.RequestStream.WriteAsync(result, _sendStreamCancellationTokenSource.Token);
                // Debug.Log($"Network Variable updated, send info to the grpcServer... Value : {newValue}");
            }
            catch (IOException)
            {
                GRPC_NetworkManager.instance.StopClient(); 
            }
        }

        public void Sync()
        {
            Debug.Log("Sync NetVar");
            
            UpdateVariableOnGrpc(Value);
        }

        private static GRPC_GenericType GetGrpcGenericType()
        {
            Type type = typeof(T);

            if (type == typeof(Int32))
            {
                return GRPC_GenericType.Int;
            }

            if (type == typeof(FixedString32Bytes) || type == typeof(FixedString64Bytes) || type == typeof(FixedString128Bytes))
            {
                return GRPC_GenericType.String;
            }
            
            if (type == typeof(bool))
            {
                return GRPC_GenericType.Bool;
            }

            if (type == typeof(Vector3))
            {
                return GRPC_GenericType.Vector3;
            }
            
            if (type == typeof(NetworkVector3Simplified))
            {
                return GRPC_GenericType.Vector3;
            }
            
            if (type == typeof(Quaternion))
            {
                return GRPC_GenericType.Vector3;
            }

            Debug.LogError($"The type '{type}' is not supported");
            return GRPC_GenericType.Isnull;
        }

        //Can't use dispose because it is called on
        //NetworkVariables by Netcode when a NetworkObject is despawned
        private void OnClientStop()
        {
            if (_isGrpcSync)
            {
                OnValueChanged -= OnValueChange_WriteInStream;
            }
            
            _sendStreamCancellationTokenSource?.Cancel();
            _sendStreamCancellationTokenSource?.Dispose();
            _sendStreamCancellationTokenSource = null;
            
            _sendRequireSyncCancellationTokenSource?.Cancel();
            _sendRequireSyncCancellationTokenSource?.Dispose();
            _sendRequireSyncCancellationTokenSource = null;
            
            _sendStream?.Dispose();
            _sendStream = null;

            _isGrpcSync = false;
        }
        
        private void OnUnrealClientConnected_WaitForNetVarSyncRequest(UnrealClient client)
        {
            _sendRequireSyncCancellationTokenSource = new CancellationTokenSource();
            AsyncServerStreamingCall<GRPC_NetVarUpdate> syncRequestStream = _client.GRPC_RequestNetVarUpdateGrpcToNetcode(new GRPC_NetVarUpdate {NetId = _netId, HashName = _variableHashName});

            WaitForNetVarSyncRequest(syncRequestStream);
        }

        private async void WaitForNetVarSyncRequest(AsyncServerStreamingCall<GRPC_NetVarUpdate> stream)
        {
            while (await stream.ResponseStream.MoveNext(_sendRequireSyncCancellationTokenSource.Token))
            {
                GRPC_NetVarUpdate response = stream.ResponseStream.Current;
                if (response.NetId == _netId && response.HashName == _variableHashName)
                {
                    Sync();
                    _sendRequireSyncCancellationTokenSource.Cancel();
                    _sendRequireSyncCancellationTokenSource.Dispose();
                    _sendRequireSyncCancellationTokenSource = null;
                }
                
            }
        }
    }
}
