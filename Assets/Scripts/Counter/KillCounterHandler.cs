using UnityEngine;

public class KillCounterHandler : CounterHandler
{
    public override void OnCountIncreased()
    {
        Debug.Log($"@{Time.time} OnKillCounterIncreased of {gameObject}");
    }

    public override void OnCountDecreased()
    {
        Debug.Log($"@{Time.time} OnKillCounterDecreased of {gameObject}");
    }
}
