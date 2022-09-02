using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace ZBI
{
    public class LobbyCanvas : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [Header("Debug")]
        [SerializeField] private bool _canStart;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(() => NetworkedGameLogic.Instance.StartGame());
        }

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

                if(PhotonNetwork.CurrentRoom.PlayerCount == 4 || _canStart)
                {
                    _startGameButton.interactable = true;
                }
            }
        }
    }
}
