using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Pool : Test_Base
{
    public BulletPool pool1;
    public FighterPool pool2;
    public HitEffectPool pool3;
    public ExplosionEffectPool pool4;
    public AsteroidPool pool5;

    Transform[] spawnTransforms;

    private void Start()
    {
        spawnTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        { spawnTransforms[i] = transform.GetChild(i); }
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
        PoolObject obj = pool1.GetObject();
        obj.transform.position = spawnTransforms[0].position;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        PoolObject obj = pool2.GetObject(spawnTransforms[1]);
        //obj.transform.position = spawnTransforms[0].position;
    }
    protected override void Test3(InputAction.CallbackContext _)
    {
        PoolObject obj = pool3.GetObject(spawnTransforms[1]);
    }
    protected override void Test4(InputAction.CallbackContext _)
    {
        PoolObject obj = pool4.GetObject(spawnTransforms[1]);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        PoolObject obj = pool5.GetObject(spawnTransforms[1]);
    }

}
