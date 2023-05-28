using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    StageClear stageClear;
    TextMeshProUGUI timeText;
    WaitForSeconds secondCounter;
   
    [Tooltip("int형 변수_초단위")]
    public int stageEndTime = 40;   //스테이지 종료 시간 : 20seconds
    public int bossTime = 20; //보스 등장시간

    int minute; // 분단위변수
    int second; // 초단위변수
    public Action BossTime; //보스 등장하라고 알리는 델리게이트
    public Action TimeOver; //게임 종료 시간이 되었다고 알리는 델리게이트
    bool stop = false; // 종료시 멈추기
    int timer;  // 누적시간 계산용
    public int Timer
    {
        get => timer;
        private set
        {
            timer = value;
            minute = timer / 60;
            second = timer % 60;
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
        secondCounter = new WaitForSeconds(1.0f);
        if (!stop)
        {
            StartCoroutine(TimeChecker());
        }
        else
        {
            StopCoroutine(TimeChecker());
        }
    }
    IEnumerator TimeChecker()
    {
        while (!stop)
        {
            yield return secondCounter;
            Timer++;
        }
    }
    private void Stop(Player obj)
    {
        stop = true;
    }

    void Update()
    {
        if (!stop)
        {
            TimeText();
        }
    }

    void TimeText()
    {
        timeText.text = $"{minute}min {second}sec";

        if (timer == bossTime)
        {
            BossTime?.Invoke();
        }

        if (timer == stageEndTime)
        {
            TimeOver?.Invoke();
        }
    }
}
