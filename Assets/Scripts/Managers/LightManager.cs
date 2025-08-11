using System;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private bool lightCheckActive = false;
    private float currentLightLevel = 10;
    private float lightLevel = 100;

    [SerializeField]
    private Gradient lightGradient;

    public static EventHandler<Color> OnLightLevelChange;

    public static Action OnLightExtinguished;

    // private void Start()
    // {
    //     //temp
    //     lightCheckActive = true;
    // }

    private void Update()
    {
        if (!lightCheckActive)
        {
            return;
        }

        lightLevel -= Time.deltaTime * 5f;

        EvaluateLight();
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
