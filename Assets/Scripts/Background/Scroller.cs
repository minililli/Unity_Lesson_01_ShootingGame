using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    // 자식으로 붙어있는 BG_Slot이 계속 왼쪽으로 이동
    // 하나의 BG_Slot이 충분히 화면밖으로 나갔을 경우, 왼쪽으로 이동하면 BG_Slot의 가장 오른쪽으로 이동시킴

    //이동속도
    //끝나는점위치
    //BG_Slot transform[]

    //...

    //이동속도
    public float scrollingSpeed = 2.5f;
    //배경이미지가 두개 붙어있는 한 덩어리
    Transform[] bgSlots = null;

    float slot_Width = 13.6f;

    protected virtual void Awake()
    {
        bgSlots = new Transform[transform.childCount]; //슬롯이 들어갈 배열
        for(int i=0; i<transform.childCount;i++)
        {
            bgSlots[i] = transform.GetChild(i);         //슬롯 하나씩 찾기
        }
        slot_Width = bgSlots[1].position.x - bgSlots[0].position.x;   //한 슬롯의 길이찾기
    }

    private void Update()
    {
        //foreach 가 for 보다 빠름
        for (int i = 0; i<bgSlots.Length;i++)
        {
            Transform slot = bgSlots[i];
            slot.Translate(Time.deltaTime * scrollingSpeed * -transform.right);
            if(slot.localPosition.x < 0)
            {
                MoveRightEnd(i);
            }
        }    
        //모든 슬롯을 돌면서 찾기
        ////foreach(Transform slot in bgSlots)
        ////{
        ////    slot.Translate(Time.deltaTime * scrollerSpeed * -transform.right);
           
        ////    if(slot.localPosition.x < -slot_Width) // 슬롯이 부모 위치보다 왼쪽으로 갔을 때
        ////    {
        ////        slot.Translate(slot_Width * bgSlots.Length * transform.right);
        ////    }
        ////}

       
    }

    protected virtual void MoveRightEnd(int index)
    {
        bgSlots[index].Translate(slot_Width * bgSlots.Length * transform.right);
    }
}
