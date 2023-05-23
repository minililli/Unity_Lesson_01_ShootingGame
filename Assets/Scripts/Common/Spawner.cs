using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//게임오브젝트를 주기적으로 생성할 클래스

//생성할 게임오브젝트
//위치
//시간 간격


public class Spawner : MonoBehaviour
{
    public PoolObjectType objectType; //생성할 게임오브젝트

    Boss boss;

    // 생성할 위치
    public float minY = -4.0f;
    public float maxY = 4.0f;

    //시간간격
    public float interval = 1.0f;

    Player player;
    private void OnEnable()
    {
    }
    private void Start()
    {
        StartCoroutine(Spawn()); // 시작할 때 Spawn 코루틴(오브젝트를 주기적으로 생성) 시작

        player = FindObjectOfType<Player>();
        player.onDie += (player) => StopAllCoroutines();

        boss = FindObjectOfType<Boss>();
        if (boss != null)
        {
            boss.onBossDie += () => StopAllCoroutines();
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);  // 인터벌만큼 대기

            // 생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(objectType);

            // 생성한 게임오브젝트에서 EnemyBase 컴포넌트 가져오기
            EnemyBase enemy = obj.GetComponent<EnemyBase>();
            enemy.TargetPlayer = player;                    // EnemyBase에 플레이어 설정
            enemy.transform.position = transform.position;  // 스포너 위치로 이동

            // 상속 받은 클래스별 별도 처리
            OnSpawn(enemy);
        }
    }

    protected virtual void OnSpawn(EnemyBase enemy)
    {
        float r = Random.Range(minY, maxY);             // 랜덤하게 적용할 기준 높이 구하고
        enemy.transform.Translate(Vector3.up * r);      // 랜덤하게 높이 적용하기
    }


    //씬 창에 개발용 정보를 그리는 함수
    virtual protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //스폰영역 큐브로 그리기
        Gizmos.DrawWireCube(transform.position,
                            new Vector3(1, (Mathf.Abs(maxY) + Mathf.Abs(minY)), 1));
    }

    virtual protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 스폰 지점을 선으로 긋기
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);
    }


}

