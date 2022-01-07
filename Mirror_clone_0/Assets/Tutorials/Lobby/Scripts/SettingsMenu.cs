using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI, resumeButton, optionsButton, QuitButton, SensitivitySliderUI, 
                      backButton, cursor;

    public void Update()
    {
        if (GameIsPaused == false && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
      
    }

    public void Pause()
    {
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;   
        pauseMenuUI.SetActive(true);
        cursor.SetActive(false);
    }

    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        cursor.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Settings()
    {
        resumeButton.SetActive(false);
        optionsButton.SetActive(false);
        QuitButton.SetActive(false);

        SensitivitySliderUI.SetActive(true);
        backButton.SetActive(true);
    }
    public void Back()
    {
        resumeButton.SetActive(true);
        optionsButton.SetActive(true);
        QuitButton.SetActive(true);

        SensitivitySliderUI.SetActive(false);
        backButton.SetActive(false);
    }
 

    public void QuitApplication()
    {
        Application.Quit();
    }
}
