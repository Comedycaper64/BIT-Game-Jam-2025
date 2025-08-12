using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerBloom bloomStatus;

    [SerializeField]
    private float movementSpeedInjured = 3f;

    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float movementSpeedFlower = 7f;

    [Header("Attack")]
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private int attackDamageBloom = 2;

    [SerializeField]
    private float attackSpeedInjured = 0.5f;

    [SerializeField]
    private float attackSpeedLeaf = 1f;

    [SerializeField]
    private float attackSpeedBud = 1.5f;

    [SerializeField]
    private float attackSpeedFlower = 2f;

    [SerializeField]
    private float projectileSpeed = 5f;

    [SerializeField]
    private float projectileSpeedBud = 8f;

    [Header("Dash")]
    [SerializeField]
    private float dashRechargeTime = 1f;

    [SerializeField]
    private float dashSpeedModifier = 3f;

    [SerializeField]
    private float dashingTime = 0.4f;

    public void UpdateStats(PlayerBloom playerBloom)
    {
        bloomStatus = playerBloom;
    }

    public float GetMovementSpeed()
    {
        switch (bloomStatus)
        {
            case PlayerBloom.injured:
                return movementSpeedInjured;
            case PlayerBloom.flower:
                return movementSpeedFlower;
            default:
                return movementSpeed;
        }
    }

    public int GetAttackDamage()
    {
        switch (bloomStatus)
        {
            case PlayerBloom.flower:
                return attackDamageBloom;
            default:
                return attackDamage;
        }
    }

    public float GetAttackSpeed()
    {
        switch (bloomStatus)
        {
            case PlayerBloom.leaf:
                return attackSpeedLeaf;
            case PlayerBloom.bud:
                return attackSpeedBud;
            case PlayerBloom.flower:
                return attackSpeedFlower;
            default:
                return attackSpeedInjured;
        }
    }

    public float GetProjectileSpeed()
    {
        switch (bloomStatus)
        {
            case PlayerBloom.bud:
                return projectileSpeedBud;
            case PlayerBloom.flower:
                return projectileSpeedBud;
            default:
                return projectileSpeed;
        }
    }

    public float GetDashRechargeTime()
    {
        return dashRechargeTime;
    }

    public float GetDashSpeedModifier()
    {
        return dashSpeedModifier;
    }

    public float GetDashTime()
    {
        return dashingTime;
    }
}
