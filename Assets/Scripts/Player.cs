using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    //InputActions 인스턴스 생성
    PlayerInputActions inputActions;
   
    //이 게임 오브젝트가 생성 완료 되었을 때 실행되는 함수 - Start()보다 더 빠름.
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    //이 게임 오브젝트가 완성된 이후 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
        inputActions.Player.Enable();
        //inputActions.Player.Fire.started;    // 버튼을 누른 직후 
        //inputActions.Player.Fire.performed;  // 버튼을 충분히 눌렀을 때 <조이스틱민감.키보드는 Started와 크게 다르지 않음.>
        //inputActions.Player.Fire.canceled;   // 버튼을 뗀 직후

        inputActions.Player.Fire.performed += OnFire;


        //실습
        inputActions.Player.Bomb.performed += OnBomb;
    }

    //이 게임 오브젝트가 비활성화될 때 실행되는 함수
    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Player.Bomb.performed -= OnBomb;
    }

    private void OnFire(InputAction.CallbackContext obj) //자동 생성 된다는데 안생성되서 직접침...
    {
        Debug.Log("Fire");
    }

    //실습 OnBomb
    private void OnBomb(InputAction.CallbackContext obj)
    {
        Debug.Log("Bomb");
    }

    // Start is called before the first frame updates
    // 시작할 때 한번 실행되는 함수  
    void Start()
    {
        //Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");

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
