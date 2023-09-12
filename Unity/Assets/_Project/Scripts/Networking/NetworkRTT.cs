using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class NetworkRTT : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<float> _networkCurrentRTT = new NetworkVariable<float>();
        public float networkCurrentRTT { get => _networkCurrentRTT.Value; private set => _networkCurrentRTT.Value = value; }
        private float _currentRTT;
        
        private ClientRpcParams _clientRpcParams;
        private float start = 0.0f;
        private float end = 0.0f;


        private void Start()
        {
            if (enabled) return;
            
            enabled = false;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsOwner == false || (IsServer && IsClient == false))
            {
                enabled = false;
                return;
            }
            
            _clientRpcParams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { OwnerClientId } }
            };
            
            Debug.Log("a");
            enabled = true;
        }
        
        private void FixedUpdate()
        {
            if (Time.realtimeSinceStartup - start > 0.5f)
            {
                _currentRTT = Mathf.Round(end - start);
                start = Time.realtimeSinceStartup;
                PingServerRpc(networkCurrentRTT);
            }
            
        }

        [ServerRpc]
        private void PingServerRpc(float currentRTT)
        {
            Debug.Log("Ping : " + start);
            networkCurrentRTT = currentRTT;
            PongClientRpc(_clientRpcParams);
        }

        [ClientRpc]
        private void PongClientRpc(ClientRpcParams clientRpcParams = default)
        {
            end = Time.realtimeSinceStartup;
            Debug.Log("Pong : " + end);
        }
    }
}
