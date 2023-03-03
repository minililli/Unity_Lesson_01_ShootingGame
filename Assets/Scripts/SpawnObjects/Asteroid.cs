using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : AsteroidBase
{

    [Header("큰운석 데이터------------------")]

    //파괴될 때 오브젝트의 종류
    public PoolObjectType childType = PoolObjectType.Asteroidsmall;

    public int splitCount = 3;

    //최소 수명
    public float minLifeTime = 4.0f;
    //최대수명
    public float maxLifeTime = 7.0f;

   
    protected override void OnEnable()
    {
        base.OnEnable();
        score = 30;
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        Debug.Log($"{lifeTime}초뒤 자폭함");

        //이전 코루틴 영향 제거
        StopAllCoroutines();
        // 새 코루틴 시작
        StartCoroutine(SelfCrush(lifeTime));
    }

    //파괴될 때 생성할 오브젝트의 갯수

    protected override void OnCrush()
    {
        base.OnCrush();

        //if ()           //5%확률로
        {
            splitCount = 20;
        }
        else
        {
            splitCount = Random.Range(3, 8);
        }
       

        float angleGap = 360 / splitCount;
        float seed = Random.Range(0.0f, 360.0f);
 
        
        for(int i=0; i<splitCount; i++)
        {
            GameObject obj = Factory.Inst.GetObject(childType);
            obj.transform.position = transform.position;

            AsteroidBase small = obj.GetComponent<AsteroidBase>();

            small.TargetPlayer = TargetPlayer;

            //Up(0,1,0) 벡터를 일단 z축에서 seed만큼ㅎ 회전시키고
            //추가로 angleGap * i 만큼 회전시키고 small의 방향으로 지정
            small.Direction= Quaternion.Euler(0,0,angleGap*i+seed) * Vector3.up;
        }
    }

    IEnumerator SelfCrush(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Debug.Log("자폭함");
        isSelfCrushed = true;
        Crush();
    }



}
