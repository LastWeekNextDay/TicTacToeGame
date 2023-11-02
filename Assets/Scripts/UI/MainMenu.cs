using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SinglePlayer()
    {
        PlayerPrefs.SetInt("MultiPlayer", 0);
        StartGame();
    }

    public void HostGame()
    {
        PlayerPrefs.SetInt("MultiPlayer", 1);
        PlayerPrefs.SetInt("GridSize", 3);
        PlayerPrefs.SetInt("WinCondition", 3);
        PlayerPrefs.SetString("MultiPlayer", "Host");
        StartGame();
    }

    public void JoinGame()
    {
        PlayerPrefs.SetInt("MultiPlayer", 1);
        PlayerPrefs.SetString("MultiPlayer", "Join");
        StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
