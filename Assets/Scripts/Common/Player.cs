using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    Animator anim;
    //2. InputSystem을 통한 움직임 구현
    PlayerInputActions inputActions; //InputActions 인스턴스 생성
    Vector2 inputDir = Vector2.zero; //움직임 스무스하게 하기위한 작업 2.초기화
    Transform fireTransform;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    [Header("플레이어의 기본정보")]
    // 움직이는 속도 수정
    public float speed = 10.0f;
    int initialLife = 3;
    private int life;
    public int Life
    {
        get => life;
        set
        {
            life = value;
            Debug.Log($"{life}");

            if(life<=0)
            {
                OnDie();
                onLifeChange?.Invoke(Life);
            }
        }
    }

    public Action<int> onLifeChange;
    public Action<Player> onDie;

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

    //무적상태관련변수
    float invincibleTime = 2;
    bool isDead = false;
    bool isInvincibleMode = false;
    //무적일 때 시간 누적용(MathF.COS에서 사용할 용도)
    private float timeElapsed = 0.0f;



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

    //--------------------------------------------------------------------------------------------------------

    // Awake 이 게임 오브젝트가 생성 완료 되었을 때 실행되는 함수 - Start()보다 더 빠름.
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 한번 찾아놓고 쓸 것_성능문제
        inputActions = new PlayerInputActions(); // new를 유일하게 사용하는 PlayerInputActions();
        SpriteRenderer spriteRenderer= GetComponent<SpriteRenderer>();
        
        fireTransform = transform.GetChild(0);
        fireFlash = transform.GetChild(1).gameObject;
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();

    }
    private void Start()
    {
        Power = 1;
        Life = initialLife;
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
        else if(collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;
            collision.gameObject.SetActive(false);
        }    
        
    }

    IEnumerator onInvincible()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");     // 레이어 변경
        isInvincibleMode= true;                                     // 무적모드 실행했다고 표시
        timeElapsed = 0.0f;                                         // 시간 카운터 초기화
        yield return new WaitForSeconds(invincibleTime);            // invincibleTime동안 대기
        spriteRenderer.color = Color.white;                         // 색깔변한 상태끝날 때를 대비해서 초기화
        isInvincibleMode = false;                                   //무적 모드 끝났다고 표시
        gameObject.layer = LayerMask.NameToLayer("Player");         // 레이어 되돌리기
    }

    private void Update()
    {
        if (isInvincibleMode)
        {
            timeElapsed += Time.deltaTime * 30;
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
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

        if (isDead)
        {

            rigid.AddForce(Vector2.left * 0.3f, ForceMode2D.Impulse);
            rigid.AddTorque(30.0f);
        }
        else
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * inputDir);
        }
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
        while (true)
        {

            GameObject obj = Factory.Inst.GetObject(bulletType);
            obj.transform.position = fireTransform.position;
            StartCoroutine(FlashEffect());
            yield return new WaitForSeconds(fireInterval);
        }

    }
    private void OnDie()
    {
        isDead = true;
        life = 0;

        Collider2D bodyCollider = GetComponent<Collider2D>();
        bodyCollider.enabled = false;

        GameObject effect = Factory.Inst.GetObject(PoolObjectType.Explosion);
        effect.transform.position = transform.position;

        InputDisable();
        inputDir = Vector3.zero;

        StopCoroutine(fireCoroutine);

        //무적모드 취소
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isInvincibleMode = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        rigid.freezeRotation = false;

        onDie?.Invoke(this);
    }

}
