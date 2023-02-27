using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour //오브젝트 풀이 사용할 게임오브젝트
{
    public Action onDisable;
    // 게임 오브젝트가 비활성화될 때 실행
    protected virtual void OnDisable() //쓸지도모르니 만든다 ~ Virtual : 부모클래스말고 자식클래스꺼 사용하도록 할때
    {
        onDisable?.Invoke(); //이 델리게이트에 등록된 함수들 실행
    }
    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }

}
