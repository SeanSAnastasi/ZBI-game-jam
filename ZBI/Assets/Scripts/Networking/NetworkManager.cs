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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Entered room");
        GetComponent<MenuController>().UpdatePlayerList(PhotonNetwork.PlayerList);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetComponent<MenuController>().UpdatePlayerList(PhotonNetwork.PlayerList);
    }
}
