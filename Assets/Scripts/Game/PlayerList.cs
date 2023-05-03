using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Static class used to keep track of players in the game and notify on changes
/// </summary>
public static class PlayerList
{
    public static UnityAction<GameObject> PlayerAdded;
    public static UnityAction<GameObject> PlayerRemoved;
    public static UnityAction<List<GameObject>> PlayerListChanged;

    public static readonly List<GameObject> Players = new();
    public static int AlivePlayerCount { get; private set; } = 0;

    public static void AddToList(GameObject gameObject)
    {
        Debug.Log($"Adding player {gameObject} to PlayerList");
        AlivePlayerCount++;
        Players.Add(gameObject);
        PlayerAdded?.Invoke(gameObject);
        PlayerListChanged?.Invoke(Players);
    }

    public static void RemoveFromList(GameObject gameObject)
    {
        Debug.Log($"Removing player {gameObject} from PlayerList");
        AlivePlayerCount--;
        Players.Remove(gameObject);
        PlayerRemoved?.Invoke(gameObject);
        PlayerListChanged?.Invoke(Players);
    }

    public static void PlayerDied()
    {
        AlivePlayerCount--;
        PlayerListChanged?.Invoke(Players);
    }
}
