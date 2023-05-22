using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PoolObject // PoolObject를 반드시 상속받아야한다.
{
    //풀에 생성해 놓을 오브젝트
    public GameObject originalPrefab;
    public int poolSize = 64;

    T[] pool;
    Queue<T> readyQueue;

    //처음 만들어졌을 때 한번 실행될 코드(초기화 코드)
    public void Initialize()
    {
        pool = new T[poolSize];
        readyQueue = new Queue<T>(poolSize);

        GenerateObjects(0, poolSize, pool); // 첫번째 풀 생성
    }

    //오브젝트를 생성하고 배열에 추가하는 함수
    void GenerateObjects(int start, int end, T[] newArray)
    {
        for (int i = start; i < end; i++) //start부터 end까지 반복
        {
            GameObject obj = Instantiate(originalPrefab, transform); //프리팹 생성
            obj.gameObject.name = $"{originalPrefab.name}+{i}";     //이름 변경
            T comp = obj.GetComponent<T>();                         //컴포넌트 찾고 (poolObject타입)
          
            comp.onDisable += () => readyQueue.Enqueue(comp);     //비활성화될 때 + 람다 (parameter) => 리턴타입void, comp채우기 //레디큐에 넣기 
            
            newArray[i] = comp;                                     //풀배열에 넣고   
            obj.SetActive(false);                                  //비활성화
        }
    }

    public T GetObject()
    {
        if (readyQueue.Count > 0) // 큐에 오브젝트가 있는지 확인-큐에 오브젝트가 있으면, 
        {
            T obj = readyQueue.Dequeue();   //큐에서 하나 꺼내고
            obj.gameObject.SetActive(true); // 활성화 시킨 다음에
            return obj;                     // 리턴
        }

        else                                //큐에 오브젝트 없으면
        {
            Expandpool();                   //풀에 두배로 늘린다.
            return GetObject();             // 새롭게 하나 요청
        }
    }


    public T GetObject(Transform spawnTransform)
    {
        if (readyQueue.Count > 0) // 큐에 오브젝트가 있는지 확인-큐에 오브젝트가 있으면, 
        {
            T obj = readyQueue.Dequeue();   //큐에서 하나 꺼내고
            obj.transform.position = spawnTransform.position;   //spawnTransform에 위치시키고,
            obj.transform.rotation = spawnTransform.rotation;   //spawnTransform 회전값을 동일하게하고,
            obj.transform.localScale = spawnTransform.localScale;//spawnTransform 스케일을 동일하게 하고
            obj.gameObject.SetActive(true); // 활성화 시킨 다음에
            return obj;                     // 리턴
        }
        else                                            //큐에 오브젝트 없으면
        {
            Expandpool();                             //풀에 두배로 늘린다.
            return GetObject(spawnTransform);            // 새롭게 하나 요청
        }
    }

    private void Expandpool()
    {
        Debug.LogError("Pool을 증가시킵니다.");
        
        int newSize = poolSize * 2;     // 새로운 풀 크기 설정
        T[] newPool = new T[newSize];   // 새로운 풀 생성
        for (int i = 0; i < poolSize; i++)// 이전 풀에 있던 내용을 새 풀에 복사
        {
            pool[i] = newPool[i];
        }
        GenerateObjects(poolSize, newSize, newPool); // 이전 풀 이후부분에 오브젝트 생성하고 새 풀에 추가
        pool = newPool;                              //새 풀을 풀로 설정
        poolSize = newSize;
    }
}
