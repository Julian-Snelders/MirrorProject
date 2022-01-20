using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string displayName = "Loading...";

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if(room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }


    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject); // dont destroy this gameobject on new scene load

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string newdisplayName)
    {
        displayName = newdisplayName;    // set displayname
    }



}
