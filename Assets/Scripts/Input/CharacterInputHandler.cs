using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    private Vector2 _moveInputVector = Vector2.zero;
    private bool _isFireButtonPressed = false;

    CharacterMovementHandler _characterMovementHandler;

    private void Awake()
    {
        _characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    private void Update()
    {
        // Checking if character has input authority to prevent running this for all players 
        if (!_characterMovementHandler.Object.HasInputAuthority)
        {
            return;
        }
        // TODO: change input to touch
        // Movement input
        _moveInputVector.x = Input.GetAxis("Horizontal");
        _moveInputVector.y = Input.GetAxis("Vertical");

        // Fire input
        if (Input.GetButtonDown("Fire1"))
        {
            _isFireButtonPressed = true;
        }
    }

    public NetworkInputData GetNetworkInput()
    {
        // Creating a struct to pass input data to the server
        NetworkInputData networkInputData = new()
        {
            // Movement data
            MovementInput = _moveInputVector,
            // Fire data
            IsFireButtonPressed = _isFireButtonPressed
        };

        // Resetting variables after using their data
        _isFireButtonPressed = false;

        return networkInputData;
    }
}
