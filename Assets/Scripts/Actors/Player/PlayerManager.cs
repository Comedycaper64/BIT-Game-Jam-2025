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
    private int petalStartAmount = 5;

    private const int PETAL_CAP = 25;
    private const int PETAL_BUD_THRESHOLD = 8;
    private const int PETAL_BLOOM_THRESHOLD = 16;

    private PlayerBloom playerBloom = PlayerBloom.leaf;
    private PlayerStats playerStats;

    public static EventHandler<bool> OnPlayerDead;
    public static EventHandler<int> OnNewPetalCount;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        petalCounter = petalStartAmount;
        UpdateBloomStatus();
    }

    private void OnEnable()
    {
        Projectile.OnPlayerProjectileHit += PlayerProjectileHit;
    }

    private void OnDisable()
    {
        Projectile.OnPlayerProjectileHit -= PlayerProjectileHit;
    }

    private void UpdateBloomStatus()
    {
        OnNewPetalCount?.Invoke(this, petalCounter);

        if (petalCounter <= 0)
        {
            playerBloom = PlayerBloom.injured;
        }
        else if (petalCounter <= PETAL_BUD_THRESHOLD)
        {
            playerBloom = PlayerBloom.leaf;
        }
        else if (petalCounter < PETAL_BLOOM_THRESHOLD)
        {
            playerBloom = PlayerBloom.bud;
        }
        else
        {
            playerBloom = PlayerBloom.flower;
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

        UpdateBloomStatus();

        return true;
    }

    public void KillPlayer()
    {
        OnPlayerDead?.Invoke(this, true);
    }

    private void PlayerProjectileHit()
    {
        IncrementPetalCounter(1);
    }
}
