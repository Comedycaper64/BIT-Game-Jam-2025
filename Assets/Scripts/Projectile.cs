using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool playerProjectile = false;
    private bool projectileActive = false;

    private int damage;
    private float speed;
    private Vector3 direction;

    [SerializeField]
    private Collider2D projectileCollider;

    [SerializeField]
    private SpriteRenderer projectileRenderer;

    [SerializeField]
    private GameObject projectileVisual;

    private void Awake()
    {
        ToggleProjectile(false);
    }

    private void Update()
    {
        if (!projectileActive)
        {
            return;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void ToggleProjectile(bool toggle)
    {
        projectileVisual.SetActive(toggle);
        projectileCollider.enabled = toggle;
        projectileActive = toggle;
    }

    public void Spawn(
        Vector2 direction,
        int damage,
        float speed,
        bool isPlayerProjectile,
        Sprite projectileSprite
    )
    {
        this.direction = direction;
        this.damage = damage;
        this.speed = speed;

        if (projectileSprite != null)
        {
            projectileRenderer.sprite = projectileSprite;
        }

        playerProjectile = isPlayerProjectile;

        ToggleProjectile(true);
    }

    public void Deactivate()
    {
        ToggleProjectile(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && (healthSystem.GetIsPlayer() != playerProjectile)
        )
        {
            healthSystem.TakeDamage(damage);
            Deactivate();
        }
        return;
    }
}
