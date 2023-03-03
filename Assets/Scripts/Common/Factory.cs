using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//enum(이넘) 타입
public enum PoolObjectType
{
    Bullet = 0,
    Hit,
    Fighter,
    Asteroid,
    Asteroidsmall,
    Explosion
    
}

public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;
    FighterPool fighterPool;
    HitEffectPool hitPool;
    ExplosionEffectPool explosionPool;
    AsteroidPool asteroidPool;
    AsteroidsmallPool asteroidsmallPool;

    protected override void PreInitialize()
    {
        bulletPool = GetComponentInChildren<BulletPool>();
        fighterPool = GetComponentInChildren<FighterPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        asteroidPool = GetComponentInChildren<AsteroidPool>();
        asteroidsmallPool = GetComponentInChildren<AsteroidsmallPool>();

    }

    protected override void Initialize()
    {
        bulletPool?.Initialize();
        fighterPool?.Initialize();
        hitPool?.Initialize();
        explosionPool?.Initialize();
        asteroidPool?.Initialize();
        asteroidsmallPool?.Initialize();
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
            case PoolObjectType.Fighter:
                result = GetFighter().gameObject;
                break;
            case PoolObjectType.Explosion:
                result = GetExplosionEffect().gameObject;
                break;
            case PoolObjectType.Asteroid:
                result = GetAsteroid().gameObject;
                break;

            case PoolObjectType.Asteroidsmall:
                result = GetAsteroidSmall().gameObject;
                break;

        }
        return result;
    }

    public Bullet GetBullet() => bulletPool?.GetObject();
    public Effect GetHitEffect() => hitPool?.GetObject();

    public Fighter GetFighter() => fighterPool?.GetObject();

    public Effect GetExplosionEffect() => explosionPool?.GetObject();
    
    //운석하나 꺼내는 함수
    public Asteroid GetAsteroid() => asteroidPool?.GetObject();

    public AsteroidBase GetAsteroidSmall() => asteroidsmallPool?.GetObject();
}
