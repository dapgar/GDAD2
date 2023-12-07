using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenu : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Game(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
