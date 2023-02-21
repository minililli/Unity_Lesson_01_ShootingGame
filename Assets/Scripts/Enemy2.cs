using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    public float speed = 5.0f;
    public float bounceTime = 1.0f;
    float currentTime = 0.0f;
    
    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector2(-1, 1);
        dir.Normalize(); // Normalize : 방향만 남김 = 길이가 1이다.
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime; // 시간 계속 누적하기
        if(currentTime > bounceTime)
        {
            currentTime = 0.0f;
            dir.y = -dir.y;
        }

        transform.Translate(Time.deltaTime * speed * dir);

    }
}
