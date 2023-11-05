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
    public TMP_InputField SPHxoText;
    public TMP_InputField MPHgridSizeText;
    public TMP_InputField MPHwinConditionText;
    public TMP_InputField MPHroomNameText;
    public TMP_InputField MPHxoText;
    public TMP_InputField MPJroomNameText;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SinglePlayer()
    {
        SessionInfo.Instance.Multiplayer = false;
        SessionInfo.Instance.GridSize = int.Parse(SPgridSizeText.text);
        SessionInfo.Instance.WinCondition = int.Parse(SPwinConditionText.text);
        SessionInfo.Instance.xo = SPHxoText.text;
        StartGame();
    }

    public void HostGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.GridSize = int.Parse(MPHgridSizeText.text);
        SessionInfo.Instance.WinCondition = int.Parse(MPHwinConditionText.text);
        SessionInfo.Instance.MultiplayerType = "Host";
        SessionInfo.Instance.RoomName = MPHroomNameText.text;
        SessionInfo.Instance.xo = MPHxoText.text;
        StartGame();
    }

    public void JoinGame()
    {
        SessionInfo.Instance.Multiplayer = true;
        SessionInfo.Instance.MultiplayerType = "Join";
        SessionInfo.Instance.RoomName = MPJroomNameText.text;
        StartGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
