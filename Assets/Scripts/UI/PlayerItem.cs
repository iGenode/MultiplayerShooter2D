using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nicknameText;
    [SerializeField]
    private TextMeshProUGUI _killsText;
    [SerializeField]
    private TextMeshProUGUI _coinsText;

    public void SetNickname(string nickname) => _nicknameText.text = nickname;

    public void SetKills(int count) => _killsText.text = count.ToString();

    public void SetCoins(int count) => _coinsText.text = count.ToString();
}
