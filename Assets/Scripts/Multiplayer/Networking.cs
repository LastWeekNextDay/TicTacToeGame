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
    public int PlayerCount = 0;
    public bool ConnectedToMaster = false;
    public bool InARoom = false;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(WaitForMaster());
        UpdatePlayerCount();
        UpdateRoomStatus();
    }

    private void UpdateRoomStatus()
    {
        InARoom = PhotonNetwork.InRoom;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ConnectedToMaster = true;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created Room " + PhotonNetwork.CurrentRoom.Name);
    }

    public Photon.Realtime.Player GetPlayer(int playerNumber)
    {
        return PhotonNetwork.PlayerList[playerNumber];
    }
    void UpdatePlayerCount()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        }
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
        //bool host = PlayerPrefs.GetInt("Host") == 1;
        bool host = true;
        if (host)
        {
            HostGame();
        } else
        {
            JoinGame();
        }
    }

    void HostGame()
    {
        // string name = PlayerPrefs.GetString("HostName");
        string name = "test";
        PhotonNetwork.CreateRoom(name, new RoomOptions { MaxPlayers = 2 });
    }

    void JoinGame()
    {
        //string name = PlayerPrefs.GetString("HostName");
        string name = "test";
        PhotonNetwork.JoinRoom(name);
    }
}
