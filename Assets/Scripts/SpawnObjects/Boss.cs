using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using System.Threading;

public class Boss : EnemyBase
{
    [Header("Boss관련 정보")]
    public GameObject BulletPrefab;
    public int bossLife = 20;
    public float midInterval = 1.0f;
    public float sideInterval = 5f;

    bool onCrush = false;
    Transform[] sidefirePos;
    TimeManager timer;
    CapsuleCollider2D[] colliders;

    protected override void Awake()
    {
        timer = FindObjectOfType<TimeManager>();
        colliders = GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        base.Awake();
    }
    protected override void OnEnable()
    {
        maxHitPoint = bossLife;
        base.OnEnable();
        Transform fireRoot = transform.GetChild(0);         //발사 위치 찾기
        sidefirePos = new Transform[fireRoot.childCount];
        for (int i = 0; i < fireRoot.childCount; i++)
        {
            sidefirePos[i] = fireRoot.GetChild(i);
        }

    }
    void Start()
    {
        timer.Bosstime += () => StartCoroutine(Appear());

        onCrush = false;
    }



    private void Update()
    {
        if (TargetPlayer.transform.position.y < transform.position.y)
        {

            transform.Translate(Time.deltaTime * -moveSpeed, 0, 0);
            if (Mathf.Abs(TargetPlayer.transform.position.y - transform.position.y) < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, TargetPlayer.transform.position.y, 0);
            }

        }
        else if (TargetPlayer.transform.position.y > transform.position.y)
        {
            transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
            if (Mathf.Abs(TargetPlayer.transform.position.y - transform.position.y) < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, TargetPlayer.transform.position.y, 0);
            }
        }

    }

    public Action onBossDie;
    protected override void OnCrush()
    {
        onCrush = true;
        onBossDie?.Invoke();
        base.OnCrush();
    }
    IEnumerator Fire(Transform[] transforms, float Interval = 0.5f)
    {
        while (!onCrush)
        {
            yield return new WaitForSeconds(Interval);
            for (int i = 0; i < transforms.Length; i++)
            {
                GameObject obj = Factory.Inst.GetObject(PoolObjectType.EnemyBullet);
                obj.transform.position = transforms[i].position;              //Bullet 위치 = 가져온 발사위치;
            }
        }
    }
    IEnumerator Fire(float Interval = 0.5f)
    {
        while (!onCrush)
        {
            yield return new WaitForSeconds(Interval);
            GameObject obj = Factory.Inst.GetObject(PoolObjectType.EnemyBullet);
            obj.transform.position = transform.GetChild(1).position;
        }
    }

    IEnumerator Appear()
    {
        while (transform.position.x > 7.0f)
        {
            transform.position += Time.deltaTime * moveSpeed * transform.up;
            yield return null;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
        //StopCoroutine(Appear());

        StartCoroutine(Fire(midInterval)); //중간 위치 발사
        StartCoroutine(Fire(sidefirePos, sideInterval)); //Side 발사
        onCrush = false;

    }
}
