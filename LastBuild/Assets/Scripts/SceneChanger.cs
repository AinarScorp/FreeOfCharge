using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    int _index;
    [SerializeField] Animation _animation;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(int index)
    {
        _index = index;
        SceneManager.LoadScene(_index);
        Time.timeScale = 1;
        Debug.Log("asdasd");
    }

    void SceneChange()
    {
    }
}
