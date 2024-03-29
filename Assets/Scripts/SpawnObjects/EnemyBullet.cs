using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class EnemyBullet : PoolObject
{
    public PoolObjectType hitType;  //발생할 히트이펙트

    public float speed = 10.0f;

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        
        StopAllCoroutines();
        StartCoroutine(LifeOver(7.0f));
    }

    void Update()
    {
        transform.position += (Time.deltaTime * speed * -transform.right); //왼쪽으로 이동시키기
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 부딪친 게임오브젝트의 태그가 "Player"이면 
        {
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Hit); //hit 이팩트 풀에서 가져오기
            obj.transform.position = collision.contacts[0].point; // hit 이팩트 충돌지점으로 이동시키기
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<Player>().Life--;
        }

    }
}
