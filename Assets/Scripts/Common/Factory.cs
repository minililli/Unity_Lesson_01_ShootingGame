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
    SpecialFighter,
    Asteroid,
    Asteroidsmall,
    Explosion,
    PowerUp,
    EnemyBullet,
    
}

public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;
    FighterPool fighterPool;
    SpecialFighterPool specialfighterPool;
    HitEffectPool hitPool;
    ExplosionEffectPool explosionPool;
    AsteroidPool asteroidPool;
    AsteroidsmallPool asteroidsmallPool;
    PowerUpPool powerUpPool;
    EnemyBulletPool enemybulletPool;

    protected override void PreInitialize()
    {
        bulletPool = GetComponentInChildren<BulletPool>();
        fighterPool = GetComponentInChildren<FighterPool>();
        specialfighterPool = GetComponentInChildren<SpecialFighterPool>();
        hitPool = GetComponentInChildren<HitEffectPool>();
        explosionPool = GetComponentInChildren<ExplosionEffectPool>();
        asteroidPool = GetComponentInChildren<AsteroidPool>();
        asteroidsmallPool = GetComponentInChildren<AsteroidsmallPool>();
        powerUpPool = GetComponentInChildren<PowerUpPool>();
        enemybulletPool = GetComponentInChildren<EnemyBulletPool>();

    }

    protected override void Initialize()
    {
        bulletPool?.Initialize();
        fighterPool?.Initialize();
        specialfighterPool?.Initialize();
        hitPool?.Initialize();
        explosionPool?.Initialize();
        asteroidPool?.Initialize();
        asteroidsmallPool?.Initialize();
        powerUpPool?.Initialize();
        enemybulletPool?.Initialize();
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
            case PoolObjectType.SpecialFighter:
                result = GetSpecialFighter().gameObject;
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
            case PoolObjectType.PowerUp:
                result = GetPowerUp().gameObject;
                break;
            case PoolObjectType.EnemyBullet:
                result = GetEnemyBullet().gameObject;
                break;
        }
        return result;
    }

    public Bullet GetBullet() => bulletPool?.GetObject();
    public Effect GetHitEffect() => hitPool?.GetObject();

    public Fighter GetFighter() => fighterPool?.GetObject();
    public SpecialFighter GetSpecialFighter() => specialfighterPool?.GetObject();
    public Effect GetExplosionEffect() => explosionPool?.GetObject();
    
    public Asteroid GetAsteroid() => asteroidPool?.GetObject();

    public AsteroidBase GetAsteroidSmall() => asteroidsmallPool?.GetObject();

    public PowerUp GetPowerUp() => powerUpPool?.GetObject();

    public EnemyBullet GetEnemyBullet() => enemybulletPool?.GetObject();
}
