using UnityEngine;
using UnityEngine.Events;

public interface IGameOverCondition
{
    UnityAction<GameObject> ConditionMetEvent { get; set; }

    void CheckCondition();
}
