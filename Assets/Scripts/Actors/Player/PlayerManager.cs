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
    private PlayerBloom playerBloom = PlayerBloom.leaf;
    private PlayerStats playerStats;

    public static EventHandler<bool> OnPlayerDead;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void UpdateBloomStatus()
    {
        //Update player bloom

        playerStats.UpdateStats(playerBloom);
    }

    public bool IncrementPetalCounter(int petalAddition)
    {
        petalCounter += petalAddition;

        UpdateBloomStatus();

        return true;
    }

    public bool TryDecrementPetalCounter(int petalDecrease)
    {
        petalCounter -= petalDecrease;

        UpdateBloomStatus();

        return true;
    }
}
