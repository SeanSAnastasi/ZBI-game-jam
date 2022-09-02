using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class MenuController : MonoBehaviour
{
    private NetworkManager networkManager;

    public GameObject entryScreen;
    public GameObject loginScreen;
    public GameObject lobbyScreen;

    public TMP_InputField nameInputField;
    public TMP_InputField roomInputField;

    public GameObject playerPrefab;
    public GameObject verticalPlayerList;
    public GameObject playerCountText;
    public GameObject startButton;

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

        if (PhotonNetwork.IsMasterClient) startButton.SetActive(true);
        else startButton.SetActive(false);

        UpdatePlayerList(networkManager.Players);
        GameObject listedPlayer = Instantiate(playerPrefab);
    }

    public void ExitLobby()
    {
        // Disconnect from the lobby.
        if (!networkManager.LeaveRoom()) return;

        entryScreen.SetActive(false);
        loginScreen.SetActive(true);
        lobbyScreen.SetActive(false);
    }

    public void UpdatePlayerList(Player[] players)
    {
        foreach (Transform child in verticalPlayerList.transform) Destroy(child.gameObject);

        foreach(Player player in players)
        {
            GameObject listedPlayer = Instantiate(playerPrefab);

            listedPlayer.transform.Find("Player Name").GetComponent<TextMeshProUGUI>().text = player.NickName;
            listedPlayer.transform.Find("Player Avatar").Find("Avatar Text").GetComponent<TextMeshProUGUI>().text = player.NickName.Substring(0, 1);

            listedPlayer.transform.SetParent(verticalPlayerList.transform);
        }

        playerCountText.GetComponent<TextMeshProUGUI>().text = players.Length.ToString();

        if (PhotonNetwork.IsMasterClient) startButton.SetActive(true);
        else startButton.SetActive(false);
    }

    public void StartGame()
    {
        // Restrict players from joining the current room.
        networkManager.RestrictPlayersJoiningRoom();
        networkManager.LoadScenePhoton("GameScene");
    }
}
