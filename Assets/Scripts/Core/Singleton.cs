using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{

    // 초기화를 진행한 표시를 나타내는 플래그
    private bool initialized = false;
    
    // 설정 안되었다는 것을 표시하기 위한 상수
    private const int NOT_SET = -1;
    private int mainSceneIndex = NOT_SET;

    //다른곳에서 접근 못하게(private) 해놓은 싱글톤용 객체.
    private static T instance;

    // 이미 종료처리에 들어갔는지 표시하기 위한 용도
    private static bool isShutDown = false;

    public static T Inst //프로퍼티 생성
    {
        get
        {
            if (isShutDown)         //종료처리 들어갔으면,
            {
                Debug.LogWarning($"{typeof(T)} 싱글톤은 이미 삭제되었다."); //이게 뜨면 사용한 곳에서 코드를 잘 못 만든 것. 이 코드 수행되면 오류다.
                return null;        // null 리턴
            }
            
            
            //접근한 시점에서 만약에 싱글톤이 있는지 확인
            if (instance == null)
            { 
                // 없으면 만들어진적이 없다.
                
                T obj = FindObjectOfType<T>();
                
                //만약에 obj가 있으면; 에디터에서 만들어진 것이 있는지 확인
                 if (obj == null)
                    //null이면 에디터에서 만들어진 것이 없다.
                {
                    GameObject gameObj = new GameObject();   //빈오브젝트 생성
                    gameObj.name = typeof(T).Name;           //이름 변경
                    obj = gameObj.AddComponent<T>();         //싱글톤을 컴포넌트로 추가
                    
                }
                instance = obj;         // 없어서 새로 만든 것이든 에디터가 만든 것이든 instance에 저장
                DontDestroyOnLoad(obj); //씬이 넘어가더라도 Destroy 하지말아라
            }

            return instance; //instance 리턴 (없었으면 새로 만들고, 있었으면 있던것, 그래서 무조건 null이아니다.)
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;                       //인스턴스 등록하고 Generic 타입일 때는 as T 붙여줘야함.
            DontDestroyOnLoad(instance.gameObject);     // 씬이 닫히더라도 지우지 말아라.
        }
        else
        {
            if (instance != this)                      //인스턴스가 이미 존재하고 있으면( null 이 아니면 )
            { 
                //Awake() 되기 전에 다른 코드에서 프로퍼티를 통해 접근했다면,
                Destroy(this.gameObject); // "나(obj)를 파괴해라.
            } 
        }
    }

    private void OnApplicationQuit()
    {
        isShutDown = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //씬이 로드되면 호출이 되는 함수
    //Scene : 로드된 씬
    //mode : 로드 모드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PreInitialize();
        Initialize();
    }

    protected virtual void PreInitialize()
    {
        if (!initialized)
        {
            initialized = true;
            Scene active = SceneManager.GetActiveScene();
            mainSceneIndex = active.buildIndex;
        }
    }

    protected virtual void Initialize()
    {
       
    }

    protected virtual void ResetData()
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}



/*일반싱글톤 예제
public class TestSingleton
{
    // static변수를 만들어서 객체를 만들지 않고 사용할 수 있게 만들기
    private static TestSingleton instance = null;

    // 다른 곳에서 instance를 수정하지 못하도록 읽기 전용 프로퍼티 만들기
    public static TestSingleton Instance
    {
        get
        {
            if (instance == null)   // 처음 접근했을 때 new하기.
            {
                instance = new TestSingleton();
            }
            return instance;        // 항상 return될 때 값은 존재한다.
        }
    }

    // 중복생성 방지 목적. private으로 생성자를 만들어 기본 public생성자가 생성되지 않게 막기
    private TestSingleton()     
    {
    }
}
*/