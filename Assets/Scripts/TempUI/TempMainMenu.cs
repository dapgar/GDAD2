using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempMainMenu : MonoBehaviour
{
    public void Game(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
