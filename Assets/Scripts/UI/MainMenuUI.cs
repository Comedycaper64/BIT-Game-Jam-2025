using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void LoadMainGame()
    {
        SceneManager.LoadScene(1);
    }
}
