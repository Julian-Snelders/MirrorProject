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
    public string playername = null;

    public override void OnStartAuthority()
    {

        if(cam.isActiveAndEnabled)
        CmdSetDisplayName(PlayerNameInput.DisplayName);
    }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        playername = displayName;
    }

    public void Update()
    {
        gamertag.text = playername;

    }


}
