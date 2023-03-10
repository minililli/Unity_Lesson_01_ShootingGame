using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFighter : EnemyBase
{
    [Header("특수적 데이터------------------")]
    public float startSpeed = 10.0f;
    public float appearTime = 0.5f;
    public float waitTime = 5.0f;
    

    protected override void OnEnable()
    {
        score = 30;
        maxHitPoint = 5;
        base.OnEnable();
        StopAllCoroutines();
        StartCoroutine(SpawnProduce());

    }


    IEnumerator SpawnProduce()
    {
        moveSpeed = startSpeed;                         // 처음에는 속도를 빠르게 설정
        yield return new WaitForSeconds(appearTime);    // 등장에 걸리는 시간이 다될 때까지 빠르게 등장
        moveSpeed = 0.0f;                               // 정지시키기
        yield return new WaitForSeconds(waitTime);      // 대기 시간만큼 정지
        moveSpeed = startSpeed;                         // 대기 시간이 끝나면 원래 속도로 돌아가기
    }

    private void Update()
    {
        // 초당 moveSpeed의 속도로 왼쪽으로 이동
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.left);
    }

    protected override void OnCrush()
    {
        base.OnCrush(); // 기존 폭파 처리

        GameObject obj = Factory.Inst.GetObject(PoolObjectType.PowerUp);    // 파워업 아이템 생성
        obj.transform.position = transform.position;        // 현재 내 위치로 옮기기
    }
}

