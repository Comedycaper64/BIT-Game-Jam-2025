using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private const int PETAL_ATTACK_COST = 1;
    private bool playerDead = false;
    private bool weaponAvailable = false;
    private float weaponRechargeSpeed = 1f;
    private float weaponRechargeTimer = 1f;
    private float weaponRechargeTime = 1f;

    [SerializeField]
    private PlayerStats stats;

    [SerializeField]
    private PlayerManager playerManager;

    [SerializeField]
    private AudioClip playerShoot;

    private void Start()
    {
        InputManager.Instance.OnAttackEvent += TryAttack;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= TryAttack;
    }

    private void Update()
    {
        if (playerDead)
        {
            return;
        }

        if (!weaponAvailable)
        {
            weaponRechargeTimer += Time.deltaTime * weaponRechargeSpeed;

            if (weaponRechargeTimer >= weaponRechargeTime)
            {
                weaponAvailable = true;
                weaponRechargeTimer = 0f;
            }
        }
    }

    private void TryAttack()
    {
        if (playerDead)
        {
            return;
        }

        if (!weaponAvailable)
        {
            return;
        }

        playerManager.TryDecrementPetalCounter(PETAL_ATTACK_COST);

        ProjectileManager.SpawnProjectile(
            transform.position,
            GetMouseDirection(),
            stats.GetAttackDamage(),
            stats.GetProjectileSpeed(),
            true
        );

        AudioManager.PlaySFX(playerShoot, 1f, 0, transform.position);

        weaponRechargeSpeed = stats.GetAttackSpeed();

        weaponAvailable = false;
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0;

        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        return mouseDirection;
    }
}
