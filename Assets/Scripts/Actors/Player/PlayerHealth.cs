using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private bool invincible = false;
    private float invincibilityDuration = 2f;
    private float impulseStrength = 1f;
    private PlayerManager playerManager;

    [SerializeField]
    private GameObject playerVisual;

    [SerializeField]
    private AudioClip playerDamageSFX;

    [SerializeField]
    private AudioClip playerDeathSFX;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private IEnumerator DamageInvincibility()
    {
        invincible = true;

        float invincibilityFlash = invincibilityDuration / 4f;

        for (int i = 0; i < 8; i++)
        {
            if ((i % 2) == 0)
            {
                playerVisual.SetActive(false);
                yield return new WaitForSeconds(invincibilityFlash * 0.25f);
            }
            else
            {
                playerVisual.SetActive(true);
                yield return new WaitForSeconds(invincibilityFlash * 0.75f);
            }
        }

        invincible = false;
    }

    public override void TakeDamage(int damageAmount)
    {
        if (invincible)
        {
            return;
        }

        impulseSource.GenerateImpulse(impulseStrength);

        if (!playerManager.TryDecrementPetalCounter(damageAmount))
        {
            AudioManager.PlaySFX(playerDeathSFX, 1f, 0, transform.position);
            playerManager.KillPlayer();
        }
        else
        {
            AudioManager.PlaySFX(playerDamageSFX, 1f, 0, transform.position);
            StartCoroutine(DamageInvincibility());
        }
    }
}
