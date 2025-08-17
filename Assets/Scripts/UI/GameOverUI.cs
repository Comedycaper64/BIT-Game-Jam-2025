using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Color lightColour;

    [SerializeField]
    private Image[] litRenderers;

    [SerializeField]
    private TextMeshProUGUI[] litText;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private TextMeshProUGUI levelTimeText;

    [SerializeField]
    private GameObject highScoreText;

    [SerializeField]
    private Animator uiAnim;

    [SerializeField]
    private CanvasGroup gameOverGroup;

    private void Start()
    {
        LevelManager.OnGameEndLight += ToggleGameOverUI;
        LevelManager.OnSetGameTime += SetGameTime;
        LightManager.OnLightLevelChange += SetLightColour;
    }

    private void OnDisable()
    {
        LevelManager.OnGameEndLight -= ToggleGameOverUI;
        LevelManager.OnSetGameTime -= SetGameTime;
        LightManager.OnLightLevelChange -= SetLightColour;
    }

    private void SetLightColour(object sender, Color newColour)
    {
        lightColour = newColour;
    }

    private void SetGameTime(object sender, float gameTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(gameTime);
        levelTimeText.text = "TIME SURVIVED: " + time.ToString(@"mm\:ss");

        if (gameTime > PlayerOptions.GetHighScore())
        {
            highScoreText.SetActive(true);
            PlayerOptions.SetHighScore(gameTime);
        }
    }

    private void ToggleGameOverUI(object sender, bool lightsOut)
    {
        if (!lightsOut)
        {
            foreach (Image renderer in litRenderers)
            {
                renderer.color = lightColour;
            }

            foreach (TextMeshProUGUI text in litText)
            {
                text.color = lightColour;
            }
            gameOverText.text = "YOU WERE SLAIN";
            uiAnim.SetTrigger("dead");
        }
        else
        {
            uiAnim.SetTrigger("appear");
        }

        gameOverGroup.blocksRaycasts = true;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
