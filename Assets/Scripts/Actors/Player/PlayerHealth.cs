using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private PlayerManager playerManager;

    [SerializeField]
    private AudioClip playerDamageSFX;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public override void TakeDamage(int damageAmount)
    {
        AudioManager.PlaySFX(playerDamageSFX, 1f, 0, transform.position);

        if (!playerManager.TryDecrementPetalCounter(damageAmount))
        {
            playerManager.KillPlayer();
        }
    }
}
