using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
