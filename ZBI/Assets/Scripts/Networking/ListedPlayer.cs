using UnityEngine;
using TMPro;
using Photon.Realtime;



    public class ListedPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _listedPlayerText;
        public Player Player { get; private set; }

        public void SetListedPlayerInfo(Player player)
        {
            Player = player;
            _listedPlayerText.text = player.NickName;
        }
    }
