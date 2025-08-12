using System;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private bool lightCheckActive = false;
    private float currentLightLevel = 10;
    private float lightLevel = 100;
    private float lightDrainSpeed = 5f;
    private float lightDrainBreakMultiplier = 2f;

    [SerializeField]
    private Gradient lightGradient;

    [SerializeField]
    private LightPlant[] lightPlants;

    public static EventHandler<Color> OnLightLevelChange;

    public static Action OnLightExtinguished;

    private void Update()
    {
        if (!lightCheckActive)
        {
            return;
        }

        ResolveLightPlants();

        EvaluateLight();
    }

    private void ResolveLightPlants()
    {
        float currentLightLevel = 0;
        float brokenDrainMultiplier = 1;

        foreach (LightPlant plant in lightPlants)
        {
            currentLightLevel += plant.GetStoredLight();

            if (plant.GetStoredLight() <= 0f)
            {
                brokenDrainMultiplier *= lightDrainBreakMultiplier;
            }
        }

        lightLevel = currentLightLevel / lightPlants.Length;

        foreach (LightPlant plant in lightPlants)
        {
            plant.DrainLight(Time.deltaTime * lightDrainSpeed * brokenDrainMultiplier);
        }
    }

    private void EvaluateLight()
    {
        float newLightLevel = lightLevel / 10f;
        newLightLevel = Mathf.CeilToInt(newLightLevel);

        if (currentLightLevel == newLightLevel)
        {
            return;
        }

        currentLightLevel = newLightLevel;

        Color evaluatedColour = lightGradient.Evaluate(newLightLevel / 10f);

        OnLightLevelChange?.Invoke(this, evaluatedColour);

        if (lightLevel <= 0f)
        {
            lightCheckActive = false;
            OnLightExtinguished?.Invoke();
        }
    }

    public void BeginLightCheck()
    {
        lightCheckActive = true;
    }
}
