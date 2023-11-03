using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    private GameLogic _gameLogic = null;
    // Start is called before the first frame update
    void Start()
    {
        _gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        turnText = GameObject.Find("TurnText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        if (_gameLogic.Turn == null) { return; }
        turnText.text = "Turn: " + _gameLogic.Turn.ToString();
        if (_gameLogic.Turn.GetComponent<PhotonView>() == null) { 
            if (_gameLogic.Turn.AI) { 
                turnText.color = Color.red; 
            } else { 
                turnText.color = Color.green; 
            }
        } else {
            if (_gameLogic.Turn.GetComponent<PhotonView>().IsMine)
            {
                turnText.color = Color.green;
            }
            else
            {
                turnText.color = Color.red;
            }
        } 
    }
}
