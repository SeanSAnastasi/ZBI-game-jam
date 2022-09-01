using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace ZBI
{
    public class LobbyCanvas : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;

        private void Start()
        {
            _startGameButton.interactable = false;
            _startGameButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _startGameButton.gameObject.SetActive(true);

                if(PhotonNetwork.CurrentRoom.PlayerCount == 4)
                {
                    _startGameButton.interactable = true;
                }
            }
        }
    }
}
