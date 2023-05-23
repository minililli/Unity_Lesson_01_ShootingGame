using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    StageClear stageClear;  //게임 스테이지 클리어 판단
    bool onClear = false;
    private Animator anim;
    private PlayerInputActions inputActions; //InputActions 인스턴스 생성
    private Vector2 inputDir = Vector2.zero; //움직임 스무스하게 하기위한 작업 2.초기화
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    [Header("플레이어의 기본정보")]
    // 움직이는 속도 수정
    public float speed = 10.0f;
    public int initialLife = 3;
    private int life;
    public int Life
    {
        get => life;
        set
        {
            if (!isDead)
            {
                if (life > value) //라이프가 감소한 상황이면
                {
                    OnHit();                //맞았을 때의 동작이 있는 함수 실행
                }

                life = value;
                Debug.Log($"{life}");
                onLifeChange?.Invoke(life); //델리게이트에 연결된 함수들 실행

            }
            if (life <= 0)
            {
                OnDie();                    //죽었을 때의 동작이 있는 함수 실행
            }

        }
    }



    public Action<int> onLifeChange;
    public Action<Player> onDie;

    [Header("피격관련 정보")]
    // 플레이어의 총알 타입
    Transform[] fireTransform;
    public PoolObjectType bulletType;
    // power아이템 적용 갯수(단계)
    int power = 0;
    public int Power
    {
        get => power;
        set
        {
            power = value;
            if (power > 3)
            {
                AddScore(extraScore);
            }
            power = Mathf.Clamp(power, 1, 3);

            RefreshFirePos(power);
        }
    }

    //총알 각도
    private float fireAngle = 30.0f;


    //총알 발사 시간 간격
    public float fireInterval = 0.5f;
    //연사용 코루틴을 저장할 변수
    IEnumerator fireCoroutine;
    // 발사 이펙트 위치
    GameObject fireFlash;

    //무적상태관련변수
    float invincibleTime = 2.0f;
    bool isDead = false;
    bool isInvincibleMode = false;
    //무적일 때 시간 누적용(MathF.COS에서 사용할 용도)
    private float timeElapsed = 0.0f;



    [Header("점수관련 정보")]
    private int score = 0;
    public int Score //프로퍼티(Property)
    {
        get { return score; }
        private set
        {
            score = value;
            onScoreChange?.Invoke(score);
            //Debug.Log($"점수 : {score}");
        }
    }
    /// <summary>
    /// Power 아이템의 추가점수
    /// </summary>
    private int extraScore = 50;

    //delegate
    public Action<int> onScoreChange;

    public void AddScore(int plus)
    {
        Score += plus; //대문자 Score여야함!
        //Debug.Log($"점수 : {score}");
    }

    //--------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        inputActions = new PlayerInputActions(); //플레이어 이동처리 
        stageClear = FindObjectOfType<StageClear>();    //스테이지클리어여부확인
        stageClear.onStageClear += () => onClear = true;


        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Transform fireRoot = transform.GetChild(0);         //발사 위치 찾기
        fireTransform = new Transform[fireRoot.childCount]; //변화 발사 위치들 찾아놓기
        for (int i = 0; i < fireRoot.childCount; i++)
        {
            fireTransform[i] = fireRoot.GetChild(i);

        }

        fireFlash = transform.GetChild(1).gameObject;
        fireFlash.SetActive(false);

        fireCoroutine = FireCoroutine();

    }
    private void Start()
    {
        Power = 1;
        Life = initialLife;
        stageClear.onStageClear += () => { InputDisable(); gameObject.SetActive(false); };
    }
    //이 게임 오브젝트가 완성된 이후 활성화 할 때 실행되는 함수
    private void OnEnable()
    {
            inputActions.Player.Enable(); // 사용할 액션맵 Player 등록
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
        anim.SetFloat("InputY", dir.y); // 애니메이터에 있는 InputY파라메터에 dir.y값을 준다.
        inputDir = dir;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Life--;
        }
        else if (collision.gameObject.CompareTag("PowerUp"))
        {
            Power++;
            collision.gameObject.SetActive(false);
        }

    }
    private void Update()
    {
        if (isInvincibleMode)
        {
            timeElapsed += Time.deltaTime * 30;
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f;           //cos을 1~0~1로 변경하는 작업
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }

    private void FixedUpdate()
    {
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
            for (int i = 0; i < power; i++)
            {
                GameObject obj = Factory.Inst.GetObject(PoolObjectType.Bullet);    //Bullet 가져오기
                Transform firePos = fireTransform[i];                   //발사 위치 설정
                obj.transform.position = firePos.position;              //Bullet 위치 = 가져온 발사위치;
                obj.transform.rotation = firePos.rotation;              //Bullet 회전 = 가져온 회전값;
            }
            StartCoroutine(FlashEffect());
            yield return new WaitForSeconds(fireInterval);
        }

    }
    private void OnHit()
    {
        Power--;
        StartCoroutine(EnterInvincibleMode());
    }
    /// <summary>
    /// 무적상태 진입용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterInvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");     // 레이어 변경("invincible")
        isInvincibleMode = true;                                     // 무적모드 실행했다고 표시
        timeElapsed = 0.0f;                                         // 시간 카운터 초기화

        yield return new WaitForSeconds(invincibleTime);            // invincibleTime동안 대기

        spriteRenderer.color = Color.white;                         // 색깔변한 상태끝날 때를 대비해서 초기화
        isInvincibleMode = false;                                   //무적 모드 끝났다고 표시
        gameObject.layer = LayerMask.NameToLayer("Player");         // 레이어 되돌리기
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
        rigid.gravityScale = 1.0f;

        onDie?.Invoke(this); //죽었다고 알리기
    }
    /// <summary>
    /// 발사위치 초기화 함수
    /// </summary>
    /// <param name="power"> 파워아이템 갯수(단계) </param>
    private void RefreshFirePos(int power)
    {
        for (int i = 0; i < fireTransform.Length; i++)
        {
            fireTransform[i].gameObject.SetActive(false);   //FireTransform 모두 비활성화하기
        }

        for (int i = 0; i < power; i++)             //power 숫자에 맞게 fireTransform 활성화
        {
            Transform firePos = fireTransform[i];
            firePos.localPosition = Vector3.zero;
            firePos.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
            firePos.Translate(1, 0, 0);

            fireTransform[i].gameObject.SetActive(true);
        }
    }
}
