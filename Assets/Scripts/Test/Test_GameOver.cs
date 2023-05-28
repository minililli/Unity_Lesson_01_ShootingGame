using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : Test_Base
{
    //1번을 누르면 "/Asset/Save/TestSave.json" 이라는 이름으로 기록해보기
    //2번을 누르면 "/Asset/Save/TestSave.json" 읽어서 Debug로 출력하기
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
        player.AddScore(1500); 
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        
    }

}
