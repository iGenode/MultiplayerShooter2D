using Fusion;
using System.Collections;
using System.Collections.Generic;
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
            if (Mathf.Abs(networkInputData.movementInput.y) > Mathf.Abs(networkInputData.movementInput.x)) 
            {
                moveDirection = new Vector2(0, networkInputData.movementInput.y);
            } 
            else
            {
                moveDirection = new Vector2(networkInputData.movementInput.x, 0);
            }
            moveDirection.Normalize();
            _networkCharacterController.Move(moveDirection);
        }
    }
}
