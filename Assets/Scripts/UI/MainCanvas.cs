using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class MainCanvas : MonoBehaviour
{
    Canvas canvas;
    float Width;
    float Height;

    PlayerInputActions inputActions;

    TextMeshProUGUI startText;
    TextMeshProUGUI exitText;
    TextMeshProUGUI Title;
    Image asteroid1;
    Image asteroid2;
    Image asteroid3;
    float asteroidSpeed = 90.0f;
    float varientSpeed;
    Image player;
    float playerSpeed = 5.0f;
    Vector3 InitPos;

    WaitForSeconds startInterval;
    public float startIntervaltime = 0.3f;
    WaitForSeconds exitInterval;
    public float exitIntervaltime = 0.5f;


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        Width = canvas.GetComponent<RectTransform>().sizeDelta.x;
        Height = canvas.GetComponent<RectTransform>().sizeDelta.y;

        Transform parent = transform.GetChild(0);
        asteroid1 = parent.GetChild(1).GetComponent<Image>();
        asteroid2 = parent.GetChild(2).GetComponent<Image>();
        asteroid3 = parent.GetChild(3).GetComponent<Image>();
        player = parent.GetChild(4).GetComponent<Image>();
        parent = transform.GetChild(1);
        startText = parent.GetComponent<TextMeshProUGUI>();
        parent = transform.GetChild(2);
        exitText = parent.GetComponent<TextMeshProUGUI>();
        parent = transform.GetChild(3);
        Title = parent.GetComponent<TextMeshProUGUI>();
        
    }

    private void OnEnable()
    {
        
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Exit.performed += ExitScene;
        inputActions.UI.Start.performed += NextScene;
    }

    private void OnDisable()
    {
        inputActions.UI.Start.performed -= NextScene;
        inputActions.UI.Exit.performed -= ExitScene;
        inputActions.UI.Disable();
    }

    private void ExitScene(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    private void NextScene(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        InitPos = player.transform.position;

        varientSpeed = UnityEngine.Random.Range(-1f,1f);
        

        startInterval = new WaitForSeconds(startIntervaltime);
        exitInterval = new WaitForSeconds(exitIntervaltime);

        StartCoroutine(FlashText(startText, startInterval));
        StartCoroutine(FlashText(exitText, exitInterval));

    }
    private void Update()
    {
        asteroid1.transform.Rotate(0, 0, Time.deltaTime * asteroidSpeed);
        asteroid2.transform.Rotate(0, 0, Time.deltaTime * asteroidSpeed * varientSpeed);
        asteroid3.transform.Rotate(0, 0, Time.deltaTime * asteroidSpeed * varientSpeed);

        player.transform.position += (Vector3.right * playerSpeed);
        if (player.transform.position.x > canvas.transform.position.x + 1400f)
        {
            player.transform.position = InitPos;
        }
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
