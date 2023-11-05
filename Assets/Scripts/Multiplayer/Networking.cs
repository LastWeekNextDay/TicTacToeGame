using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Net;
using Photon.Realtime;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Http;


public class Networking : MonoBehaviourPunCallbacks
{
    public bool ConnectedToMaster = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(WaitForMaster());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master");
        ConnectedToMaster = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ConnectedToMaster = false;
        Debug.Log("Disconnected from server for reason " + cause.ToString());
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created Room " + PhotonNetwork.CurrentRoom.Name);
        //PhotonNetwork.JoinRoom(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed to create room: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room: " + message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Player " + newPlayer.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
    }

    public Photon.Realtime.Player GetPlayer(int playerNumber)
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Players.TryGetValue(playerNumber, out var player))
        {
            return player;
        }
        return null;

    }

    IEnumerator WaitForMaster() {         
        while (!ConnectedToMaster)
        {
            yield return null;
        }
        HostOrConnect();
    }
    void HostOrConnect()
    {
        if (SessionInfo.Instance.MultiplayerType == "Host")
        {
            
            HostGame();
        } else
        {
            
            JoinGame();
        }
    }

    public IEnumerator RoomConnectionInitialization()
    {
        while (!ConnectedToMaster)
        {
            string message = "Waiting to connect to master...";
            Debug.Log(message);
            UIController control = GameObject.Find("UIController").GetComponent<UIController>();
            control.OnConnectingScene(message);
            yield return null;
        }
        while (!PhotonNetwork.InRoom)
        {
            string message = "Waiting to connect to room...";
            Debug.Log(message);
            UIController control = GameObject.Find("UIController").GetComponent<UIController>();
            control.OnConnectingScene(message);
            yield return null;
        }
        while (PhotonNetwork.CurrentRoom.PlayerCount < 1)
        {
            string message = "Waiting for host to join room...";
            Debug.Log(message);
            UIController control = GameObject.Find("UIController").GetComponent<UIController>();
            control.OnConnectingScene(message);
            yield return null;
        }
    }

    public IEnumerator WaitForSecondPlayer()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Debug.Log("Waiting for other player to join...");
            UIController control = GameObject.Find("UIController").GetComponent<UIController>();
            control.OnLoadingScene();
            yield return null;
        }
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log("Player " + player.Value.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
            UIController control = GameObject.Find("UIController").GetComponent<UIController>();
            control.OffLoadingScene();
        }
    }

    void HostGame()
    {
        // string name = PlayerPrefs.GetString("HostName");
        string name = SessionInfo.Instance.RoomName;
        PhotonNetwork.CreateRoom(name, new RoomOptions { MaxPlayers = 2, IsVisible = true });
    }

    void JoinGame()
    {
        //string name = PlayerPrefs.GetString("HostName");
        string name = SessionInfo.Instance.RoomName;
        PhotonNetwork.JoinRoom(name);
    }

    void SetPlayerPiece(string piece)
    {
        Debug.Log("[Networking] About to set player piece to: " + piece);
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
    {
        { "Piece", piece }
    };

        if (PhotonNetwork.LocalPlayer == null)
        {
            Debug.LogError("[Networking] PhotonNetwork.LocalPlayer is null.");
            return;
        }

        if (PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps))
        {
            Debug.Log("[Networking] Successfully called SetCustomProperties.");
        }
        else
        {
            Debug.LogError("[Networking] SetCustomProperties call failed.");
        }
    }

    // Callback for when custom properties are updated
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Piece"))
        {
            Debug.Log("[Networking] Player piece updated to: " + changedProps["Piece"].ToString());
        }
    }





    public void SendGameLogicViewIDToClient(int viewID)
    {
        photonView.RPC("ReceiveGameLogicViewID", RpcTarget.Others, viewID);
    }

    public void SendPlayerInfo(string multiplayerType, PhotonView playerView)
    {
        photonView.RPC("ReceivePlayer", RpcTarget.Others, multiplayerType, playerView.ViewID);
    }

    public void ChangeTurnTo(int playerViewID)
    {
        photonView.RPC("TurnChange", RpcTarget.Others, playerViewID);
    }

    public void PlayExplosion(float x, float y, float z)
    {
        photonView.RPC("Explosion", RpcTarget.Others, x, y, z);
    }

    public void PlaySound(float x, float y, float z)
    {
        photonView.RPC("Sound", RpcTarget.Others, x, y, z);
    }

    public void AssignPiece(string ox)
    {
        photonView.RPC("Playerox", RpcTarget.Others, ox);
    }

    [PunRPC]
    void ReceiveGameLogicViewID(int viewID)
    {
        GameObject.Find("GameLogic").GetComponent<PhotonView>().ViewID = viewID;
    }

    [PunRPC]
    void ReceivePlayer(string multiplayerType, int playerViewID)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        gameLogic.MPPlayerCreation(multiplayerType, playerViewID);
    }

    [PunRPC]
    void TurnChange(int playerViewID)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        gameLogic.Turn = PhotonView.Find(playerViewID).gameObject.GetComponent<Player>();
    }

    [PunRPC]
    void Explosion(float x, float y, float z)
    {
        StartCoroutine(Slot.PlayExplosion(new Vector3(x, y, z)));
    }

    [PunRPC]
    void Sound(float x, float y, float z)
    {
        StartCoroutine(Slot.PlaySound(new Vector3(x, y, z)));
    }

    [PunRPC]
    void GameEnd(String winner)
    {
        // This code will be executed on all clients
        UIController uiController = GameObject.Find("UIController").GetComponent<UIController>();
        if (uiController != null)
        {
                uiController.ShowWinner(winner);
        }
    }
    [PunRPC]
    void Playerox(string ox)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    
        gameLogic.Player1.Piece = ox;
        gameLogic.Turn = gameLogic.Player1;
        if (ox == "X")
        {
            gameLogic.Player2.Piece = "O";
        }
        else
        {
            gameLogic.Player2.Piece = "X";
      }
    }
}
