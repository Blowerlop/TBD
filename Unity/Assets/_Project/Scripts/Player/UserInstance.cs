using System;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// Stores client's specific infos.
    /// </summary>
    public class UserInstance : NetworkBehaviour
    {
        /// <summary>
        /// Reference to the local client's UserInstance.
        /// </summary>
        [ClearOnReload] public static UserInstance Me;
        
        public CharacterRefs LinkedCharacter { get; private set; }
        public event Action<CharacterRefs> OnCharacterLinked;
        
        //NetVars
        [ShowInInspector] private GRPC_NetworkVariable<int> _networkClientId = new("ClientId");
        
        [ShowInInspector] private GRPC_NetworkVariable<FixedString64Bytes> _networkPlayerName = new("Name", value: "UnknowName");
        [ShowInInspector] private GRPC_NetworkVariable<int> _networkTeam = new("Team", value: -1);
        [ShowInInspector] private GRPC_NetworkVariable<bool> _networkIsMobile = new("IsMobile");
        [ShowInInspector] private GRPC_NetworkVariable<bool> _networkIsReady = new("IsReady");
        [ShowInInspector] private NetworkVariable<int> _networkCharacter = new();
        
        public int ClientId => _networkClientId.Value;
        
        public string PlayerName => _networkPlayerName.Value.ToString();
        public int Team => _networkTeam.Value;
        public bool IsMobile => _networkIsMobile.Value;
        public bool IsReady => _networkIsReady.Value;
        public int CharacterId => _networkCharacter.Value;
        
        public override void OnNetworkSpawn()
        {
            InitializeNetworkVariables();

            if (IsClient)
            {
                _networkClientId.OnValueChanged += OnClientIdChanged;
                _networkTeam.OnValueChanged += OnTeamChanged;
            }
            
            if (!IsOwner) return;

            Me = this;
        }
        
        public override void OnNetworkDespawn()
        {
            ResetNetworkVariables();

            if (IsClient)
            {
                if(UserInstanceManager.instance) UserInstanceManager.instance.ClientUnregisterUserInstance(this);
                _networkClientId.OnValueChanged -= OnClientIdChanged;
                _networkTeam.OnValueChanged -= OnTeamChanged;
            }
            
            if (!IsOwner) return;

            Me = null;
        }

        private void InitializeNetworkVariables()
        {
            if (!IsServer && !IsHost) return;
            
            _networkClientId.Initialize();
            _networkPlayerName.Initialize();
            _networkTeam.Initialize();
            _networkIsMobile.Initialize();
        }

        private void ResetNetworkVariables()
        {
            if (!IsServer && !IsHost) return;
            
            _networkClientId.Reset();
            _networkPlayerName.Reset();
            _networkTeam.Reset();
            _networkIsMobile.Reset();
        }
        
        public void LinkCharacter(CharacterRefs refs)
        {
            Debug.LogError("LinkCharacter");
            LinkedCharacter = refs;
            
            OnCharacterLinked?.Invoke(refs);
        }

        public void UnlinkCharacter()
        {
            LinkedCharacter = null;
            
            // Really useful ?
            OnCharacterLinked?.Invoke(null);
        }
        
        private void OnClientIdChanged(int oldValue, int newValue)
        {
            // Should only happen once when user instance is spawned
            
            UserInstanceManager.instance.ClientRegisterUserInstance(this);
        }
        
        private void OnTeamChanged(int oldValue, int newValue)
        {
            TeamManager.instance.ClientOnTeamChanged(this, oldValue, newValue);
        }
        
        #region Setters
        
        public void SetClientId(int clientId)
        {
            if (!IsServer && !IsHost) return;
            
            _networkClientId.Value = clientId;
        }
        
        [ServerRpc(RequireOwnership = false), Button]
        public void SetNameServerRpc(string playerName)
        {
            _networkPlayerName.Value = playerName;
        }
        
        [ServerRpc(RequireOwnership = false), Button]
        public void SetTeamServerRpc(int playerTeam)
        {
            _networkTeam.Value = playerTeam;
        }

        [ServerRpc(RequireOwnership = false), Button]
        public void SetIsMobileServerRpc(bool isMobile)
        {
            _networkIsMobile.Value = isMobile;
        }
        
        [ServerRpc(RequireOwnership = false), Button]
        public void SetIsReadyServerRpc(bool isReady)
        {
            _networkIsReady.Value = isReady;
        }
        
        [ServerRpc(RequireOwnership = false), Button]
        public void SetCharacterServerRpc(int characterId)
        {
            _networkCharacter.Value = characterId;
        }

        #endregion
    }
}