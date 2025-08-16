using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private enum PitchEnum
    {
        normal,
        twentyFive,
        fifty,
        seventyFive,
        onetwentyfive,
        onefifty,
        oneSeventyFive,
        twohundred
    }

    private bool musicFadeInProgress = false;

    private const float MIN_PITCH_VARIATION = 0.8f;
    private const float MAX_PITCH_VARIATION = 1.2f;
    private const float FADE_SPEED = 0.25f;

    private static int sfxPoolCounter = 0;
    private float fadeCounter = 0f;

    // private bool fadeIn = false;
    // private bool fadeOut = false;

    // [SerializeField]
    // private AudioSource musicAudioSource;
    private AudioSource previousAudioSource;
    private AudioSource currentAudioSource;

    [SerializeField]
    private AudioSource lowLightAudio;

    [SerializeField]
    private AudioSource mediumLightAudio;

    [SerializeField]
    private AudioSource highLightAudio;

    [SerializeField]
    private AudioSource[] localSfxPool;
    private static AudioSource[] sfxPool;

    private static Dictionary<PitchEnum, float> enumToPitch = new Dictionary<PitchEnum, float>();

    private void OnEnable()
    {
        sfxPool = localSfxPool;
        sfxPoolCounter = 0;
    }

    private void OnDisable()
    {
        PauseManager.OnMusicVolumeUpdated -= UpdateMusicVolume;
    }

    private void Start()
    {
        PauseManager.OnMusicVolumeUpdated += UpdateMusicVolume;

        if (enumToPitch.Count == 0)
        {
            enumToPitch.Add(PitchEnum.normal, 1f);
            enumToPitch.Add(PitchEnum.twentyFive, 0.25f);
            enumToPitch.Add(PitchEnum.fifty, 0.5f);
            enumToPitch.Add(PitchEnum.seventyFive, 0.75f);
            enumToPitch.Add(PitchEnum.onetwentyfive, 1.25f);
            enumToPitch.Add(PitchEnum.onefifty, 1.5f);
            enumToPitch.Add(PitchEnum.oneSeventyFive, 1.75f);
            enumToPitch.Add(PitchEnum.twohundred, 2f);
        }

        PlayHighLightMusic();
    }

    private void Update()
    {
        if (musicFadeInProgress)
        {
            FadeMusic();
        }
    }

    private void SetMusicAudioSourceVolume(float newVolume)
    {
        currentAudioSource.volume = newVolume;
    }

    private void FadeMusic()
    {
        fadeCounter += FADE_SPEED * Time.unscaledDeltaTime;
        float fadeInVolume = Mathf.Lerp(0f, PlayerOptions.GetMusicVolume(), fadeCounter);

        if (currentAudioSource)
        {
            currentAudioSource.volume = fadeInVolume;
        }

        if (previousAudioSource)
        {
            float fadeOutVolume = PlayerOptions.GetMusicVolume() - fadeInVolume;
            previousAudioSource.volume = fadeOutVolume;
        }

        if (fadeCounter >= 1f)
        {
            musicFadeInProgress = false;
        }
    }

    public void PlayNoMusic()
    {
        previousAudioSource = currentAudioSource;
        currentAudioSource = null;
        musicFadeInProgress = true;
        fadeCounter = 0f;
    }

    public void PlayLowLightMusic()
    {
        previousAudioSource = currentAudioSource;
        currentAudioSource = lowLightAudio;
        musicFadeInProgress = true;
        fadeCounter = 0f;
    }

    public void PlayMediumLightMusic()
    {
        previousAudioSource = currentAudioSource;
        currentAudioSource = mediumLightAudio;
        musicFadeInProgress = true;
        fadeCounter = 0f;
    }

    public void PlayHighLightMusic()
    {
        previousAudioSource = currentAudioSource;
        currentAudioSource = highLightAudio;
        musicFadeInProgress = true;
        fadeCounter = 0f;
    }

    private static AudioSource PlaySFXClip(
        AudioClip clip,
        Vector3 position,
        float volume,
        float pitch
    )
    {
        AudioSource source = sfxPool[sfxPoolCounter];
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();

        sfxPoolCounter++;

        if (sfxPoolCounter >= sfxPool.Length)
        {
            sfxPoolCounter = 0;
        }

        return source;
    }

    public static AudioSource PlaySFX(
        AudioClip clip,
        float volume,
        int pitchEnum,
        Vector3 originPosition,
        bool varyPitch = true
    )
    {
        if (!Application.isPlaying)
        {
            return null;
        }

        float pitchVariance = 1f;

        if (varyPitch)
        {
            pitchVariance = Random.Range(MIN_PITCH_VARIATION, MAX_PITCH_VARIATION);
        }

        return PlaySFXClip(
            clip,
            originPosition,
            volume * PlayerOptions.GetSFXVolume(),
            enumToPitch[(PitchEnum)pitchEnum] * pitchVariance
        );
    }

    private void UpdateMusicVolume(object sender, float newVolume)
    {
        SetMusicAudioSourceVolume(PlayerOptions.GetMusicVolume());
    }
}
