using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool pauseCooldown = false;
    private bool pauseActive = false;

    [SerializeField]
    private CanvasGroup pauseMenuScreen;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    public static EventHandler<bool> OnPauseGame;

    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnSFXVolumeUpdated;

    private void Awake()
    {
        //pauseMenuScreen.blocksRaycasts = false;
        Time.timeScale = 1f;

        musicSlider.value = PlayerOptions.GetMusicVolumeSettings();
        sfxSlider.value = PlayerOptions.GetSFXVolume();
    }

    private void Start()
    {
        InputManager.Instance.OnMenuEvent += TogglePause;
        LevelManager.OnGameEndLight += EndOfGamePause;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMenuEvent -= TogglePause;
        LevelManager.OnGameEndLight -= EndOfGamePause;
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerOptions.SetSFXVolume(newVolume);

        OnSFXVolumeUpdated?.Invoke(this, newVolume);
    }

    private IEnumerator PauseCD()
    {
        pauseCooldown = true;
        yield return new WaitForSecondsRealtime(0.5f);
        pauseCooldown = false;
    }

    public void TogglePause()
    {
        if (pauseCooldown)
        {
            return;
        }

        StartCoroutine(PauseCD());

        pauseActive = !pauseActive;

        pauseMenuScreen.gameObject.SetActive(pauseActive); // Temp basic enable

        pauseMenuScreen.blocksRaycasts = pauseActive;

        if (pauseActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        OnPauseGame?.Invoke(this, pauseActive);
    }

    private void EndOfGamePause(object sender, bool lightsOut)
    {
        pauseActive = true;
        Time.timeScale = 0f;
    }
}
