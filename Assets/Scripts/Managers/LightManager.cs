using System;
using System.Collections;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private enum LightCategory
    {
        low,
        medium,
        high
    }

    private bool lightCheckActive = false;
    private bool musicChangeCooldown = false;
    private float currentLightLevel = 10;
    private float lightLevel = 100;
    private float lightDrainSpeed = 1f;
    private float lightDrainSpeedFaster = 1.5f;
    private float lightDrainBreakMultiplier = 1.6f;
    private const float MEDIUM_LIGHT_LEVEL = 3f;
    private const float HIGH_LIGHT_LEVEL = 6f;
    private const float MUSIC_CHANGE_CD = 2f;
    private LightCategory currentLightCategory;
    private Coroutine musicChangeCoroutine;

    [SerializeField]
    private Gradient lightGradient;

    [SerializeField]
    private LightPlant[] lightPlants;

    [SerializeField]
    private AudioManager audioManager;

    public static EventHandler<Color> OnLightLevelChange;

    public static Action OnLightExtinguished;

    private void Start()
    {
        currentLightCategory = LightCategory.high;
    }

    private void OnEnable()
    {
        LevelManager.OnGameEndLight += EndLightCheck;
    }

    private void OnDisable()
    {
        LevelManager.OnGameEndLight -= EndLightCheck;
        if (musicChangeCoroutine != null)
        {
            StopCoroutine(musicChangeCoroutine);
        }
    }

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
        int livingPlants = 0;

        foreach (LightPlant plant in lightPlants)
        {
            currentLightLevel += plant.GetStoredLight();

            if (plant.GetStoredLight() <= 0f)
            {
                brokenDrainMultiplier *= lightDrainBreakMultiplier;
            }
            else
            {
                livingPlants++;
            }
        }

        lightLevel = currentLightLevel / lightPlants.Length;

        foreach (LightPlant plant in lightPlants)
        {
            plant.DrainLight(Time.deltaTime * lightDrainSpeed * brokenDrainMultiplier);

            if (livingPlants <= 1)
            {
                plant.RemoveDrainStop();
            }
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

        CheckMusicChange();

        Color evaluatedColour = lightGradient.Evaluate(newLightLevel / 10f);

        OnLightLevelChange?.Invoke(this, evaluatedColour);

        if (lightLevel <= 0f)
        {
            lightCheckActive = false;
            OnLightExtinguished?.Invoke();
        }
    }

    private void CheckMusicChange()
    {
        if (musicChangeCooldown)
        {
            return;
        }

        if ((currentLightLevel > HIGH_LIGHT_LEVEL) && (currentLightCategory != LightCategory.high))
        {
            audioManager.PlayHighLightMusic();
            currentLightCategory = LightCategory.high;

            musicChangeCoroutine = StartCoroutine(MusicChangeCD());
        }
        else if (
            (currentLightLevel <= HIGH_LIGHT_LEVEL)
            && (currentLightLevel > MEDIUM_LIGHT_LEVEL)
            && (currentLightCategory != LightCategory.medium)
        )
        {
            audioManager.PlayMediumLightMusic();
            currentLightCategory = LightCategory.medium;
            musicChangeCoroutine = StartCoroutine(MusicChangeCD());
        }
        else if (
            (currentLightLevel <= MEDIUM_LIGHT_LEVEL) && (currentLightCategory != LightCategory.low)
        )
        {
            audioManager.PlayLowLightMusic();
            currentLightCategory = LightCategory.low;
            musicChangeCoroutine = StartCoroutine(MusicChangeCD());
        }
    }

    private IEnumerator MusicChangeCD()
    {
        musicChangeCooldown = true;
        yield return new WaitForSeconds(MUSIC_CHANGE_CD);
        musicChangeCooldown = false;
    }

    public void IncreaseLightDrainSpeed()
    {
        lightDrainSpeed = lightDrainSpeedFaster;
    }

    public void BeginLightCheck()
    {
        lightCheckActive = true;
        OnLightLevelChange?.Invoke(this, lightGradient.Evaluate(1f));
    }

    private void EndLightCheck(object sender, bool lightsOut)
    {
        lightCheckActive = false;
        audioManager.PlayNoMusic();
    }
}
