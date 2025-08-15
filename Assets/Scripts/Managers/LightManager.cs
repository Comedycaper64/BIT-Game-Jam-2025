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
    private float lightDrainSpeed = 2f;
    private float lightDrainBreakMultiplier = 2f;
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
        LevelManager.OnGameEnd += EndLightCheck;
    }

    private void OnDisable()
    {
        LevelManager.OnGameEnd -= EndLightCheck;
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

    public void BeginLightCheck()
    {
        lightCheckActive = true;
    }

    private void EndLightCheck()
    {
        lightCheckActive = false;
    }
}
