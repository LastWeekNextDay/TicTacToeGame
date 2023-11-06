using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField SPgridSizeText;
    public TMP_InputField SPwinConditionText;
    public TMP_InputField MPHgridSizeText;
    public TMP_InputField MPHwinConditionText;
    public TMP_InputField MPHroomNameText;
    public TMP_InputField MPJroomNameText;

    public void StartGame()
    {
        if(SessionInfo.Instance.GridSize > 0 && SessionInfo.Instance.WinCondition > 0 && (SessionInfo.Instance.xo == "X" || SessionInfo.Instance.xo == "O"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MPJoinGame()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SinglePlayer()
    {
        SessionInfo.Instance.Multiplayer = false;
        SessionInfo.Instance.GridSize = int.TryParse(SPgridSizeText.text, out int num1) ? num1 : 0;
        SessionInfo.Instance.WinCondition = int.TryParse(SPwinConditionText.text, out int num2) ? num2 : 0;

        StartGame();
    }

    public void HostGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.GridSize = int.TryParse(MPHgridSizeText.text, out int num3) ? num3 : 0;
        SessionInfo.Instance.WinCondition = int.TryParse(MPHwinConditionText.text, out int num4) ? num4 : 0;
        SessionInfo.Instance.MultiplayerType = "Host";
        SessionInfo.Instance.RoomName = MPHroomNameText.text;
        StartGame();
    }

    public void JoinGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.MultiplayerType = "Join";
        SessionInfo.Instance.RoomName = MPJroomNameText.text;
        MPJoinGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetX()
    {
        SessionInfo.Instance.xo = "X";
    }

    public void SetO()
    {
        SessionInfo.Instance.xo = "O";
    }

    public void SetRand()
    {
        SessionInfo.Instance.xo = (Random.Range(0, 2) == 0) ? "X" : "O";
    }
}
