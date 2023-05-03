using UnityEngine;
using UnityEngine.Events;

public class GenericEvent<T> : ScriptableObject
{
    public UnityAction<T> Event;

    public void RaiseEvent(T t)
    {
        Event?.Invoke(t);
    }
}
