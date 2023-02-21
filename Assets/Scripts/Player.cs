using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Animator anim;
    //2. InputSystem을 통한 움직임 구현
    PlayerInputActions inputActions; //InputActions 인스턴스 생성
    Vector2 inputDir = Vector2.zero; //움직임 스무스하게 하기위한 작업 2.초기화

    // 움직이는 속도 수정
    public float speed = 15.0f;
    public GameObject bullet;

    Transform fireTransform;



    // Awake 이 게임 오브젝트가 생성 완료 되었을 때 실행되는 함수 - Start()보다 더 빠름.
    private void Awake()
    {
        anim = GetComponent<Animator>(); // 한번 찾아놓고 쓸 것_성능문제
        inputActions = new PlayerInputActions(); // new를 유일하게 사용하는 PlayerInputActions();
        fireTransform = transform.GetChild(0);

    }

    //이 게임 오브젝트가 완성된 이후 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable(); // 사용할 액션맵 Player 등록
        //inputActions.Player.Fire.started;    // 버튼을 누른 직후 
        //inputActions.Player.Fire.performed;  // 버튼을 충분히 눌렀을 때 <조이스틱민감.키보드는 Started와 크게 다르지 않음.>
        //inputActions.Player.Fire.canceled;   // 버튼을 뗀 직후

        inputActions.Player.Fire.performed += OnFire;
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
        inputActions.Player.Fire.performed -= OnFire;
        
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");
        GameObject obj = Instantiate(bullet);
        obj.transform.position = fireTransform.position;
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



    // 시작할 때 한번 실행되는 함수  
    void Start()
    {
        //Debug.Log("Start");
        
    }

    void Update()
    {

        //transform.position += (Vector3)(Time.deltaTime * speed * inputDir);//움직임 스무스하게 하기위한 작업 4
        transform.Translate(Time.deltaTime * speed * inputDir); //: 위와 동일한 움직임 구현하는 문장
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


    }
}
