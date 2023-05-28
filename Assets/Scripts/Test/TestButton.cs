using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(ReStart);
    }

    private void ReStart()
    {
        Debug.Log("Click완료");
    }
}
