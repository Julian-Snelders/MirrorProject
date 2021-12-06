using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public GameObject JoinButtonScreen;
    public void quitgame()
    {
        Application.Quit();
    }

    public void Back()
    {
        JoinButtonScreen.SetActive(false);
    }

}
