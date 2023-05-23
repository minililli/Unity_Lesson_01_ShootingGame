using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    PlayerInputActions inputActions;
    Galaxy galaxy;

    RectTransform rect;
    CanvasGroup panelGroup;
    TextMeshProUGUI restartText;
    TextMeshProUGUI exitText;

    bool stageClear = false;
    public Action onStageClear; //스테이지 종료됨을 알리는 델리게이트


    WaitForSeconds reStartInterval;
    public float reStartIntervaltime = 0.3f;
    WaitForSeconds exitInterval;
    public float exitIntervaltime = 0.5f;



    private void Awake()
    {
        inputActions = new PlayerInputActions();

        galaxy = FindObjectOfType<Galaxy>();
        rect = GetComponent<RectTransform>();
        panelGroup = GetComponent<CanvasGroup>();


        restartText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        exitText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        panelGroup.alpha = 0.0f;
    }

   
    private void OnDisable()
    {
        inputActions.UI.Start.performed -= NextScene;
        inputActions.UI.Exit.performed -= ExitScene;
        inputActions.UI.Disable();
    }

    private void ExitScene(InputAction.CallbackContext _)
    {
        Application.Quit();
    }

    private void NextScene(InputAction.CallbackContext _)
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        stageClear = false;
        galaxy.StageClearAlarm += OnStageClear;



        reStartInterval = new WaitForSeconds(reStartIntervaltime);
        exitInterval = new WaitForSeconds(exitIntervaltime);

        StartCoroutine(FlashText(restartText, reStartInterval));
        StartCoroutine(FlashText(exitText, exitInterval));


    }

    public void OnStageClear()
    {
        panelGroup.alpha = 1.0f;
        stageClear = true;
        UIEnable();
        onStageClear?.Invoke();
    }
    private void UIEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Exit.performed += ExitScene;
        inputActions.UI.Start.performed += NextScene;
    }

    IEnumerator FlashText(TextMeshProUGUI obj, WaitForSeconds interval)
    {
        while (true)
        {
            obj.enabled = false;
            yield return interval;
            obj.enabled = true;
            yield return interval;
        }
    }
}
