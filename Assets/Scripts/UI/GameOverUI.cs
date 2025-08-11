using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup gameOverGroup;

    private void Start()
    {
        LightManager.OnLightExtinguished += ToggleGameOverUI;
    }

    private void OnDisable()
    {
        LightManager.OnLightExtinguished -= ToggleGameOverUI;
    }

    private void ToggleGameOverUI()
    {
        gameOverGroup.gameObject.SetActive(true);
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
