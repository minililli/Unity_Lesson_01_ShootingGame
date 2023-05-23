using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    StageClear stageClear;
    TextMeshProUGUI timeText;
    int second; // 초단위변수
    int milisecond; // 밀리초단위
    public int stageEndTime = 20;   //스테이지 종료 시간 : 20seconds
    public int bosstime = 4; //보스 등장시간
    public Action Bosstime; //보스 등장하라고 알리는 델리게이트
    bool stop = false; // 종료시 멈추기
    int timer;  // 누적시간 계산용
    public int Timer
    {
        get => timer;
        private set
        {
            timer = value;
            second = timer / 60;
            milisecond = timer % 60;
        }
    }
    private void Awake()
    {
        stageClear = FindObjectOfType<StageClear>();
        timeText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        FindObjectOfType<Player>().onDie += Stop;
        stageClear.onStageClear += () => stop = true;
    }

    private void Stop(Player obj)
    {
        stop = true;
    }

    void Update()
    {
        if (!stop)
        {
            Timer++;

        }
        TimeText();
    }

    void TimeText()
    {
        timeText.text = $"{second}. {milisecond}sec";
        if (Timer == bosstime * 60)
        {
            Bosstime?.Invoke();
        }
    }
}
