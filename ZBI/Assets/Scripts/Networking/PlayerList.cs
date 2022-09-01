using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerList : MonoBehaviourPunCallbacks
{
    
    public List<Player> playerList;

    public void UpdatePlayersList()
    {
        //Instantiate the prefab in the dynamic list
    }

    #region PUN Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);

        foreach (Player player in playerList)
        {
            Debug.Log(player);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        foreach (Player player in playerList)
        {
            if(otherPlayer.UserId == player.UserId)
            {
                playerList.Remove(player);
            }
        }
    }

    #endregion



}
