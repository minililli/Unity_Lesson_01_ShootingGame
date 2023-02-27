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
    public GameObject spawnPrefab; //생성할 게임오브젝트
    Vector3 Where;  

    // 생성할 위치
    public float minY = -4.0f;
    public float maxY = 4.0f;

    //시간간격
    public float interval = 1.0f;
    WaitForSeconds Wait;

    Player player = null;

    private void Start()
    {
        
        player = FindObjectOfType<Player>();

        Wait = new WaitForSeconds(interval); // yield return new WaitForSeconds(interval)보다는 게임 실행 도중 interval 변하지 않는다면 미리 한번 만들어두는 것이 메모리상 유리
        
        StartCoroutine(Spawn()); // 시작할 때 Spawn 코루틴(오브젝트를 주기적으로 생성) 시작
    
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            //생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Enemy);
            
            
            Enemy enemy = obj.GetComponent<Enemy>();        //생성한 게임 오브젝트에서 Enemy
            enemy.TargetPlayer = player;                    //Enemy에 플레이어 설정

            enemy.transform.position = transform.position;
            float r = Random.Range(minY, maxY);
            enemy.BaseY = r;
           
            yield return Wait; // 인터벌만큼 대기
           
        }
    
    }

    //씬 창에 개발용 정보를 그리는 함수
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        Gizmos.color = new Color(1, 0, 0);
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);

        Gizmos.DrawWireCube(transform.position, new Vector3(1, (Mathf.Abs(maxY)+Mathf.Abs(minY)), 1));

        OnPointGizmo();
    }

    private void OnPointGizmo()
    {
        Gizmos.color = new Color(0, 0, 1f);
        {
            Gizmos.DrawWireSphere(Where, 0.5f);
        }
    }
}

