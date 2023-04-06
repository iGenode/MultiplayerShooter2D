using Fusion;
using UnityEngine;

public class CharacterMovementHandler : NetworkBehaviour
{
    private NetworkCharacterControllerCustom _networkCharacterController;

    private void Awake()
    {
        _networkCharacterController = GetComponent<NetworkCharacterControllerCustom>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

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
}
