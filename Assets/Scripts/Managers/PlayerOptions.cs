using UnityEngine;

public class PlayerOptions : MonoBehaviour
{
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string HIGH_SCORE = "Score";
    private const float MUSIC_LIMITER = 0.1f;

    private static float MUSIC_VOLUME_DEF = 0.5f;
    private static float SFX_VOLUME_DEF = 0.75f;
    private static float HIGH_SCORE_DEF = 0f;

    public static void SetMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetHighScore(float newScore)
    {
        PlayerPrefs.SetFloat(HIGH_SCORE, newScore);
        PlayerPrefs.Save();
    }

    public static float GetHighScore()
    {
        if (!PlayerPrefs.HasKey(HIGH_SCORE))
        {
            return HIGH_SCORE_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(HIGH_SCORE);
        }
    }

    public static float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF * MUSIC_LIMITER;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME) * MUSIC_LIMITER;
        }
    }

    public static float GetMusicVolumeSettings()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
    }

    public static float GetSFXVolume()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return SFX_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }
}
