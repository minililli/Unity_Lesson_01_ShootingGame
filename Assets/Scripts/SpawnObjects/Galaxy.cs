using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    //갤럭시 스폰조건 : 보스죽음
    //갤럭시 역할 : 스테이지 종료

    Boss boss;  
    
    [Range(0f, 1f)]
    public float speedBreaker = 0.5f;
    /// <summary>
    /// 회전 속도
    /// </summary>
    float rotateSpeed = 30f;
    /// <summary>
    /// 스테이지 클리어를 알리는 델리게이트
    /// </summary>
    public Action StageClearAlarm;   
    private void Start()
    {
        boss = FindObjectOfType<Boss>();
        boss.onBossDie += onSpawn;
        Player player = FindObjectOfType<Player>();
        this.gameObject.SetActive(false);
    }

    private void onSpawn()
    {
        this.gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * 0.5f, 0);
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StageClearAlarm?.Invoke();   
        }
    }
}

