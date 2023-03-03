using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;


public class StarsScroller : Scroller
{
    //bg-stars도 배경에서 움직이게 만들기
    //1.1 무한으로 계속 나와야함
    //1.3 BGSlot보다 빨리움직여야함
    //1.3 오른쪽 끝으로 움직일때 랜덤하게 flip 되기

    //public float scrollingSpeed = 4.0f;
    //Transform[] starTransform;
    //float Stars_Width = 20.0f;
    SpriteRenderer[] spriteRenderer;


    protected override void Awake()
    {

    //    //starTransform = new Transform[transform.childCount];
    //    //for (int i = 0; i < transform.childCount; i++)
    //    //{
    //    //    starTransform[i] = transform.GetChild(i);
    //    //}
        base.Awake();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void MoveRightEnd(int index)
    {
        base.MoveRightEnd(index);

        int rand = UnityEngine.Random.Range(0, 4); ; //0(0b_00), 1(0b_01), 2(ob_10), 3(0b_11) 중 하나
        
        spriteRenderer[index].flipX = (rand & 0b_10) != 0;
        spriteRenderer[index].flipY = (rand & 0b_10) != 0;

    }
}