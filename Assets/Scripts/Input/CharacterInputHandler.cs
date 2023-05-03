using UnityEngine;
using UnityEngine.UI;

public class CharacterInputHandler : MonoBehaviour
{
    [Header("Player input")]
    [SerializeField]
    private JoystickControls _joystickInput;
    [SerializeField]
    private Button _fireButton;

    private Vector2 _moveInputVector = Vector2.zero;
    private bool _isFireButtonPressed = false;

    private CharacterMovementHandler _characterMovementHandler;

    private void Awake()
    {
        _characterMovementHandler = GetComponent<CharacterMovementHandler>();

        // TODO: remove listener somewhere?
        _fireButton.onClick.AddListener(FireViaButton);
    }

    private void Start()
    {
        // If character has input authority
        if (_characterMovementHandler.Object.HasInputAuthority)
        {
            // Subscribe to input events
            _joystickInput.OnMove += MoveViaJoystick;
        }
    }

    private void MoveViaJoystick(Vector2 move)
    {
        _moveInputVector.x = move.x;
        _moveInputVector.y = move.y;
    }

    private void FireViaButton()
    {
        _isFireButtonPressed = true;
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
