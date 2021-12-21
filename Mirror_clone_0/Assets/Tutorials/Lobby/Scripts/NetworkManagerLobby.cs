using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;     // minimum amount of people required for starting game.
    [Scene] [SerializeField] private string menuScene = string.Empty; //draggable scene in inspector

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null; // reference to script NetworkRoomPlayerLobby

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>(); // list of the individual roomplayers
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>(); // list of the individual gameplayers. (gotten from roomplayer or potential spectators)

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList(); // show/search path to spawnableprefabs folder for host

    public override void OnStartClient() //show path to spawnableprefabs for client
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs"); // search path to spawnableprefabs folder for client

        foreach ( var prefab in spawnablePrefabs)       // foreach prefab in folder spawnableprefabs
        {
            NetworkClient.RegisterPrefab(prefab);         //originally ClientScene. Registered
        }
    }

    public override void OnClientConnect(NetworkConnection conn) // called on client when connected to a server.
    {
        base.OnClientConnect(conn);   //base logic

        OnClientConnected?.Invoke();  //raise event
    }

   
    public override void OnClientDisconnect(NetworkConnection conn) // called on clients when disconnected from server.
    {
        base.OnClientDisconnect(conn); //base logic
         
        OnClientDisconnected?.Invoke(); // raise event
    }

    public override void OnServerConnect(NetworkConnection conn)  // server full? check.
    {
        if (numPlayers >= maxConnections) // if more then maxconnections = (4).
        {
            conn.Disconnect();          
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)  // stops players from joining games that have started
        {
            conn.Disconnect();
            return;
        } 
    }

    public override void OnServerAddPlayer(NetworkConnection conn)  // when onconnectclient connects
    {
        if (SceneManager.GetActiveScene().path == menuScene) // if scene == mainmenu
        {
            bool isLeader = RoomPlayers.Count == 0;     // if playercount == 0 then you are the host (isLeader)

            NetworkRoomPlayerLobby roomPlayerinstance = Instantiate(roomPlayerPrefab); // instatiate roomplayerprefab (Lobby UI)

            roomPlayerinstance.IsLeader = isLeader;     // reference for roomplayerinstance so isleader stays the same

            NetworkServer.AddPlayerForConnection(conn, roomPlayerinstance.gameObject); // join server en keep your roomplayerinstance prefab yours.
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>(); // get clients roomplayerscript

            RoomPlayers.Remove(player); // remove player from list after disconnect

            NotifyPlayerOfReadyState(); // next method
        }

        base.OnServerDisconnect(conn); // destroys player for connection 
    }

    public override void OnStopServer() // base for stopping server en clearing the roomplayer list
    {
        RoomPlayers.Clear();
        // qeueu for new game, then roomplayer is allready cleared
    }

    public void NotifyPlayerOfReadyState()
    {
        foreach (var player in RoomPlayers) // loop over all Roomplayers
        {
            player.HandleReadyToStart(IsReadyToStart()); // next bool named IsReadyToStart
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers) { return false; } // current list size smaller then 2? bool = false

        foreach (var player in RoomPlayers) // loop over all roomplayers
        {
            if(!player.IsReady) { return false; } // if any player is not ready. bool = false;
        }

        return true;  // else true
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().path == menuScene) // if the scene == menuScene
        {
            if (!IsReadyToStart()) { return; } // not ready = return

            ServerChangeScene("Scene_Map_01"); // ready = scene change 
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        //from menu to game
        
        if(SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Scene_Map")) // if current scene = menuScene en new scene starts with, then.
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--) // go through all roomplayers
            {
                var conn = RoomPlayers[i].connectionToClient;   // get their connection
                var gameplayerInstance = Instantiate(gamePlayerPrefab); // spawn in gameplayerprefab
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName); // set displayname

                NetworkServer.Destroy(conn.identity.gameObject); // destroy their roomplayer 

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true); // assign gameplayerinstance to player instead of roomplayer
            }
        }
        base.ServerChangeScene(newSceneName); // change scene from networkmanager
    }

    public override void OnServerSceneChanged(string sceneName) // when loaded next scene
    {
        if (sceneName.StartsWith("Scene_Map"))      // if scene name is (correct)
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem); // spawn player spawnsystem
            NetworkServer.Spawn(playerSpawnSystemInstance);   // sync spawn for clients via server
        }
    }

    public override void OnServerReady(NetworkConnection conn) // networkmanager checking for connection
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
        Debug.Log(conn);
    }

}



