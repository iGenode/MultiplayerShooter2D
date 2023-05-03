using System.Linq;
using UnityEngine;

public class PlayerListUIHandler : MonoBehaviour
{
    [Header("List container")]
    [SerializeField]
    private RectTransform _contentContainer;

    [Header("Player Item")]
    [SerializeField]
    private PlayerItem _playerItemPrefab;

    // Making sure that this component subscribes to GameOverEvent even if disabled
    public PlayerListUIHandler()
    {
        GameManager.GameOverEvent += ShowSummary;
    }

    // Fill the list with items
    private void ShowSummary(GameObject winner)
    {
        Debug.Log("Showing summary");
        AddPlayer(winner);

        // TODO: implement coin counting

        // Filling the player list with players other than the winner
        foreach (GameObject playerObject in PlayerList.Players.Where(pl => pl != winner)) {
            AddPlayer(playerObject);
        }
    }

    // Instantiating a UI list element that represents this player
    private void AddPlayer(GameObject player)
    {
        var playerItem = Instantiate(_playerItemPrefab, _contentContainer);
        var playerRef = player.GetComponent<NetworkPlayer>();
        playerItem.SetNickname(playerRef.Nickname.Value);
        playerItem.SetKills(playerRef.GetComponent<KillCounterHandler>().Count);
        // TODO: set other properties
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing from gameOverEvent in PlayerListUIHandler");
        GameManager.GameOverEvent -= ShowSummary;
    }
}
