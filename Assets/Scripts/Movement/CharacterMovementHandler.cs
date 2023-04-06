using Fusion;
using UnityEngine;

public class CharacterMovementHandler : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _networkCharacterController;
    private HealthHandler _healthHandler;

    private void Awake()
    {
        _networkCharacterController = GetComponent<NetworkCharacterControllerCustom>();
        _healthHandler = GetComponent<HealthHandler>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (_healthHandler.IsDead)
            {
                return;
            }
        }

        if (GetInput(out NetworkInputData networkInputData))
        {
            // Move only one direction at a time
            Vector2 moveDirection;
            if (Mathf.Abs(networkInputData.MovementInput.y) > Mathf.Abs(networkInputData.MovementInput.x))
            {
                moveDirection = new Vector2(0, networkInputData.MovementInput.y);
            }
            else
            {
                moveDirection = new Vector2(networkInputData.MovementInput.x, 0);
            }
            moveDirection.Normalize();
            _networkCharacterController.Move(moveDirection);
        }
    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        _networkCharacterController.Controller.enabled = isEnabled;
    }
}
