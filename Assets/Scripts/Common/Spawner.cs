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

    // 생성할 위치
    public float minY = -4.0f;
    public float maxY = 4.0f;

    //시간간격
    public float interval = 1.0f;
    WaitForSeconds Wait;

    protected Player player = null;
    /// 플레이어에 대한 참조
    /// </summary>
   
    private void Start()
    {
        
        player = FindObjectOfType<Player>();

        //Wait = new WaitForSeconds(interval); // yield return new WaitForSeconds(interval)보다는 게임 실행 도중 interval 변하지 않는다면 미리 한번 만들어두는 것이 메모리상 유리
        
        StartCoroutine(Spawn()); // 시작할 때 Spawn 코루틴(오브젝트를 주기적으로 생성) 시작
    
    }

    virtual protected IEnumerator Spawn()
    {
        yield return null;
    }

    //씬 창에 개발용 정보를 그리는 함수
   virtual protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //스폰영역 큐브로 그리기
        Gizmos.DrawWireCube(transform.position,
                            new Vector3(1, (Mathf.Abs(maxY)+Mathf.Abs(minY)), 1));
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

