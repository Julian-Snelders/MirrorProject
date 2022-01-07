using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNameTag : NetworkBehaviour
{
   // [SerializeField] private Text nameText;
  //  private NetworkRoomPlayerLobby RoomPlayer;
    public override void OnStartAuthority()
    {
      
    //  if (isLocalPlayer)
    //        SetName();
   //     RoomPlayer = GetComponent<NetworkRoomPlayerLobby>();
    }

   // void SetName()
  //  {
      //  nameText.text = RoomPlayer.DisplayName;
   // }
    // Update is called once per frame
    void Update()
    {
        
    }
}
