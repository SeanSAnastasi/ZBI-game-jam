using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ListedPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _listedPlayerText;
    public Player PlayerInfo { get; private set; }

    public void SetListedPlayerInfo(Player playerInfo)
    {
        PlayerInfo = playerInfo;
        _listedPlayerText.text = playerInfo.NickName;
    }
}
