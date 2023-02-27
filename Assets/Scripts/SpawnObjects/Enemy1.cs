using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    float Horizontalspeed = 1.0f;
    float Verticalspeed = 1.0f;

    float baseY;
    float dir = 1.0f;
    public float heightLimit = 3.0f;

    // Start is called before the first frame update
    void EnemyMove() 
    {
        //transform.Translate(Time.deltaTime * Horizontalspeed * Vector2.left);
        transform.position += (Vector3)(Time.deltaTime * Horizontalspeed * Vector2.left);

        transform.Translate(Time.deltaTime * Verticalspeed * Vector2.up * dir);

        if ((transform.position.y > baseY + heightLimit) || (transform.position.y < baseY - heightLimit))
        {
            dir *= -1.0f;
        }

    }


    void Start()
    {
        baseY = transform.position.y;
    }
    // Update is called once per frame

    void Update()
    {
        EnemyMove();
    }
}
