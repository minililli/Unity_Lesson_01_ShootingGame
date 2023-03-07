using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Animator anim;
    //2. InputSystem을 통한 움직임 구현
    PlayerInputActions inputActions; //InputActions 인스턴스 생성
    Vector2 inputDir = Vector2.zero; //움직임 스무스하게 하기위한 작업 2.초기화

   
    Transform fireTransform;
    Rigidbody2D rigid;

    // 움직이는 속도 수정
    public float speed = 10.0f;
    // 플레이어의 총알 타입
    public PoolObjectType bulletType;
    
    public float fireInterval = 0.5f;
    

    //delegate
    public Action<int> onScoreChange;

    public int Score //프로퍼티(Property)
    { 
        get { return score; }   // 다른곳에서 특정 값(score) 확인할 때 사용됨 get => scroe;
        private set             // 기본적으로 다른곳에서 특정 값(score) 설정할 때 사용됨. 앞에 private 붙이면, 자신만 사용가능
        { 
            score = value;
            onScoreChange?.Invoke(score);
            //Debug.Log($"점수 : {score}");
        }  
    }

   
   
    GameObject fireFlash;

    int score = 0;

    //연사용 코루틴을 저장할 변수
    IEnumerator fireCoroutine;



    public void AddScore(int plus)
    {
        Score += plus; //대문자 Score여야함!
        //Debug.Log($"점수 : {score}");
    }


    // Awake 이 게임 오브젝트가 생성 완료 되었을 때 실행되는 함수 - Start()보다 더 빠름.
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 한번 찾아놓고 쓸 것_성능문제
        inputActions = new PlayerInputActions(); // new를 유일하게 사용하는 PlayerInputActions();
        
        
        fireTransform = transform.GetChild(0);
        fireFlash = transform.GetChild(1).gameObject;
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();

    }

    //이 게임 오브젝트가 완성된 이후 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable(); // 사용할 액션맵 Player 등록
        //inputActions.Player.Fire.started;    // 버튼을 누른 직후 
        //inputActions.Player.Fire.performed;  // 버튼을 충분히 눌렀을 때 <조이스틱민감.키보드는 Started와 크게 다르지 않음.>
        //inputActions.Player.Fire.canceled;   // 버튼을 뗀 직후

        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Bomb.performed += OnBomb; // 실습Bomb
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    //이 게임 오브젝트가 비활성화될 때 실행되는 함수
    private void OnDisable() // Enable과 순서가 반대!
    {
        inputActions.Player.Disable();

        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Bomb.performed -= OnBomb; // 실습Bomb
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
    }

    private void OnFireStart(InputAction.CallbackContext context)
    {
        //Debug.Log("Fire");
        StartCoroutine(fireCoroutine);



    }
    private void OnFireStop(InputAction.CallbackContext context)
    {
        StopCoroutine(fireCoroutine);

    }


    IEnumerator FireCoroutine()
    {
        while(true)
        {

            GameObject obj = Factory.Inst.GetObject(bulletType);     
            obj.transform.position = fireTransform.position;
            StartCoroutine(FlashEffect());
            yield return new WaitForSeconds(fireInterval);
        }

    }

    IEnumerator FlashEffect()
    {
        fireFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        fireFlash.SetActive(false);
    }

    private void OnBomb(InputAction.CallbackContext context) // 빠른작업 및 리팩터링-메서드추출 => 자동생성활용
    {
        Debug.Log("Bomb");
    }

    //실습 OnBomb 직접침
    /*private void OnBomb(InputAction.CallbackContext obj)
    {
        Debug.Log("Bomb");
    }*/

    private void OnMoveInput(InputAction.CallbackContext context) //이동 관련
    {
        Vector2 dir = context.ReadValue<Vector2>();
        //Debug.Log(dir);

        //Transform t = GetComponent<Transform>(); -> 느림/ transform은 일반적으로 가지고 있기 때문에 바로 .으로 활용가능함!
        anim.SetFloat("InputY",dir.y); // 애니메이터에 있는 InputY파라메터에 dir.y값을 준다.
        inputDir = dir;        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"충돌영역에 들어감 + 충돌대상 : {collision.gameObject.name}");
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log($"충돌영역에서 나감 + 충돌대상 : {collision.gameObject.name}");
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    Debug.Log("접촉해있으면서 움직이는중");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"트리거안에 들어감 + 트리거대상 : {collision.gameObject.name}");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log($"트리거에서 나감 + 트리거대상 : {collision.gameObject.name}");
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log("트리거에서 움직임");
    //}

    // 시작할 때 한번 실행되는 함수  
    void Start()
    {
        //Debug.Log("Start");
        //gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        // 항상 일정한 시간 간격으로 실행되는 업데이트
        // 물리 연산이 들어가는 것은 이쪽에서 실행
        
        //Debug.Log(Time.fixedDeltaTime);
        //움직이는 방법 두가지
        //rigid.MovePosition(); //특정위치로 순간이동시키기. 움직일때 막히면 거기서부터는 진행안함. 관성이 없는 움직임시킬때 유용함.
        //rigid Addforce(); //특정방향으로 힘을 가하기. 움직일때 막히면 거기서부터는 진행안함. 관성이있음. 

        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * inputDir);


    }
    /*void Update()
    {
        Debug.Log(Time.deltaTime);

        //transform.Translate(Time.deltaTime * speed * inputDir); //: 위와 동일한 움직임 구현하는 문장
        //transform.position += (Vector3)(Time.deltaTime * speed * inputDir);//움직임 스무스하게 하기위한 작업 4
        // 위 코드문해석 => (Update안에서 실행할 때) 초당 speed의 속도로 inputDir방향으로 이동
        //30프레임 컴퓨터의 deltaTime = 1/30 = 0.0333333, 120프레임 컴퓨터의 deltaTime = 1/120 = 0.008333333


        //Debug.Log("Update");
        // 1.InputManager 를 통한 움직임 구현 -> 반응속도 떨어지고 CPU 지속사용 등으로 게임 성능 저하됨.
        //if ( Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("W키가 눌러짐");
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("A키가 눌러짐");
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Debug.Log("S키가 눌러짐");
        //}


        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("D키가 눌러짐");
        //}

        //float inputHorizontal = Input.GetAxis("Horizontal"); //수평 방향 처리
        //float inputVertical = Input.GetAxis("Vertical"); //수직 방향 처리

        //Debug.Log(inputHorizontal);
        //Debug.Log(inputVertical);


    }*/
}
