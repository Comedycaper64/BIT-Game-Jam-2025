using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool playerProjectile = false;
    private bool projectileActive = false;

    private int damage;
    private float speed;
    private float knockbackStrength = 5f;
    private Vector3 direction;

    [SerializeField]
    private Collider2D projectileCollider;

    [SerializeField]
    private GameObject projectileVisual;

    [SerializeField]
    private GameObject projectileVisualEnemy;

    [SerializeField]
    private LayerMask environmentLayermask;
    public static Action OnPlayerProjectileHit;
    public static Action OnPlayerProjectileExpended;

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
                    OnPlayerProjectileExpended?.Invoke();
                }

                if (other.TryGetComponent<EnemyMovement>(out EnemyMovement movement))
                {
                    movement.KnockbackEnemy(direction, knockbackStrength);
                }
            }

            healthSystem.TakeDamage(damage);
            Deactivate();
            return;
        }

        if ((environmentLayermask & (1 << other.gameObject.layer)) != 0)
        {
            Deactivate();
            OnPlayerProjectileExpended?.Invoke();
            //Maybe particle effect for hitting wall?
            return;
        }
    }
}
