using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Test_PowerUp : Test_Base
{
    Player player;
    private void Start()
    {
       player = FindObjectOfType<Player>();
    }
    protected override void Test1(InputAction.CallbackContext _)
    {
        //player.power = 1;
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        //player.power = 2;
    }
    protected override void Test3(InputAction.CallbackContext _)
    {
        //player.power = 3;
    }
    protected override void Test4(InputAction.CallbackContext _)
    {
      
    }

}
