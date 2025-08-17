using System;
using UnityEngine;

public enum PlayerBloom
{
    injured,
    leaf,
    bud,
    flower
}

public class PlayerManager : MonoBehaviour
{
    private int petalCounter = 0;
    private int petalStartAmount = 4;

    private const int PETAL_CAP = 15;
    private const int PETAL_BUD_THRESHOLD = 5;
    private const int PETAL_BLOOM_THRESHOLD = 10;

    private PlayerBloom playerBloom = PlayerBloom.leaf;
    private PlayerStats playerStats;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator playerHeadAnimator;

    [SerializeField]
    private AudioClip bloomSFX;

    public static EventHandler<bool> OnPlayerDead;
    public static EventHandler<int> OnNewPetalCount;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        petalCounter = petalStartAmount;
        UpdateBloomStatus();
    }

    private void OnEnable()
    {
        Projectile.OnPlayerProjectileHit += PlayerProjectileHit;
        Projectile.OnPlayerProjectileExpended += PlayerProjectileExpended;
    }

    private void OnDisable()
    {
        Projectile.OnPlayerProjectileHit -= PlayerProjectileHit;
        Projectile.OnPlayerProjectileExpended -= PlayerProjectileExpended;
    }

    private void UpdateBloomStatus(bool petalIncrease = false)
    {
        OnNewPetalCount?.Invoke(this, petalCounter);

        playerAnimator.SetBool("injured", false);
        playerHeadAnimator.SetBool("bulb", false);
        playerHeadAnimator.SetBool("flower", false);

        if (petalCounter <= 0)
        {
            playerBloom = PlayerBloom.injured;
            playerAnimator.SetBool("injured", true);
        }
        else if (petalCounter <= PETAL_BUD_THRESHOLD)
        {
            if (playerBloom != PlayerBloom.leaf)
            {
                playerBloom = PlayerBloom.leaf;

                playerHeadAnimator.SetTrigger("bloom");

                if (petalIncrease)
                {
                    playerAnimator.SetTrigger("leaf");
                    AudioManager.PlaySFX(bloomSFX, 1f, 0, transform.position);
                }
            }
        }
        else if (petalCounter < PETAL_BLOOM_THRESHOLD)
        {
            playerHeadAnimator.SetBool("bulb", true);

            if (playerBloom != PlayerBloom.bud)
            {
                playerBloom = PlayerBloom.bud;

                playerHeadAnimator.SetTrigger("bloom");

                if (petalIncrease)
                {
                    playerAnimator.SetTrigger("bud");
                    AudioManager.PlaySFX(bloomSFX, 1f, 0, transform.position);
                }
            }
        }
        else
        {
            playerHeadAnimator.SetBool("flower", true);

            if (playerBloom != PlayerBloom.flower)
            {
                playerBloom = PlayerBloom.flower;

                if (petalIncrease)
                {
                    playerAnimator.SetTrigger("flower");
                    AudioManager.PlaySFX(bloomSFX, 1f, 0, transform.position);
                    playerHeadAnimator.SetTrigger("bloom");
                }
            }
        }

        playerStats.UpdateStats(playerBloom);
    }

    public bool IncrementPetalCounter(int petalAddition)
    {
        if (petalCounter >= PETAL_CAP)
        {
            return false;
        }

        petalCounter += petalAddition;

        if (petalCounter > PETAL_CAP)
        {
            petalCounter = PETAL_CAP;
        }

        UpdateBloomStatus(true);

        playerAnimator.SetTrigger("heal");

        return true;
    }

    public bool TryDecrementPetalCounter(int petalDecrease)
    {
        if (petalCounter <= 0)
        {
            return false;
        }

        petalCounter -= petalDecrease;

        if (petalCounter < 0)
        {
            petalCounter = 0;
        }

        playerAnimator.SetTrigger("hurt");

        UpdateBloomStatus();

        return true;
    }

    public void KillPlayer()
    {
        OnPlayerDead?.Invoke(this, true);
    }

    private void PlayerProjectileHit()
    {
        //IncrementPetalCounter(1);
    }

    private void PlayerProjectileExpended(object sender, int petalsExpended)
    {
        TryDecrementPetalCounter(petalsExpended);
    }
}
