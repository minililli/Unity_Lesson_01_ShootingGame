using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : ObjectPool<Asteroid>
{
    public PoolObjectType asteroidType;
}