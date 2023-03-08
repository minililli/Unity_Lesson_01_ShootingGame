using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFighter : Enemy_Base            //powerup 드랍
{
    public PoolObjectType explosionType;

   

    float timeElapsed = 0.0f;
    float baseY;
    public float BaseY
    {
        set
        {
            baseY = value;
            Vector3 newPos = transform.position;
            newPos.y += baseY;
            transform.position = newPos;
        }
    }


    private void OnEnable()
    {
        base.OnEnable(); //기존 초기화 작업 진행

        //처음에 빠르게 움직이게 하고, 기다리다가 다시 빠르게 움직인다
        StartCoroutine(waitMove());
    }
    IEnumerator waitMove()
    {
        moveSpeed = Random.Range(10.0f, 30.0f) ;
       

        yield return new WaitForSeconds(3);
    }
    protected override void OnCrush()
    {

        base.OnCrush(); ///기존 폭파처리

        GameObject obj=Factory.Inst.GetObject(PoolObjectType.PowerUp); //파워업아이템 생성
        obj.transform.position = transform.position;                    //현재 내 위치로 옮기기

    }


}

