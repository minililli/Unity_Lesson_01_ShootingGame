using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PoolObject
{

    //이동속도
    public float moveSpeed = 2.0f;
    //이동방향
    Vector2 dir;

    //플레이어의 트랜스폼
    Transform playerTransform;
    Animator anim;



    //기본적으로 랜덤항향이 지정되지만, 60%확률로 무조건 플레이어의 반대방향으로 진행하게 만들기
    [Range(0f, 1f)]
    public float dirChange = 0.4f;
    //방향 전환 시간 간격
    public float dirChangeinterval = 5.0f;
    //게임 상 활성화되어있는 시간
    public float timeOut = 10.0f;
    // 방향 변경 전 대기시간(DirChange())
    WaitForSeconds changeInterval;

    // 최대 튕길 횟수
    const int dirChangeCountMax = 5;
    // 튕긴 횟수 = 최대 튕길 횟수 (초기화)
    int dirChangeCount = dirChangeCountMax;

    int DirChangeCount
    {
        get => dirChangeCount;
        set
        {
            dirChangeCount = value;
            //anim.SetInteger("Count", dirChangeCount);
            if (dirChangeCount <= 0)
            {
                StopAllCoroutines();
            }

        }
    }

    private void Awake()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        Animation anim = GetComponent<Animation>();
        changeInterval = new WaitForSeconds(dirChangeinterval);
    }

    private void OnEnable()
    {
        
        SetRandomDirection(true); // 시작할 때 랜덤방향 설정하기
        dirChangeCount = dirChangeCountMax; //튕기는 횟수 초기화

        StopAllCoroutines();            //이전 코루틴들 모두 제거
        StartCoroutine(DirChange());    //방향 전환설정
        StartCoroutine(TimeOut());      //활성화시간 설정
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir); //방향대로 이동시키기
    }

    void SetRandomDirection(bool allRandom = false)
    {
        // 40%확률
        if (playerTransform != null && !allRandom && Random.value < 0.4f)
        {

            Vector2 playerToPowerup = transform.position - playerTransform.position;
            // 플레이어에서 파워업으로 가는 방향 벡터를 z축 기준으로 +-90도를 랜덤으로 회전
            dir = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerup;

            Debug.Log("도망");
        }
        else //완전 랜덤이거나, 40%확률에 당첨되지 않았을 때
        { dir = Random.insideUnitCircle; } //반지름 1인 원 안의 랜덤한 위치 가져오기
        //Debug.Log("랜덤");

        //(원) // 원 안의 랜덤한 위치
        dir = dir.normalized;
        DirChangeCount--;   //경로 재 설정하면 DirChangeCount 감소
    }

    IEnumerator DirChange()
    {
        while (true)
        {
            yield return changeInterval;
            SetRandomDirection();
        }
    }
    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(timeOut);   // timeOut 후 DirChangeCount=0
        DirChangeCount = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DirChangeCount > 0 && collision.gameObject.CompareTag("Border"))
        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);   //dir = dir와 부딪친지점에서의 normal벡터

            DirChangeCount--;   //충돌하면 DirChangeCount 감소

        }

    }
}


