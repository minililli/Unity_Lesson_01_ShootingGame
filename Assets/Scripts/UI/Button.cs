using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator anim;
    Button button;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        button = GetComponentInChildren<Button>();
        //button.onClick.AddListener(OnRestart);


    }
}
