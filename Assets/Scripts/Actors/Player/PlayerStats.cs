using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;

    [Header("Dash")]
    [SerializeField]
    private float dashRechargeTime = 1f;

    [SerializeField]
    private float dashSpeedModifier = 3f;

    [SerializeField]
    private float dashingTime = 0.4f;

    public void UpdateStats(PlayerBloom playerBloom) { }

    public float GetMovementSpeed()
    {
        return movementSpeed;
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
