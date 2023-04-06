using Fusion;
using System.Collections;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [Header("Projectiles")]
    [SerializeField]
    private ProjectileHandler _projectile;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem _fireParticle;

    private NetworkObject _networkObject;
    private HealthHandler _healthHandler;

    [Networked(OnChanged = nameof(OnFireChanged))]
    [HideInInspector]
    public bool IsFiring { get; set; }

    private float _lastTimeFired = 0;
    private const float FireCooldown = 0.3f;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
        _healthHandler = GetComponent<HealthHandler>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!_healthHandler.IsDead)
        {
            if (GetInput(out NetworkInputData networkInputData))
            {
                if (networkInputData.IsFireButtonPressed)
                {
                    Fire(transform.up);
                }
            }
        }
    }

    private void Fire(Vector3 aimDirection)
    {
        if (Time.time - _lastTimeFired < FireCooldown)
        {
            return;
        }

        StartCoroutine(FireNotification());

        Runner.Spawn(_projectile, transform.position + aimDirection * 0.7f, transform.rotation, Object.InputAuthority, (runner, spawnedProjectile) =>
        {
            spawnedProjectile.GetComponent<ProjectileHandler>().FireProjectile(Object.InputAuthority);
        });

        _lastTimeFired = Time.time;
    }

    // FIXME: Kinda sketchy
    // Notifying all clients that fire was pressed
    private IEnumerator FireNotification()
    {
        IsFiring = true;

        _fireParticle.Play();

        yield return new WaitForSeconds(0.09f);

        IsFiring = false;
    }

    static void OnFireChanged(Changed<WeaponHandler> changed)
    {
        //Debug.Log($"@{Time.time} OnFireChanged, value is {changed.Behaviour.IsFiring}");

        // Did the player fire
        bool isCurrentlyFiring = changed.Behaviour.IsFiring;

        // Get the old value
        changed.LoadOld();

        // Was the player firing before
        bool wasFiringBefore = changed.Behaviour.IsFiring;
        // If the player just fired and was not already firing - fire on the server
        if (isCurrentlyFiring && !wasFiringBefore)
        {
            changed.Behaviour.OnFireRemote();
        }
    }

    private void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
        {
            _fireParticle.Play();
        }
    }
}
