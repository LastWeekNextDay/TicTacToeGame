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
            Debug.Log("Waiting to connect to master...");
            yield return null;
        }
        while (!PhotonNetwork.InRoom)
        {
            Debug.Log("Waiting to connect to room...");
            yield return null;
        }
        while (PhotonNetwork.CurrentRoom.PlayerCount < 1)
        {
            Debug.Log("Waiting for host to join room...");
            yield return null;
        }
    }

    public IEnumerator WaitForSecondPlayer()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Debug.Log("Waiting for other player to join...");
            yield return null;
        }
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log("Player " + player.Value.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
        }
    }

    void HostGame()
    {
        // string name = PlayerPrefs.GetString("HostName");
        string name = "test";
        PhotonNetwork.CreateRoom(name, new RoomOptions { MaxPlayers = 2, IsVisible = true });
    }

    void JoinGame()
    {
        //string name = PlayerPrefs.GetString("HostName");
        string name = "test";
        PhotonNetwork.JoinRoom(name);
    }

    public void SendGameLogicViewIDToClient(int viewID)
    {
        photonView.RPC("ReceiveGameLogicViewID", RpcTarget.Others, viewID);
    }

    public void SendPlayerInfo(string multiplayerType, PhotonView playerView)
    {
        photonView.RPC("ReceivePlayer", RpcTarget.Others, multiplayerType, playerView.ViewID);
    }

    [PunRPC]
    public void ReceiveGameLogicViewID(int viewID)
    {
        GameObject.Find("GameLogic").GetComponent<PhotonView>().ViewID = viewID;
    }

    [PunRPC]
    public void ReceivePlayer(string multiplayerType, int playerViewID)
    {
        GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        gameLogic.MPPlayerCreation(multiplayerType, playerViewID);
    }
}
