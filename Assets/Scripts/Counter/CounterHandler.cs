using UnityEngine;
using Fusion;

public abstract class CounterHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnCountChanged))]
    [HideInInspector]
    public int Count { get; private set; } = 0;

    public int AddCount(int toAdd) => Count += toAdd;

    private static void OnCountChanged(Changed<CounterHandler> changed)
    {
        Debug.Log($"@{Time.time} OnCountChanged value {changed.Behaviour.Count}");

        int newCount = changed.Behaviour.Count;

        changed.LoadOld();

        int oldCount = changed.Behaviour.Count;

        if (newCount > oldCount)
        {
            changed.Behaviour.OnCountIncreased();
        }
        else if (newCount < oldCount)
        {
            changed.Behaviour.OnCountDecreased();
        }
    }

    public abstract void OnCountIncreased();


    public abstract void OnCountDecreased();
}
