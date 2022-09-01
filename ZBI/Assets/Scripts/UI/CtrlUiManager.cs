using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CtrlGameUiManager : MonoBehaviour
{
    public TMP_InputField questionInput;
    public TMP_InputField promptInput;

    private PhotonView photonView;

    void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    // public void OnSubmitPrompt() //this is attached to a button
    // {
    //   string prompt = promptInput.GetComponent<TMP_InputField>().text;

    //   //TODO: send promt to master
    //   Debug.Log("sending promt to master");
    //   photonView.RPC("SendPromtToMaster", RpcTarget.MasterClient, prompt);
    // }

    // public void OnSubmitQuestion() //this is attached to a button
    // {
    //   string question = questionInput.GetComponent<TMP_InputField>().text;

    //   //TODO: send promt to master
    //   Debug.Log("sending promt to master");
    //   photonView.RPC("SendPromtToMaster", RpcTarget.MasterClient, question);
    // }

    // [PunRPC]
    // private void SendPromtToMaster(string prompt)
    // {
    //     if (!PhotonNetwork.IsMasterClient) return;
    //     Debug.Log("receiving promt at master");

    //     //TODO: save prompt, Generate Sprites and send to the right player

    //     Debug.Log("Sending generatedstuff to everyone");
    //     photonView.RPC("GetDataFromMaster", RpcTarget.All, null);
    // }
}
