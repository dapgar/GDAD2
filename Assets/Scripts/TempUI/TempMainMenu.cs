using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonClick;
   public void Game(int sceneID)
    {
        buttonClick.Play();
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    public void Exit()
    {
        buttonClick.Play();
        Application.Quit();
    }
}
