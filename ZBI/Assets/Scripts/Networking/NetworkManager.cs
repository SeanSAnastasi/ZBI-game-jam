using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    private bool isConnected = false;
    private static bool instanceExists = false;


    public bool IsConnected
    {
        get { return isConnected; }
    }

    public Player[] Players
    {
        get { return PhotonNetwork.PlayerList; }
    }


    private void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        isConnected = true;
        Debug.Log("Test log");
        ConnectToPhoton();
        DontDestroyOnLoad(gameObject);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server!");
        isConnected = true;
    }

    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public bool ConnectToLobby(string playerName, string roomName)
    {
        if (!PhotonNetwork.IsConnected) return false;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.PublishUserId = true;
        PhotonNetwork.LocalPlayer.NickName = playerName;

        return PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public bool LeaveRoom()
    {
        return PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        FindObjectOfType<MenuController>().UpdatePlayerList(PhotonNetwork.PlayerList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        FindObjectOfType<MenuController>().UpdatePlayerList(PhotonNetwork.PlayerList);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        FindObjectOfType<MenuController>().UpdatePlayerList(PhotonNetwork.PlayerList);
    }
}
