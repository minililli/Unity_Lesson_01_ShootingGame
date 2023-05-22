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
        //transform.GetChild(0).gameObject.SetActive(true);   //GameOver 글자 비활성화 되어있던 것을 활성화
        //gameObject.SetActive(true);                         //게임오버 패널 전체 보이기
        canvasgroup.alpha = 1.0f;
        anim.SetTrigger("GameOverStart");
    }
}
