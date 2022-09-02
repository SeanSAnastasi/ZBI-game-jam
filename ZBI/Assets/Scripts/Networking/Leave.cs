using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Leave : MonoBehaviourPunCallbacks
{


    //cashed
    Transform _roomText;

    // Start is called before the first frame update
    void Start()
    {
        _roomText = transform.parent.Find("RoomText");

        if (_roomText)
        {
            _roomText.gameObject.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Launcher");
    }
}
