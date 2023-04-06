using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    // TODO: look into bits? instead of primitives
    public Vector2 MovementInput;
    public NetworkBool IsFireButtonPressed;
}
