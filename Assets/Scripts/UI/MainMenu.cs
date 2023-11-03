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
        SessionInfo.Instance.Multiplayer = false;
        SessionInfo.Instance.GridSize = 3;
        SessionInfo.Instance.WinCondition = 3;
        StartGame();
    }

    public void HostGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.GridSize = 5;
        SessionInfo.Instance.WinCondition = 5;
        SessionInfo.Instance.MultiplayerType = "Host";
        StartGame();
    }

    public void JoinGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.MultiplayerType = "Join";
        StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
