using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private AssetHolder _assetHolder = null;
    public Networking NetworkManager = null;

    public bool GameActive = false;
    public Player Player1 = null;
    public Player Player2 = null;
    public Player Turn = null;
    public TicTacToeGrid Grid = null;
    public VictoryCalculator VictoryCalculator = null;

    // Start is called before the first frame update
    void Start()
    {
        _assetHolder = GameObject.Find("AssetHolder").GetComponent<AssetHolder>();
        int size = -1;
        int winCon = -1;
        if (SessionInfo.Instance.Multiplayer)
        {
            if (NetworkManager == null)
            {
                NetworkManager = Instantiate(_assetHolder.NetworkManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Networking>();
            }
            SetupMultiPlayer1();
            
        } else
        {
            size = SessionInfo.Instance.GridSize;
            winCon = SessionInfo.Instance.WinCondition;
            Grid = new TicTacToeGrid(_assetHolder, this);
            SetupSinglePlayer(size, winCon);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAssetHolder(AssetHolder assetHolder)
    {
        _assetHolder = assetHolder;
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
        OnChangeTurn();
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

    public void OnChangeTurn()
    {
        if (!SessionInfo.Instance.Multiplayer)
        {
            Player2.GetComponent<AI>().Reset();
        }
    }

    public void SetupSinglePlayer(int size, int winCon) 
    {
        Player1 = CreatePlayerSP(_assetHolder.HumanPlayerObjPrefab);
        Player2 = CreatePlayerSP(_assetHolder.AIPlayerObjPrefab);
        InitializeGame(size, winCon);
    }

    public void SetupMultiPlayer1()
    {
        Player1 = CreatePlayerMP(_assetHolder.HumanPlayerObjPrefab);
        Player2 = CreatePlayerMP(_assetHolder.HumanPlayerObjPrefab);
        
        StartCoroutine(SetupMultiPlayer2());
    }

    IEnumerator SetupMultiPlayer2()
    {
        yield return NetworkManager.RoomConnectionInitialization();
        Debug.Log("Host has joined!");
        Player1.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetPlayer(0));
        StartCoroutine(SetupMultiPlayer3());
    }

    IEnumerator SetupMultiPlayer3()
    {
        yield return NetworkManager.WaitForSecondPlayer();
        Debug.Log("Other player has joined!");
        Player2.gameObject.GetComponent<PhotonView>().TransferOwnership(NetworkManager.GetPlayer(1));
        int size = -1;
        int winCon = -1;
        if (SessionInfo.Instance.MultiplayerType == "Host")
        {
            size = SessionInfo.Instance.GridSize;
            winCon = SessionInfo.Instance.WinCondition;
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
            customProperties["GridSize"] = size;
            customProperties["WinCondition"] = winCon;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
            Grid = new TicTacToeGrid(_assetHolder, this);
        }
        else if (SessionInfo.Instance.MultiplayerType == "Join")
        {
            while (PhotonNetwork.CurrentRoom == null) {                 
                Debug.Log("Waiting for current room to be set...");
                yield return null;
            }
            while (PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == null && PhotonNetwork.CurrentRoom.CustomProperties["WinCondition"] == null)
            {
                Debug.Log("Waiting for grid size and win condition to be set...");
                yield return null;
            }
            size = (int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"];
            SessionInfo.Instance.GridSize = size;
            winCon = (int)PhotonNetwork.CurrentRoom.CustomProperties["WinCondition"];
            SessionInfo.Instance.WinCondition = winCon;
            Grid = new TicTacToeGrid(_assetHolder, this);
            Debug.Log("Grid size: " + SessionInfo.Instance.GridSize + ", Win condition: " + SessionInfo.Instance.WinCondition);
        }
        InitializeGame(SessionInfo.Instance.GridSize, SessionInfo.Instance.WinCondition);
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
