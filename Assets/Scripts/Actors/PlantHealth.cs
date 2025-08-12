using UnityEngine;

public class PlantHealth : HealthSystem
{
    [SerializeField]
    private LightPlant lightPlant;

    public override void TakeDamage(int damageAmount)
    {
        lightPlant.FeedLightPlant();
    }
}
