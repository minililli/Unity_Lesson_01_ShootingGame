using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SpecialEnemy : Test_Base
{

    protected override void Test1(InputAction.CallbackContext context)
    {
        var test = Factory.Inst.GetPowerUp();

    }
}
