using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class AsteroidBase : Enemy_Base
{

    [Header("운석 기본데이터------------------")]
    //파괴했을 때 점수
   
    
    /// </summary>
    /// 이동 속도(초당 이동 거리)
    public float maxmoveSpeed = 4.0f;
    public float minmoveSpeed = 2.0f;
    //float moveSpeed;
    ///// <summary>
    ///// </summary>

    ///// 회전 속도(초당 회전 각도(도:degree))
    public float maxrotateSpeed = 360.0f;
    public float minrotateSpeed = 30.0f;
    //float rotateSpeed;

    //flip용 SpriteRenderer;
    SpriteRenderer asteroidRenderer;

    /// <summary>
    /// 운석의 이동 방향
    /// </summary>
    Vector3 dir = Vector3.left;
  
    public Vector3 Direction
    {
        set => dir = value;
    }

    protected virtual void Awake()
    {
        asteroidRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        score = 5;

        moveSpeed = Random.Range(minmoveSpeed, maxmoveSpeed);
        //최저일때 0 , 최고일때 1이 되는 수식 만들기
        float ratio = (moveSpeed - minmoveSpeed) / (maxmoveSpeed - minmoveSpeed);

        //moveSpeed가 minmoveSpeed이면 ratio = 0
        //moveSpeed가 maxmoveSpeed이면 ratio = 1

        //ratio 가 0이면 minRotateSpeed, 1이면, maxRotateSpeed 반환
        rotateSpeed = Mathf.Lerp(minrotateSpeed, maxrotateSpeed, ratio); //보간 Interpolate함수

        int flip = Random.Range(0, 4);
        asteroidRenderer.flipX = (flip & 0b_01) != 0;
        asteroidRenderer.flipY = (flip & 0b_10) != 0;

    }


    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * dir, Space.World);
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        //흰색으로 오브젝트 위치에서 dir의 1.5배 만큼 이동한 지점
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + dir * 1.5f);      
    }

}

