using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private AssetHolder _assetHolder = null;
    public GameObject NetworkManager = null;

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
        //bool multiplayer = PlayerPrefs.GetInt("Multiplayer") == 1;
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        Multiplayer = true;
        MakeSureAssetHolderIsNotNull();
        if (Multiplayer)
        {
            SetupMultiPlayer1(3, 3);
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

    public void SetAssetHolder(AssetHolder assetHolder)
    {
        _assetHolder = assetHolder;
    }

    void MakeSureAssetHolderIsNotNull()
    {
        if (_assetHolder == null)
        {
            _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
            if (Grid == null)
            {
                Grid = new TicTacToeGrid(_assetHolder, this);
            } else
            {
                Grid.SetAssetHolder(_assetHolder);
            }
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

    public void SetupMultiPlayer1(int size, int winCon)
    {
        if (NetworkManager == null)
        {
            NetworkManager = Instantiate(_assetHolder.NetworkManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        GameObject player1 = Instantiate(_assetHolder.HumanPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player1.AddComponent<PhotonView>();
        GameObject player2 = Instantiate(_assetHolder.HumanPlayerObjPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player2.AddComponent<PhotonView>();
        Player1 = player1.GetComponent<Player>();
        Player2 = player2.GetComponent<Player>();
        
        StartCoroutine(SetupMultiPlayer2(size, winCon));
    }

    IEnumerator SetupMultiPlayer2(int size, int winCon)
    {
        while (!NetworkManager.GetComponent<Networking>().ConnectedToMaster && !NetworkManager.GetComponent<Networking>().InARoom)
        {
            yield return null;
        }
        Player1.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetComponent<Networking>().GetPlayer(0));
        StartCoroutine(SetupMultiPlayer3(size, winCon));
    }

    IEnumerator SetupMultiPlayer3(int size, int winCon)
    {
        while (NetworkManager.GetComponent<Networking>().PlayerCount < 2)
        {
            Debug.Log("Waiting for other player to join...");
            yield return null;
        }
        Debug.Log("Other player has joined!");
        //Player2.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetComponent<Networking>().GetPlayer(1));
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
