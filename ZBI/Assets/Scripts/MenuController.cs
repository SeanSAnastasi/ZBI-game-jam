using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class MenuController : MonoBehaviour
{

    public GameObject entryScreen;
    public GameObject loginScreen;
    public GameObject lobbyScreen;

    public TMP_InputField nameInputField;
    public TMP_InputField roomInputField;


    public GameObject playerPrefab;

    public NetworkManager networkManager;

    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }


    public void NavigateToLogin()
    {
        entryScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        loginScreen.SetActive(true);
    }

    public void NavigateToLobby()
    {
        string playerName = nameInputField.text;
        string roomName = roomInputField.text;

        // TODO: nice to have
        // Display a notification that the player must enter a name and a room name.
        if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(roomName)) return;

        // connect to or create the lobby.
        if (!networkManager.ConnectToLobby(playerName, roomName)) return;

        entryScreen.SetActive(false);
        loginScreen.SetActive(false);

        lobbyScreen.SetActive(true);

        GameObject startButton = GameObject.Find("Start Button");
        if (PhotonNetwork.IsMasterClient) startButton.SetActive(true);
        else startButton.SetActive(false);
    }

    public void ExitLobby()
    {
        // TODO: disconnect from the lobby.
        if (!networkManager.LeaveRoom()) return;
        entryScreen.SetActive(false);
        loginScreen.SetActive(true);
        lobbyScreen.SetActive(false);
    }
}
