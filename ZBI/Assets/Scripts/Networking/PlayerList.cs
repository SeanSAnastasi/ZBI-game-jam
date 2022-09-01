using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace ZBI {

    public class PlayerList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ListedPlayer _listedPlayer;
        [SerializeField] private Transform _content;

        private List<ListedPlayer> _playerList = new List<ListedPlayer>();

        public TextMeshProUGUI debugText;

        private void Awake()
        {
            GetCurrentPlayersInRoom();
        }

        private void GetCurrentPlayersInRoom()
        {
            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddListedPlayer(playerInfo.Value);
            }
        }

        private void AddListedPlayer(Player player)
        {
            ListedPlayer listedPlayer = Instantiate(_listedPlayer, _content);
            if (listedPlayer != null)
            {
                listedPlayer.SetListedPlayerInfo(player);
                _playerList.Add(listedPlayer);
            }
        }

        public void UpdatePlayersList(List<Player> playerList)
        {

            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }

            //Instantiate the prefab in the dynamic list
            foreach (Player player in playerList)
            {
                //Added to the list
                ListedPlayer listedPlayer = Instantiate(_listedPlayer, _content);
                if (listedPlayer != null)
                {
                    listedPlayer.SetListedPlayerInfo(player);
                }

                Debug.Log(player.NickName);
            }
        }

        #region PUN Callbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddListedPlayer(newPlayer);
            debugText.text = newPlayer.NickName + " entered the room!";
            //_playerList.Add(newPlayer);
            //foreach (Player player in _playerList)
            //{
            //    Debug.Log(player);
            //}

            //UpdatePlayersList(_playerList);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

            //Removed from the list
            int index = _playerList.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
            {
                Destroy(_playerList[index].gameObject);
                _playerList.RemoveAt(index);
                debugText.text = otherPlayer.NickName + " left the room!";
            }

            //foreach (Player player in _playerList)
            //{
            //    if (otherPlayer.NickName == player.NickName)
            //    {
            //        _playerList.Remove(player);
            //    }
            //}

            //UpdatePlayersList(_playerList);
        }


        #endregion

    }
}
