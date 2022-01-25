using UnityEngine;
using Mirror;
//using Steamworks;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null; // script networkmanagerlobby reference

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null; // gameobject landingpagepanel (lobby UI) reference.


    public void HostLobby() // press host button
    {
        landingPagePanel.SetActive(false); // disable landingPagePanel;


        networkManager.StartHost(); // start as host
    }

}
