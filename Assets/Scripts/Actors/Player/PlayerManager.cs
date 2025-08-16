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

    private const int PETAL_ATTACK_COST = 1;
    private const int PETAL_CAP = 15;
    private const int PETAL_BUD_THRESHOLD = 5;
    private const int PETAL_BLOOM_THRESHOLD = 10;

    private PlayerBloom playerBloom = PlayerBloom.leaf;
    private PlayerStats playerStats;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Animator playerHeadAnimator;

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

    private void UpdateBloomStatus()
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
            playerBloom = PlayerBloom.leaf;
        }
        else if (petalCounter < PETAL_BLOOM_THRESHOLD)
        {
            playerBloom = PlayerBloom.bud;
            playerHeadAnimator.SetBool("bulb", true);
        }
        else
        {
            playerBloom = PlayerBloom.flower;
            playerHeadAnimator.SetBool("flower", true);
        }

        playerHeadAnimator.SetTrigger("bloom");

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

        playerAnimator.SetTrigger("heal");

        UpdateBloomStatus();

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

    private void PlayerProjectileExpended()
    {
        TryDecrementPetalCounter(PETAL_ATTACK_COST);
    }
}
