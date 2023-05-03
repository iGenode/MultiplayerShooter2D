using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager
{
    public static UnityAction GameStartedEvent;
    public static UnityAction<GameObject> GameOverEvent;

    public static bool IsGameStarted { get; private set; } = false;
    public static bool IsGameOver { get; private set; } = false;

    public GameManager(IGameOverCondition condition)
    {
        condition.ConditionMetEvent += GameOver;
    }

    static GameManager()
    {
        PlayerList.PlayerListChanged += ShouldStart;
    }

    private static void ShouldStart(List<GameObject> playerList)
    {
        if (!IsGameStarted && !IsGameOver && playerList.Count >= 2) 
        {
            IsGameStarted = true;
            GameStartedEvent?.Invoke();
        }
    }

    private static void GameOver(GameObject winner)
    {
        IsGameOver = true;
        GameOverEvent?.Invoke(winner);
    }
}
