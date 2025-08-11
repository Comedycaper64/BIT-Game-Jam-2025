using System;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup gameStartGroup;

    public static Action OnGameStart;

    public void StartGame()
    {
        gameStartGroup.gameObject.SetActive(false);
        gameStartGroup.blocksRaycasts = false;
        OnGameStart?.Invoke();
    }
}
