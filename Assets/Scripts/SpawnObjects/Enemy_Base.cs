using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy_Base : PoolObject
{
    [Header("적 기본 데이터--------------------")]

    //적의HP
    int hitPoint = 1;
    public int maxHitPoint = 1;

    bool isCrushed = false;

    //public float maxmoveSpeed = 4.0f;
    //public float minmoveSpeed = 2.0f;
    public float moveSpeed = 1.5f;

    //public float maxrotateSpeed = 360.0f;
    //public float minrotateSpeed = 30.0f;
    public float rotateSpeed;

    public int score;

    virtual protected void OnEnable()
    {
        //정상이 된 것 표시하기
        isCrushed = false;
        //몇대 맞아야 터질지 세팅
        hitPoint = maxHitPoint;
    }
   
    protected bool isSelfCrushed = false;


    public PoolObjectType destroyEffect = PoolObjectType.Explosion;

    /// 플레이어에 대한 참조
    /// </summary>
    /// 
    Player player = null;


    /// <summary>
    /// player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기 전용. 자신과 상속받은 클래스에서 읽기도 가능.
    /// </summary>
    public Player TargetPlayer
    {
        protected get => player;
        set
        {
            if (player == null)     // player가 null일때만 설정
            {
                player = value;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Attacked();
        }
    }

    protected virtual void OnHit()
    {

    }

    protected void Attacked()
    {
       
        OnHit();
        hitPoint--;     //맞으면 hitpoint 감소
       if(hitPoint < 1)
        {
            Crush();
        }

    }

    //부서지면 무조건 실행해야 할 일들 처리
    protected void Crush()
    {
        if (isSelfCrushed)
        { score = 0; }

            if (!isCrushed)
        {
            isCrushed = true;

            GameObject obj = Factory.Inst.GetObject(destroyEffect);
            obj.transform.position = transform.position;

            gameObject.SetActive(false);
        }

        OnCrush();

    }

    //부서질때 상속받는 
    protected virtual void OnCrush()
    {
         player?.AddScore(score); 
    }

}
