using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using System.Threading;

public class Boss : MonoBehaviour
{
    [Header("Boss관련 정보")]
    public GameObject BulletPrefab;
    public int maxBossLife = 20;
    int bossLife;
    int Life
    {
        get => bossLife;
        set
        {
            bossLife = value;
            Mathf.Clamp(bossLife, 0, maxBossLife);
            if (bossLife < 1)
            {
                OnCrush();
            }
        }
    }
    public float midInterval = 2.0f;
    public float sideInterval = 5.0f;
    public float moveSpeed = 2.0f;
    bool onStart = false;
    bool onCrush = false;
    Transform[] sidefirePos;
    CapsuleCollider2D[] colliders;
    Player player;
    TimeManager timer;
    public Action onBossDie;

    protected void Awake()
    {
        colliders = GetComponents<CapsuleCollider2D>();
        //콜라이더 꺼놓기
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        timer = FindObjectOfType<TimeManager>();
        player = FindObjectOfType<Player>();
    }
    protected void OnEnable()
    {
        onCrush = false;
        Life = maxBossLife;
        //side발사 위치 찾기
        Transform fireRoot = transform.GetChild(0);
        sidefirePos = new Transform[fireRoot.childCount];
        for (int i = 0; i < fireRoot.childCount; i++)
        {
            sidefirePos[i] = fireRoot.GetChild(i);
        }

        StartCoroutine(MidFire()); //중간 위치 발사
        StartCoroutine(SideFire()); //Side 발사
    }
    protected void OnDisable()
    {
        StopAllCoroutines();
        onStart = false;
    }

    protected void Start()
    {
        onStart = false;
        onCrush = false;
        timer.BossTime += () =>
        {
            StartCoroutine(Appear());
        };


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            Life--;
        }
    }

    private void Update()
    {

        if (player.transform.position.y < transform.position.y)
        {

            transform.Translate(Time.deltaTime * -moveSpeed, 0, 0);
            if (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y, 0);
            }

        }
        else if (player.transform.position.y > transform.position.y)
        {
            transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
            if (Mathf.Abs(player.transform.position.y - transform.position.y) < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y, 0);
            }
        }

        if (player == null)
        {
            transform.position = Vector3.zero;
        }
    }
    protected void OnCrush()
    {
        onCrush = true;
        this.gameObject.SetActive(false);
        onBossDie?.Invoke();
    }

    IEnumerator Appear()
    {
        while (transform.position.x > 7.0f)
        {
            transform.position += Time.deltaTime * moveSpeed * transform.up;
            yield return new WaitForSeconds(1f);
        }
        onStart = true;
        OnAppear();
        StopCoroutine(Appear());
    }
    void OnAppear()
    {
        //collider 켜기
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
    }

    IEnumerator SideFire()
    {
        while (!onCrush)
        {
            yield return new WaitForSeconds(sideInterval);
            //Bullet 위치 = 가져온 발사위치;
            if (onStart)
            {
                for (int i = 0; i < sidefirePos.Length; i++)
                {
                    GameObject obj = Factory.Inst.GetObject(PoolObjectType.EnemyBullet);
                    obj.transform.position = sidefirePos[i].position;
                }
            }
        }
    }
    IEnumerator MidFire()
    {
        while (!onCrush)
        {
            yield return new WaitForSeconds(midInterval);
            if (onStart)
            {
                GameObject obj = Factory.Inst.GetObject(PoolObjectType.EnemyBullet);
                obj.transform.position = transform.GetChild(1).position;
            }
        }

    }

}
