using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Local = this;

            Debug.Log("Spawned local player");
        }
        else
        {
            Debug.Log("Spawned remote player");
        }

        gameObject.name = $"Player {Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        // TODO: is this the right way? was if (player == Object.InputAuthority)
        if (player.IsValid == Object.HasInputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
