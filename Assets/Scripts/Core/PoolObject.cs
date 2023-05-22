using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour 
{
    // 게임 오브젝트가 비활성화될 때 실행
    public Action onDisable;

    protected virtual void OnDisable() 
    {
        onDisable?.Invoke(); //이 델리게이트에 등록된 함수들 실행
    }

    protected IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }

}
