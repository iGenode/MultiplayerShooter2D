using Fusion;
using System.Collections.Generic;
using UnityEngine;

// TODO: predictions and spawning on client first?
public class ProjectileHandler : NetworkBehaviour
{
    [Header("Effects")]
    [SerializeField]
    private GameObject _hitParticle;

    [Header("Collision detection")]
    [SerializeField]
    private LayerMask _layersToHit;

    // Fired by references
    private PlayerRef _firedByPlayerRef;

    // Reference of a projectile network object
    private NetworkObject _networkObject;

    // Maximum projectile lifetime timer
    private TickTimer _maxLifetimeTimer = TickTimer.None;

    // Hit info
    private List<LagCompensatedHit> _hits = new();

    // Constants
    // Projectile speed
    private const int ProjectileSpeed = 20;
    // Lifetime
    private const int LifetimeSeconds = 6;

    public void FireProjectile(PlayerRef firedByPlayerRef)
    {
        _networkObject = GetComponent<NetworkObject>();

        _firedByPlayerRef = firedByPlayerRef;

        _maxLifetimeTimer = TickTimer.CreateFromSeconds(Runner, LifetimeSeconds);
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += ProjectileSpeed * Runner.DeltaTime * transform.up;

        if (Object.HasStateAuthority)
        {
            // If lifetime expired
            if (_maxLifetimeTimer.ExpiredOrNotRunning(Runner))
            {
                // Shouldn't really happen
                Runner.Despawn(_networkObject);
                return;
            }

            // Check if the projectile has hit anything
            int hitCount = Runner.LagCompensation.OverlapSphere(transform.position, 0.175f, _firedByPlayerRef, _hits, _layersToHit, HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority);

            if (hitCount > 0)
            {
                // Check every hit
                foreach (LagCompensatedHit hit in _hits)
                {
                    // If hit is on a Hitbox
                    if (hit.Hitbox != null)
                    {
                        if (Object.HasStateAuthority)
                        {
                            hit.Hitbox.transform.root.GetComponent<HealthHandler>().OnTakeDamage();
                        }

                        Debug.Log($"Hit on {hit.Hitbox}");
                    }

                    Runner.Despawn(_networkObject);
                }
            }
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // TODO: particle should be a cone with direction oposite of the hit
        Instantiate(_hitParticle, transform.position, Quaternion.identity);
    }
}
