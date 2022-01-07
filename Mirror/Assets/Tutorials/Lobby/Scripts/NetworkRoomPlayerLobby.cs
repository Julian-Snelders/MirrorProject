using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class NetworkRoomPlayerLobby : NetworkBehaviour // script per player
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;                     // lobbyUI off by default except if its me
    [SerializeField] public TMP_Text[] playerNameTexts = new TMP_Text[8];  // playernames
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[8]; // colored ready text
    [SerializeField] private Button startGameButton = null;                 // startgame button disabled. only host can start

    [SerializeField] private Button CopyIp; // button for copying public IP Adress

    [SyncVar(hook = nameof(HandleDisplayNameChanged))] // syncvar = only updated by server and then updated to clients 
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader; // = host. reference made in NetworkManagerLobby
    public bool IsLeader   // define bool
    {
        set
        {
            isLeader = value; // isLeader = IsLeader
            startGameButton.gameObject.SetActive(value); // activate when value true/ otherwise false
            CopyIp.interactable = true;         // copy ip button true
        }
    }

    private NetworkManagerLobby room; // variable

    private NetworkManagerLobby Room // set variable
    {
        get
        {
            if (room != null) { return room; } // return as room. if null?
            return room = NetworkManager.singleton as NetworkManagerLobby; // create it if null
        }
    }

    public override void OnStartAuthority() // gets called on the object that belongs to you
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName); //set your playername in lobby

        lobbyUI.SetActive(true);   // activate lobby UI
    }

    public override void OnStartClient() // start function for all clients / host
    {
        Room.RoomPlayers.Add(this);  // add this client to RoomPlayers list

        UpdateDisplay(); // next method
    }

    public override void OnStopClient() // function called when player leaves
    {
        Room.RoomPlayers.Remove(this); // remove them from list

        UpdateDisplay(); // next method
    }

    public void HandleReadyStatusChanged(bool oldValue, bool NewValue) => UpdateDisplay(); // update display method

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay(); // update display method

    private void UpdateDisplay()
    {
        if (!hasAuthority) // if its not our player thats updating
        {

            foreach (var player in Room.RoomPlayers) // find the player with authority
            {
                if (player.hasAuthority)  // found that player
                {
                    player.UpdateDisplay(); // update their display
                    break;
                }
            }

            return;
        }

        //if we have authority :
        for (int i = 0; i < playerNameTexts.Length; i++) // loop through UI texts for players
        {
            playerNameTexts[i].text = "Waiting For Player...";  // empty = Waiting for player...
            playerReadyTexts[i].text = string.Empty;            // ready up string = empty
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)   // loop through players 
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName; // set name in UI text
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?   // set ready up text with color and meaning
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart) // update ready status for all clients
    {
        if (!isLeader) { return; } // only leader (host) sees this

        startGameButton.interactable = readyToStart;  //ineractible true or false depending on bool readytostart
    }

    [Command]
    private void CmdSetDisplayName(string displayName) // when server gets the displayname
    {
        DisplayName = displayName;  // set it
    }

    [Command]
    public void CmdReadyUp() // ready up button
    {
        IsReady = !IsReady; // set bool true/false per click

        Room.NotifyPlayerOfReadyState(); // notify other players onclick
    }

    [Command]
    public void CmdStartGame() // start game button
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; } // extra check if this person is the leader

        Room.StartGame(); // start game
    }

    public void CopyIPAdress() // copy public IP button
    {
        string externalip = new WebClient().DownloadString("http://icanhazip.com"); //get string from this site (Site gives String with your public IP)
        TextEditor textEditor = new TextEditor();                                   // create text editor.
        textEditor.text = externalip;                                               // text editor is string from site (IP)
        textEditor.SelectAll();                                                     // select whole string
        textEditor.Copy();                                                          // copy this string to clipboard
    }
}
