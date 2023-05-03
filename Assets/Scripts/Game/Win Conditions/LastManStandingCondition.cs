using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Last man standing game over condition
/// </summary>
public class LastManStandingCondition : IGameOverCondition
{
    public UnityAction<GameObject> ConditionMetEvent { get; set; }

    public void CheckCondition()
    {
        // If the game has started and is not over
        if (GameManager.IsGameStarted && !GameManager.IsGameOver)
        {
            Debug.Log($"Checking Last Man Standing condition, player count is {PlayerList.AlivePlayerCount}");
            // If one player is still alive
            if (PlayerList.AlivePlayerCount == 1)
            {
                // Finding a player that is still alive
                ConditionMetEvent.Invoke(PlayerList.Players.Find(player => !player.GetComponent<HealthHandler>().IsDead));
            }
        }
    }
}
