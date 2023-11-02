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
        Grid = new TicTacToeGrid(_assetHolder, this);
        if (SessionInfo.Instance.Multiplayer)
        {
            if (NetworkManager == null)
            {
                NetworkManager = Instantiate(_assetHolder.NetworkManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Networking>();
                NetworkManager.GetComponent<PhotonView>().ViewID = 1;
            }
            this.AddComponent<PhotonView>();
            GetComponent<PhotonView>().ViewID = 2;
            StartCoroutine(SetupMultiPlayer());
            
        } else
        {
            size = SessionInfo.Instance.GridSize;
            winCon = SessionInfo.Instance.WinCondition;
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
        //int random = UnityEngine.Random.Range(0, 2);
        //if (random == 0)
        //
        Turn = Player1;
        Player1.Piece = "X";
        Player2.Piece = "O";
        //}
        //else
        //{
        //    Turn = Player2;
        //    Player2.Piece = "X";
        //    Player1.Piece = "O";
        //}
    }

    public void OnChangeTurn()
    {
        if (!SessionInfo.Instance.Multiplayer)
        {
            Player2.GetComponent<AI>().Reset();
        }
    }

    IEnumerator SetupMultiPlayer()
    {
        yield return NetworkManager.RoomConnectionInitialization();
        Debug.Log("Host has joined!");
        int size = -1;
        int winCon = -1;

        if (SessionInfo.Instance.MultiplayerType == "Host")
        {
            size = SessionInfo.Instance.GridSize;
            winCon = SessionInfo.Instance.WinCondition;
            while (this.GetComponent<PhotonView>().ViewID < 1)
            {
                PhotonNetwork.AllocateViewID(this.GetComponent<PhotonView>());
                Debug.Log("Waiting for view ID to be allocated...");
                yield return null;
            }
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
            customProperties["GridSize"] = size;
            customProperties["WinCondition"] = winCon;
            Debug.Log("Game Logic View ID: " + this.GetComponent<PhotonView>().ViewID);
            customProperties["GameLogicViewID"] = this.GetComponent<PhotonView>().ViewID;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
        yield return NetworkManager.WaitForSecondPlayer();
        Debug.Log("Other player has joined!");
        if (SessionInfo.Instance.MultiplayerType == "Host")
        {
            NetworkManager.SendGameLogicViewIDToClient(this.GetComponent<PhotonView>().ViewID);
        } else if (SessionInfo.Instance.MultiplayerType == "Join")
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
            while (PhotonNetwork.CurrentRoom.CustomProperties["GameLogicViewID"] == null || this.GetComponent<PhotonView>().ViewID != (int)PhotonNetwork.CurrentRoom.CustomProperties["GameLogicViewID"])
            {
                Debug.Log("Waiting for game logic to synchronize...");
                yield return null;
            }
            size = (int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"];
            SessionInfo.Instance.GridSize = size;
            winCon = (int)PhotonNetwork.CurrentRoom.CustomProperties["WinCondition"];
            SessionInfo.Instance.WinCondition = winCon;
            Grid = new TicTacToeGrid(_assetHolder, this);
            Debug.Log("Grid size: " + SessionInfo.Instance.GridSize + ", Win condition: " + SessionInfo.Instance.WinCondition);
        }
        if (SessionInfo.Instance.MultiplayerType == "Host")
        {
            Player1 = CreatePlayer(_assetHolder.HumanPlayerMPObjPrefab);
            NetworkManager.SendPlayerInfo(SessionInfo.Instance.MultiplayerType, Player1.GetComponent<PhotonView>());
        } else
        {
            Player2 = CreatePlayer(_assetHolder.HumanPlayerMPObjPrefab);
            NetworkManager.SendPlayerInfo(SessionInfo.Instance.MultiplayerType, Player2.GetComponent<PhotonView>());
        }
        while (Player1 == null || Player2 == null)
        {
            Debug.Log("Waiting for players to be set...");
            yield return null;
        }
        InitializeGame(SessionInfo.Instance.GridSize, SessionInfo.Instance.WinCondition);
    }

    public void MPPlayerCreation(string MultiplayerType, int viewID)
    {
        if (MultiplayerType == "Host")
        {
            Player1 = PhotonView.Find(viewID).gameObject.GetComponent<Player>();
        } else
        {
            Player2 = PhotonView.Find(viewID).gameObject.GetComponent<Player>();
        }
    }

    public void SetupSinglePlayer(int size, int winCon)
    {
        Player1 = CreatePlayer(_assetHolder.HumanPlayerObjPrefab);
        Player2 = CreatePlayer(_assetHolder.AIPlayerObjPrefab);
        InitializeGame(size, winCon);
    }

    Player CreatePlayer(GameObject prefab)
    {
        return _assetHolder.Spawn(prefab, Vector3.zero).GetComponent<Player>();
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
