using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    [SerializeField]
    private GameObject _mobileInputUI;

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            // Logic for local player
            Local = this;

            Debug.Log("Spawned local player");
        }
        else
        {
            // Logic for remote player
            // Hide controls of other players
            _mobileInputUI.SetActive(false);

            Debug.Log("Spawned remote player");
        }

        gameObject.name = $"Player {Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        // If client is still on the server (has input authority) - despawn the player that left
        if (player.IsValid == Object.HasInputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
