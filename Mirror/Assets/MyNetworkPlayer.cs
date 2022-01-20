using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    public TMP_Text gamertag; // this is the UI that floats above the head of players.
    public Camera cam;

    public void Update()
    {
      //  if (!hasAuthority) { return; }

      //  if(cam.isActiveAndEnabled)
       // gamertag.text = GetComponent<NetworkManagerLobby>().gamertag;
        //gamertag.text = name;
    }


}
