public class PlayerHealth : HealthSystem
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public override void TakeDamage(int damageAmount)
    {
        if (!playerManager.TryDecrementPetalCounter(damageAmount))
        {
            playerManager.KillPlayer();
        }
    }
}
