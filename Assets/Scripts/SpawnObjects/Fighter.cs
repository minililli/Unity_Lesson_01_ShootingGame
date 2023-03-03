using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Enemy_Base
{
    [Header("비행기 데이터------------------")]
    public PoolObjectType explosionType;

    [Range(1.0f,3.0f)]
    public float amplitude = 1; // amplitude=진폭 => 사인 결과값을 증폭시킬 변수 선언(위아래 차이 결정)
    public float frequency = 1; // 사인그래프가 한번 도는데 걸리는 시간

    float timeElapsed = 0.0f;
    float baseY;
    public float BaseY
    {
        set
        {
            baseY = value;
            Vector3 newPos = transform.position;
            newPos.y += baseY;
            transform.position = newPos;
        }
    }
    

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();

        transform.localPosition = Vector3.zero;
        baseY = 0.0f; // 기본높이 설정

        maxHitPoint = 2;
        score = 10;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime * frequency;
        float x = transform.position.x - (moveSpeed * Time.deltaTime); // x는 현재위치에서 약간 왼쪽으로 이동
        float y = baseY + Mathf.Sin(timeElapsed) * amplitude;

        transform.position = new Vector3(x, y, 0); // 구한 x,y를 이용해 
    }
}

