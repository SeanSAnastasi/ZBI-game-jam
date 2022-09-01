using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Leave : MonoBehaviourPunCallbacks
{


    //cashed
    GameObject _roomText = null;

    // Start is called before the first frame update
    void Start()
    {
        _roomText = transform.Find("RoomText").gameObject;

        _roomText.GetComponent<TMP_InputField>().text = PhotonNetwork.CurrentRoom.Name;
    }

    void OnLeave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Launcher");
    }
}
