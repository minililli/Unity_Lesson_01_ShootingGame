using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    Player player;
    CanvasGroup canvas;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        canvas = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        canvas.alpha = 0.0f;
    }
    private void Start()
    {
        player.onDie += (_) => canvas.alpha = 1.0f;
    }
}
