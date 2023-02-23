using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Range(0.1f, 3.0f)]
    public float speed = 1.0f;

    public float amplitude = 1; // amplitude=진폭 => 사인 결과값을 증폭시킬 변수 선언(위아래 차이 결정)
    public float frequency = 1; // 사인그래프가 한번 도는데 걸리는 시간


    float timeElapsed = 0.0f;
    float baseY;

    public GameObject explosionPrefab;

    public int score = 10;
    Player player = null;

    //살아있는지 여부. true면 살아있고, false면 죽어있다.
    bool isAlive = true;


    //Player에 처음 한번만 값을 설정 가능한 프로퍼티. 쓰기전용
    public Player TargetPlayer
    {
        set
        {
            if (player == null) // player가 null일 때만 설정 가능하다.
            {
                player = value;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        baseY = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime * frequency;
        float x = transform.position.x - (speed * Time.deltaTime); // x는 현재위치에서 약간 왼쪽으로 이동
        float y = baseY + Mathf.Sin(timeElapsed) * amplitude;

        transform.position = new Vector3(x, y, 0); // 구한 x,y를 이용해 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        { Die(); }

    }
    
    //죽었을 때 실행되는 함수
    void Die()
    {

        if (isAlive)
        {
            isAlive = false;
            //GameObject obj = GameObject.Find("Player"); //이름으로 게임오브젝트 찾기-Not good
            //GameObject player = GameObject.FindGameObjectWithTag("Player"); //게임 태그 "Player"를 찾기 (태그가 많을 시 제일 빠른 값 반환) -Not good 씬 전체를 뒤짐
            //Player player = FindObjectOfType<Player>(); //타입으로 찾기 -Not good;
            player.AddScore(score);

            GameObject obj = Instantiate(explosionPrefab); //폭발이벤트 생성
            obj.transform.position = transform.position;   //위치는 적의 위치로 설정

            Destroy(gameObject);                           // 적 삭제
        }
    }    
}

