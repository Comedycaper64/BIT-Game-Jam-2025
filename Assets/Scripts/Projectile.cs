using System;
using Unity.Cinemachine;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool playerProjectile = false;
    private bool projectileActive = false;

    private int damage;
    private float speed;
    private float knockbackStrength = 10f;
    private float impulseStrength = 0.25f;
    private Vector3 direction;

    [SerializeField]
    private Collider2D projectileCollider;

    [SerializeField]
    private GameObject projectileVisual;

    [SerializeField]
    private GameObject projectileVisualEnemy;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    [SerializeField]
    private AudioClip wallHitSFX;

    [SerializeField]
    private LayerMask environmentLayermask;
    public static Action OnPlayerProjectileHit;
    public static EventHandler<int> OnPlayerProjectileExpended;

    private void Awake()
    {
        ToggleProjectile(false, false);
    }

    private void Update()
    {
        if (!projectileActive)
        {
            return;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void ToggleProjectile(bool toggle, bool isPlayer)
    {
        projectileVisual.SetActive(false);
        projectileVisualEnemy.SetActive(false);

        if (toggle)
        {
            projectileVisual.SetActive(isPlayer);
            projectileVisualEnemy.SetActive(!isPlayer);
        }

        projectileCollider.enabled = toggle;
        projectileActive = toggle;
    }

    public void Spawn(Vector2 direction, int damage, float speed, bool isPlayerProjectile)
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;

        playerProjectile = isPlayerProjectile;

        ToggleProjectile(true, playerProjectile);
    }

    public void Deactivate()
    {
        impulseSource.GenerateImpulse(impulseStrength);
        ToggleProjectile(false, playerProjectile);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && (healthSystem.GetIsPlayer() != playerProjectile)
        )
        {
            if (playerProjectile)
            {
                if (healthSystem.GetType() != typeof(PlantHealth))
                {
                    OnPlayerProjectileHit?.Invoke();
                }
                else
                {
                    OnPlayerProjectileExpended?.Invoke(this, 2);
                }

                if (other.TryGetComponent<EnemyMovement>(out EnemyMovement movement))
                {
                    movement.KnockbackEnemy(direction, knockbackStrength);
                    ParticleManager.SpawnParticles(transform.position, -direction);
                }
            }

            healthSystem.TakeDamage(damage);
            Deactivate();
            return;
        }

        if ((environmentLayermask & (1 << other.gameObject.layer)) != 0)
        {
            Deactivate();
            if (playerProjectile)
            {
                OnPlayerProjectileExpended?.Invoke(this, 1);
            }

            ParticleManager.SpawnParticles(transform.position, direction);
            AudioManager.PlaySFX(wallHitSFX, 1f, 0, transform.position);
            return;
        }
    }
}
