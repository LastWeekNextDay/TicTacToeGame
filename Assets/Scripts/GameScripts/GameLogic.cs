using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private AssetHolder _assetHolder = null;
    public Networking NetworkManager = null;

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
        Multiplayer = PlayerPrefs.GetInt("Multiplayer") == 1;
        if (Multiplayer)
        {
            if (NetworkManager == null)
            {
                NetworkManager = Instantiate(_assetHolder.NetworkManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Networking>();
            }
        }
        int size = PlayerPrefs.GetInt("GridSize");
        int winCon = PlayerPrefs.GetInt("WinCondition");
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        Grid = new TicTacToeGrid(_assetHolder, this);
        MakeSureAssetHolderIsNotNull();
        if (Multiplayer)
        {
            SetupMultiPlayer1(size, winCon);
        } else
        {
            SetupSinglePlayer(size, winCon);
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
        Turn = (Turn == Player1) ? Player2 : Player1;
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
        Player1 = CreatePlayerSP(_assetHolder.HumanPlayerObjPrefab);
        Player2 = CreatePlayerSP(_assetHolder.AIPlayerObjPrefab);
        InitializeGame(size, winCon);
    }

    public void SetupMultiPlayer1(int size, int winCon)
    {
        Player1 = CreatePlayerMP(_assetHolder.HumanPlayerObjPrefab);
        Player2 = CreatePlayerMP(_assetHolder.HumanPlayerObjPrefab);
        
        StartCoroutine(SetupMultiPlayer2(size, winCon));
    }

    IEnumerator SetupMultiPlayer2(int size, int winCon)
    {
        yield return NetworkManager.RoomConnectionInitialization();
        Debug.Log("Host has joined!");
        Player1.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetPlayer(0));
        StartCoroutine(SetupMultiPlayer3(size, winCon));
    }

    IEnumerator SetupMultiPlayer3(int size, int winCon)
    {
        yield return NetworkManager.WaitForSecondPlayer();
        Debug.Log("Other player has joined!");
        Player2.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetPlayer(1));
        InitializeGame(size, winCon);
    }

    Player CreatePlayerSP(GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
    }

    Player CreatePlayerMP(GameObject prefab)
    {
        Player player = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
        player.gameObject.GetComponent<PhotonView>().OwnershipTransfer = OwnershipOption.Takeover;
        player.gameObject.GetComponent<PhotonView>().ViewID = NetworkManager.CurrentID;
        return player;
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
