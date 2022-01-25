using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    public TMP_Text gamertag; // this is the UI that floats above the head of players.

    public Camera cam;       

    [SyncVar]
    public string playername = null;        //empty string

    public override void OnStartAuthority()
    {
        if(cam.isActiveAndEnabled)
        CmdSetDisplayName(PlayerNameInput.DisplayName); // Takes the playername from the NetworkGamePlayerlobby (player before spawn) and sets it to the spawned player
    }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        playername = displayName;                    // string playername is the same as displayname from NetworkGamePlayerLobby.
    }

    public void Update()
    {
        gamertag.text = playername;                 // string reads playername

    }


}
