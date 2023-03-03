using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{

    //목적지 설정
    Transform destination;

    private void Awake()
    {
        destination = transform.GetChild(0);                          //첫번째 자식으로 만들기
    }

    protected override IEnumerator Spawn()
    {
        while (true) //무한루프
        {
            //생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Asteroid);

            Asteroid asteroid = obj.GetComponent<Asteroid>();        //생성한 게임 오브젝트에서 Asteroid
            asteroid.TargetPlayer = player;                        //Asteroid에 플레이어 설정

            asteroid.transform.position = transform.position;
            float r = Random.Range(minY, maxY);
            asteroid.transform.Translate(Vector3.up * r);

            Vector3 destPos = destination.position;             //목적지 중심지 저장
            destPos.y = Random.Range(minY, maxY);               //목적지의 y값만 랜덤으로 조정
            
            
            //방향만 남기기 위해 normalized 실행
            asteroid.Direction = (destPos - asteroid.transform.position).normalized; 


            yield return new WaitForSeconds(interval); // 인터벌만큼 대기

        }

    }


    protected override void OnDrawGizmos()
    {      
        base.OnDrawGizmos();

        //목적지 영역을 큐브로 그리기
        Gizmos.color = Color.green;

        if (destination == null)
        {
            destination = transform.GetChild(0); 
        }
        Gizmos.DrawWireCube(destination.transform.position,
                            new Vector3(1, (Mathf.Abs(maxY) + Mathf.Abs(minY)), 1));

    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        if (destination == null)                                        //destination이 자식 transform이기 때문에 플리이 전에는 없음.
        {
            destination = transform.GetChild(0);
        }
        Vector3 from = destination.position + Vector3.up * minY;
        Vector3 to = destination.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);
    }

   
}
