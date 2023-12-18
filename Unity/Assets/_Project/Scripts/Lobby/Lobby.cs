using System;
using System.Linq;
using Project.Extensions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    enum ELobbyState
    {
        None,
        TeamSelection,
        CharacterSelection,
        Game
    }
    
    public class Lobby : NetworkSingleton<Lobby>
    {
        [SerializeField] private Pager _pager;
        private ELobbyState _lobbyState = ELobbyState.TeamSelection;
        public readonly Event onAllPlayersReadyEvent = new Event(nameof(onAllPlayersReadyEvent));


        protected override void Awake()
        {
            dontDestroyOnLoad = false;
            base.Awake();
        }

        private void Start()
        {
            GoToTeamSelectionPage();
        }
        
        public void SetPlayerReadyState(bool state)
        {
            SetPlayerReadyStateServerRpc((int)NetworkManager.Singleton.LocalClientId, state);
        }

        public void TogglePlayerReadyState()
        {
            SetPlayerReadyStateServerRpc((int)NetworkManager.Singleton.LocalClientId, !UserInstance.Me.IsReady);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyStateServerRpc(int playerId, bool state)
        {
            UserInstance userInstance = UserInstanceManager.instance.GetUserInstance(playerId);
            if (userInstance == null)
            {
                Debug.LogError("User instance is null");
                return;
            }
            
            userInstance.SetIsReady(state);

            if (userInstance.IsReady)
            {
                CheckIfAllPlayersReady();
            }
        }

        private void CheckIfAllPlayersReady()
        {
            if (UserInstanceManager.instance.GetUsersInstance().Count(x => x.IsReady) ==
                UserInstanceManager.instance.count)
            {
                OnAllPlayersReady();
            }
        }
        
        private void OnAllPlayersReady()
        {
            switch (_lobbyState)
            {
                case ELobbyState.TeamSelection:
                    // GoToCharacterSelectionPageServerRpc();
                    break;
                
                case ELobbyState.CharacterSelection:
                    GoToGameScene();
                    break;
                
                case ELobbyState.Game:
                    break;
                
                case ELobbyState.None:
                default:
                    Debug.LogError("Lobby State invalid state");
                    break;
            }
            
            onAllPlayersReadyEvent.Invoke(this, true);
        }

        // Don't need to be a RPC because it will be executed at the Start
        private void GoToTeamSelectionPage()
        {
            _pager.GoToPage(0);
            _lobbyState = ELobbyState.TeamSelection;
        }

        [ServerRpc]
        private void GoToCharacterSelectionPageServerRpc()
        {
            UserInstanceManager.instance.GetUsersInstance().ForEach(x => x.SetIsReady(false));
            GoToCharacterSelectionPageClientRpc();
        }
        
        [ClientRpc]
        private void GoToCharacterSelectionPageClientRpc()
        {
            _pager.GoToPage(1);
            _lobbyState = ELobbyState.CharacterSelection;
        }
        
        private void GoToGameScene()
        {
            LoadingScreenManager.Show(new LoadingScreenParameters(), null);
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
