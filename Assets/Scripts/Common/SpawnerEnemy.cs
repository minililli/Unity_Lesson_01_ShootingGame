using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : Spawner
{
   
    override protected IEnumerator Spawn()
    {
        while (true)
        {
            //생성하고 생성한 오브젝트를 스포너의 자식으로 만들기
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.Fighter);


            Fighter fighter = obj.GetComponent<Fighter>();        //생성한 게임 오브젝트에서 Enemy
            fighter.TargetPlayer = player;                    //Enemy에 플레이어 설정

            fighter.transform.position = transform.position;
            float r = Random.Range(minY, maxY);
            fighter.BaseY = r;

            yield return new WaitForSeconds(interval); //인터벌만큼 대기

        }

    }

    //씬 창에 개발용 정보를 그리는 함수
    override protected void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        Gizmos.color = new Color(1, 0, 0);
        Vector3 from = transform.position + Vector3.up * minY;
        Vector3 to = transform.position + Vector3.up * maxY;
        Gizmos.DrawLine(from, to);

        Gizmos.DrawWireCube(transform.position, new Vector3(1, (Mathf.Abs(maxY) + Mathf.Abs(minY)), 1));

    }

}
