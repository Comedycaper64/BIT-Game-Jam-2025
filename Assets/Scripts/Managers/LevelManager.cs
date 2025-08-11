using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LightManager lightManager;

    private void Awake()
    {
        GameStartUI.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        GameStartUI.OnGameStart -= StartGame;
    }

    private void StartGame()
    {
        InputManager.Instance.GameStart();
        //Enable player
        lightManager.BeginLightCheck();
    }
}
