using System;
using System.Collections;
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

    [Header("플레이어의 기본정보")]
    // 움직이는 속도 수정
    public float speed = 10.0f;
    private int life;
    public int Life
    {
        get => life;
        set
        {
            life = value;
        }
    }

    [Header("피격관련 정보")]
    // 플레이어의 총알 타입
    public PoolObjectType bulletType;
    int power = 0;
    public int Power
    {
        get => power;
        set => power = value;
    }
    public float fireInterval = 0.5f;

    //연사용 코루틴을 저장할 변수
    IEnumerator fireCoroutine;
    GameObject fireFlash;



    [Header("점수관련 정보")]
    private int score = 0;
    public int Score //프로퍼티(Property)
    { 
        get { return score; }   // 다른곳에서 특정 값(score) 확인할 때 사용됨 get => score;
        private set             // 기본적으로 다른곳에서 특정 값(score) 설정할 때 사용됨. 앞에 private 붙이면, 자신만 사용가능
        { 
            score = value;
            onScoreChange?.Invoke(score);
            //Debug.Log($"점수 : {score}");
        }  
    }

    //delegate
    public Action<int> onScoreChange;
  
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
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
    }

    //이 게임 오브젝트가 비활성화될 때 실행되는 함수
    private void OnDisable() // Enable과 순서가 반대!
    {
        InputDisable();
    }

    void InputDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Disable();
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Life--;
        }
        if(collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;
        }    
        
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
}
