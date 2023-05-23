using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    Player player;
    Animator anim;
    CanvasGroup canvasgroup;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        canvasgroup = GetComponent<CanvasGroup>();
    }


    private void Start()
    {
        
        player = FindObjectOfType<Player>();
        canvasgroup.alpha = 0f;
        player.onDie += ShowPanel;                          //플레이어의 onDie 델리게이트에 함수 등
    }
    
    private void ShowPanel(Player player)
    {
        canvasgroup.alpha = 1.0f;
        anim.SetTrigger("GameOverStart");
    }
}
