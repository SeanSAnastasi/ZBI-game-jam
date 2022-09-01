using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace ZBI {

    public class PlayerList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _content;
        private List<Player> _playerList = new List<Player>();

        public TextMeshProUGUI debugText;

        public void UpdatePlayersList()
        {
            //Instantiate the prefab in the dynamic list

        }

        #region PUN Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _playerList.Add(newPlayer);

            foreach (Player player in _playerList)
            {
                Debug.Log(player);
                debugText.text = player.NickName + " entered the room!";
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            foreach (Player player in _playerList)
            {
                if (otherPlayer.UserId == player.UserId)
                {
                    _playerList.Remove(player);
                    debugText.text = player.NickName + " left the room!";
                }
            }
        }

        #endregion

    }
}
