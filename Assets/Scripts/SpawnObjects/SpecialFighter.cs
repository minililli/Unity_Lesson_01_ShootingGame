using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFighter : Enemy_Base
{
    [Header("비행기 데이터------------------")]
    public PoolObjectType explosionType;

    protected override void OnEnable()
    {
        maxHitPoint = 5;
        score = 20;
        base.OnEnable();
    }

    private void Update()
    {
        StopAllCoroutines();
        StartCoroutine(specialMove());
    }

    IEnumerator specialMove()
    {
        while (true)
        {
            moveSpeed = 0;
            transform.Translate(Time.deltaTime * moveSpeed * Vector2.left);
            yield return new WaitForSeconds(1.0f);
            moveSpeed = 6.0f;
            transform.Translate(Time.deltaTime * moveSpeed * Vector2.left);
            yield return new WaitForSeconds(2.0f);
        }
    }
}

