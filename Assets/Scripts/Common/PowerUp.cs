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



    //기본적으로 랜덤항향이 지정되지만, 60%확률로 무조건 플레이어의 반대방향으로 진행하게 만들기
    [Range(0f, 1f)]
    public float dirChange = 35.0f;
    //방향 전환 시간 간격
    public float dirChangeinterval = 5.0f;
    //bool isChanged = false;

    WaitForSeconds changeInterval;

    private void Awake()
    {
        //player = FindObjectOfType<Player>();
        changeInterval = new WaitForSeconds(dirChangeinterval);
    }

    private void OnEnable()
    {
        if (playerTransform == null) // 없을때만 찾기
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerTransform = player.transform;
            SetRandomDirection(true); // 시작할 때 랜더방향 설정하기

            StopAllCoroutines();
            StartCoroutine(DirChange());
        }

        /*if (player != null)
        {
            playerTransform = player.transform;
            float random = Random.Range(0.0f, 1.0f);

            if (random < dirChange)
            {
                Debug.Log("dirChange 실행");
                isChanged = true;
            }
            else { isChanged = false; }
            SetRandomDirection(isChanged);

        }*/
    }
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir); //방향대로 이동시키기
    }

    void SetRandomDirection(bool allRandom = false)
    {
        // 40%확률로
        if (!allRandom && Random.value < 0.4f)
        {
            //dir = Random.onUnitSphere; (3D-구)
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
    }

    /*else
    {
        Debug.Log("allRandom.else 실행");

        dir = playerTransform.position;
        dir = Random.insideUnitCircle;
        float dirangle = Random.Range(-90.0f, 90.0f);
        playerTransform.rotation = Quaternion.AngleAxis(dirangle, playerTransform.position);
        dir = dir.normalized;
    }*/

    IEnumerator DirChange()
    {
        while (true)
        {
            yield return changeInterval;
            SetRandomDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))

        {
            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
        }
    }
}

