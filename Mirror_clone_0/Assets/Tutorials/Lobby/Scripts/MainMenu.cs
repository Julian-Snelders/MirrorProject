using UnityEngine;
using Mirror;
//using Steamworks;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null; // script networkmanagerlobby reference

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null; // gameobject landingpagepanel (lobby UI) reference.

   // [SerializeField] private bool useSteam = false;

   // protected Callback<LobbyCreated_t> lobbyCreated;
  //  protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
  //  protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start()
    {
     //   if (!useSteam) { return; }

     //   lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
     //   gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
    //    lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

}

    public void HostLobby() // press host button
    {
        landingPagePanel.SetActive(false); // disable landingPagePanel;

      //  if (useSteam)
     //   {
     //       SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 8);
     //       return;
     //   }
         
        networkManager.StartHost(); // start as host
    }

   /* private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress", SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();

        landingPagePanel.SetActive(false);
    }
   */
}
