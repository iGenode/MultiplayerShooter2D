using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Event")]
public class EmptyEvent : ScriptableObject
{
    public UnityAction Event;

    public void RaiseEvent()
    {
        Event?.Invoke();
    }
}
