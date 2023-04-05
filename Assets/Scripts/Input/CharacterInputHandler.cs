using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    private Vector2 _moveInputVector = Vector2.zero;

    private void Update()
    {
        // Movement input
        _moveInputVector.x = Input.GetAxis("Horizontal");
        _moveInputVector.y = Input.GetAxis("Vertical");
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.movementInput = _moveInputVector;

        return networkInputData;
    }
}
