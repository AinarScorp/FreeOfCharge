using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuScript : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
