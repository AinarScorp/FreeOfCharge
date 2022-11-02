using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Player.Driver;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    bool GameIsPaused = false;

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] ColorController colorController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
     pauseMenuUI.SetActive(false);
     Time.timeScale = 1f;
     GameIsPaused = false;
     ToggleContorls(true);

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        ToggleContorls(false);
    }

    void ToggleContorls(bool value)
    {
        if (colorController ==null) return;

        colorController.enabled = value;
        
    }

}
