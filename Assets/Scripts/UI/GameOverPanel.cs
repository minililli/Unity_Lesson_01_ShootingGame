using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    Player player;
    Animator anim;
    CanvasGroup canvasgroup;
    Button button;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvasgroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
        canvasgroup.alpha = 0f;
        player.onDie += ShowPanel; 
    }


    private void ShowPanel(Player player)
    {
        anim.SetTrigger("GameOverStart");

    }
    private void ShowPanel()
    {
        anim.SetTrigger("GameOverStart");

    }
}
