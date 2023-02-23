using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,5.0f); //this만 두면 script가 사라짐. => script가 적용된 gameObject를 삭제해라.
    }

    void GoBullet() //내가 만든 함수
    {
        transform.position += (Time.deltaTime * speed * transform.right); //->Vector2.right은 월드좌표로 이동함.
        //위와 동일한 코드
        //transform.Translate(Time.deltaTime * speed * Vector2.right,Space.Self);

        //local좌표와 world좌표
        //local좌표 : 각 오브젝트 별 기준으로 한 좌표계
        //world좌표 : 맵을 기준으로 한 좌표계

    }

    // Update is called once per frame
    void Update()
    {
        GoBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // 부딪친 게임오브젝트의 태그가 "Enemy"일때만 처리  
      //if(collision.gameObject.tag == "Enemy");는 절대로 하지 말 것. 더 느리고, 메모리도 많이 쓴다. 
        {
            OnHitEffect();
            Destroy(gameObject);
        }
    }
    private void OnHitEffect()
    {
        GameObject obj = Instantiate(effect);
        obj.transform.position = transform.position;
    }


}
