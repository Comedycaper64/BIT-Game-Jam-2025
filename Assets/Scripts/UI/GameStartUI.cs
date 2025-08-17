using System;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup gameStartGroup;

    [SerializeField]
    private Animator gameStartAnimator;

    public static Action OnGameStart;

    public void StartGame()
    {
        //gameStartGroup.gameObject.SetActive(false);
        gameStartAnimator.SetTrigger("disappear");
        gameStartGroup.blocksRaycasts = false;
        OnGameStart?.Invoke();
    }
}
