using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour // for connecting with IP
{
    [SerializeField] private NetworkManagerLobby networkmanager = null; // reference networkmanager

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null; // reference landingpagepanel
    [SerializeField] private TMP_InputField ipAddressInputField = null; 
    [SerializeField] private Button joinButton = null;

    private void OnEnable() // if script == enabled
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;     
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable() // if script == disabled
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby() //press join lobby button
    {
        string ipAddress = ipAddressInputField.text; // text in inputfield = ipadress

        networkmanager.networkAddress = ipAddress;
        networkmanager.StartClient();  // start as client (needs networkAdress)

        joinButton.interactable = false; // disable join button
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true; // enable join button for user when goes to main menu

        gameObject.SetActive(false);       // disable this object
        landingPagePanel.SetActive(false); // disable landingpage (mainmenu) 
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true; //after disconnect or wrong IP, the joinbutton will be interactible again
    }




}
