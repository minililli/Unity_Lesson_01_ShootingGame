using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//직렬화 가능하다고 표시해놓은 attribute
[Serializable] 
public class SaveData
{
    public string[] rankerNames;
    public int[] highScores;
}
