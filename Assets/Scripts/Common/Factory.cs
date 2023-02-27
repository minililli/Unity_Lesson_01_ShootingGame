using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//enum(이넘) 타입
public enum PoolObjectType
{
    Bullet = 0,
    Hit,
    Enemy,
    Explosion
}

public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;
    EnemyPool enemyPool;
    HitEffectPool hitPool;
    ExplosionEffectPool explosionPool;

    protected override void PreInitialize()
    {
        bulletPool = GetComponentInChildren<BulletPool>();
        enemyPool = GetComponentInChildren<EnemyPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();

    }

    protected override void Initialize()
    {
        bulletPool?.Initialize();
        enemyPool?.Initialize();
        hitPool?.Initialize();
        explosionPool?.Initialize();
    }



    public GameObject GetObject(PoolObjectType type)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.Bullet:
                result = GetBullet().gameObject;
                break;
            case PoolObjectType.Hit:
                result = GetHitEffect().gameObject;
                break;
            case PoolObjectType.Enemy:
                result = GetEnemy().gameObject;
                break;
            case PoolObjectType.Explosion:
                result = GetExplosionEffect().gameObject;
                break;
        }
        return result;
    }

    public Bullet GetBullet() => bulletPool?.GetObject();
    public Effect GetHitEffect() => hitPool?.GetObject();

    public Enemy GetEnemy() => enemyPool?.GetObject();

    public Effect GetExplosionEffect() => explosionPool?.GetObject();
}
