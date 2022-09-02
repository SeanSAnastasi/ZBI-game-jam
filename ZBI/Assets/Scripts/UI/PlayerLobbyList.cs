using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyList : MonoBehaviour
{
    void OnEnable() 
    {
        MenuController menuController = FindObjectOfType<MenuController>();
        menuController.UpdatePlayerList(FindObjectOfType<NetworkManager>().Players);
    }
}
