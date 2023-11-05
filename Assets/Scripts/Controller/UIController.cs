using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI GamedataText;
    private GameLogic _gameLogic = null;
    public GameObject Loading;
    public GameObject GameTime;
    public GameObject Victory;
    public TMP_Text SceneText;
    public TMP_Text VictoryText;
    // Start is called before the first frame update
    void Start()
    {
        _gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OnConnectingScene(string text)
    {
        Loading.SetActive(true);
        SceneText.SetText(text);
        GameTime.SetActive(false);
        Victory.SetActive(false);
    }
    public void OnLoadingScene()
    {
        Loading.SetActive(true);
        SceneText.SetText("Waiting for other Player...");
        GameTime.SetActive(false);
        Victory.SetActive(false);
    }

    public void OffLoadingScene()
    {
        Loading.SetActive(false);
        GameTime.SetActive(true);
        Victory.SetActive(false);
    }

    public void SinglePlayerScene()
    {
        Loading.SetActive(false);
        GameTime.SetActive(true);
        Victory.SetActive(false);
    }

    public void ShowVictory()
    {
        Victory.SetActive(true);
        GameTime.SetActive(false);
        VictoryText.text = "Victory!!!";
    }

    public void ShowDefeat()
    {
        Victory.SetActive(true);
        GameTime.SetActive(false);
        VictoryText.text = "Defeat";
    }

    public void ShowDraw()
    {
        Victory.SetActive(true);
        GameTime.SetActive(false);
        VictoryText.text = "Draw";
    }

    public void ShowWinner(string winner)
    {
        Victory.SetActive(true);
        GameTime.SetActive(false);
        VictoryText.text = "Player " + winner + " Won";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        string yourTurn = "Now is Your Turn";
        string opponentTurn = "Now is Opponent Turn";
        if (_gameLogic.Turn == null) { return; }
        /*GamedataText.text = "Turn: " + _gameLogic.Turn.ToString();*/
        if (_gameLogic.Turn.GetComponent<PhotonView>() == null) {
            if (_gameLogic.Turn.AI) {
                GamedataText.text = opponentTurn;
            } else {
                GamedataText.text = yourTurn;
            }
        } else {
            if (_gameLogic.Turn.GetComponent<PhotonView>().IsMine)
            {
                GamedataText.text = yourTurn;
            }
            else
            {
                GamedataText.text = opponentTurn;
            }
        } 
    }
}
