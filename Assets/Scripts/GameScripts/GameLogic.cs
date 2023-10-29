using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private AssetHolder _assetHolder = null;

    public bool Multiplayer = false;
    public bool GameActive = false;
    public Player Player1 = null;
    public Player Player2 = null;
    public Player Turn = null;
    public TicTacToeGrid Grid = null;
    public VictoryCalculator VictoryCalculator = null;

    // Start is called before the first frame update
    void Start()
    {
        //int size = PlayerPrefs.GetInt("GridSize");
        //int winCon = PlayerPrefs.GetInt("WinCondition");
        MakeSureAssetHolderIsNotNull();
        if (Multiplayer)
        {
            SetupMultiPlayer(3, 3);
        } else
        {
            SetupSinglePlayer(3, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MakeSureAssetHolderIsNotNull();
    }

    void MakeSureAssetHolderIsNotNull()
    {
        if (_assetHolder == null)
        {
            _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
            Grid = new TicTacToeGrid(_assetHolder, this);
            VictoryCalculator?.SetGrid(Grid);
        }
    }

    public void InitializeGame(int gridSize, int winCondition)
    {
        Grid.SetupGrid(gridSize);
        Camera.main.GetComponent<CameraScript>().SetupCamera();
        VictoryCalculator = new VictoryCalculator(Grid, winCondition);
        RandomizeFirstGoer();
        GameActive = true;
    }

    public void ChangeTurn()
    {
        if (Turn == Player1)
        {
            Turn = Player2;
        }
        else
        {
            Turn = Player1;
        }
    }

    void RandomizeFirstGoer() {        
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            Turn = Player1;
            Player1.Piece = "X";
            Player2.Piece = "O";
        }
        else
        {
            Turn = Player2;
            Player2.Piece = "X";
            Player1.Piece = "O";
        }
    }

    public void SetupSinglePlayer(int size, int winCon) 
    {
        GameObject player1 = Instantiate(_assetHolder.HumanPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject player2 = Instantiate(_assetHolder.AIPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Player1 = player1.GetComponent<Player>();
        Player2 = player2.GetComponent<Player>();
        InitializeGame(size, winCon);
    }

    public void SetupMultiPlayer(int size, int winCon)
    {
        GameObject player1 = Instantiate(_assetHolder.HumanPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject player2 = Instantiate(_assetHolder.HumanPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Player1 = player1.GetComponent<Player>();
        Player2 = player2.GetComponent<Player>();
        InitializeGame(size, winCon);
    }

    public void OnPiecePlaced(int x, int y, Player player)
    {
        Debug.Log("Player " + player.Piece + " placed a piece on " + x + ", " + y);
        if (VictoryCalculator.ValueHasWon(x, y))
        {
            Debug.Log("Player " + player.Piece + " has won!");
            GameActive = false;
            return;
        }
        if (VictoryCalculator.GameIsTied())
        {
            Debug.Log("Game is tied!");
            GameActive = false;
            return;
        }
        ChangeTurn();
    }
}
