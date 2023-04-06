using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHealthChanged))]
    [HideInInspector]
    public byte Health { get; set; }

    [Networked(OnChanged = nameof(OnLifecycleChanged))]
    [HideInInspector]
    public bool IsDead { get; set; }
    // Starting health
    private const byte _startingHealth = 5;
    // Initialization flag
    private bool _isInitialized = false;

    [Header("Hit UI")]
    [SerializeField]
    private Color _onHitColor;
    [SerializeField]
    private Image _onHitImage;

    [Header("Hit sprite")]
    [SerializeField]
    private SpriteRenderer _playerSprite;

    private Color _defaultPlayerColor;

    private HitboxRoot _hitboxRoot;
    private CharacterMovementHandler _characterMovement;

    private void Awake()
    {
        _hitboxRoot = GetComponentInChildren<HitboxRoot>();
        _characterMovement = GetComponentInChildren<CharacterMovementHandler>();
    }

    private void Start()
    {
        Health = _startingHealth;
        IsDead = false;

        _defaultPlayerColor = _playerSprite.material.color;

        _isInitialized = true;
    }

    private IEnumerator OnHit()
    {
        // Flash the screen and character on hit
        int flashCount = 5;
        while (flashCount > 0)
        {
            // Set character and ui as hit color
            _playerSprite.material.color = _onHitColor;
            if(Object.HasInputAuthority)
            {
                _onHitImage.color = _onHitColor;
            }

            yield return new WaitForSeconds(0.2f);

            // Reset the color
            _playerSprite.material.color = _defaultPlayerColor;
            if (Object.HasInputAuthority && !IsDead)
            {
                _onHitImage.color = default;
            }

            flashCount--;
        }
    }

    // Only called on the server
    public void OnTakeDamage()
    {
        // Dismiss if already ded
        if (IsDead)
        {
            return;
        }
        
        // Take a hit
        Health--;

        Debug.Log($"@{Time.time} player {transform.name} took damage, {Health} left");

        // Player died
        if (Health <= 0)
        {
            Debug.Log($"@{Time.time} player {transform.name} died");
            IsDead = true;
        }
    }


    static void OnHealthChanged(Changed<HealthHandler> changed)
    {
        Debug.Log($"@{Time.time} OnHealthChanged value {changed.Behaviour.Health}");

        // Get current health
        byte newHealth = changed.Behaviour.Health;

        // Load old health
        changed.LoadOld();

        // Get old health
        byte oldHealth = changed.Behaviour.Health;

        // If took damage - handle the change
        if (newHealth < oldHealth)
        {
            changed.Behaviour.OnHealthReduced();
        }
    }

    private void OnHealthReduced()
    {
        if (!_isInitialized)
        {
            return;
        }

        StartCoroutine(OnHit());
    }

    static void OnLifecycleChanged(Changed<HealthHandler> changed)
    {
        Debug.Log($"@{Time.time} OnLifecycleChanged value {changed.Behaviour.IsDead}");
        // Is player currently dead
        bool isDead = changed.Behaviour.IsDead;

        // Load old lifecycle state
        changed.LoadOld();

        // Was player dead before
        bool wasDead = changed.Behaviour.IsDead;

        // If player just died - handle death
        if (isDead && !wasDead)
        {
            changed.Behaviour.OnDeath();
        }
    }

    private void OnDeath()
    {
        // Hide the player
        _playerSprite.gameObject.SetActive(false);
        // Disable player's hitbox
        _hitboxRoot.HitboxRootActive = false;
        // Disable player's movement
        _characterMovement.SetCharacterControllerEnabled(false);

        // TODO: death effects
    }
}
