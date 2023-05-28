using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    Player player;
    TextMeshProUGUI score;

    float currentScore;
    int targetScore;
    public int TargetScore
    {
        get => targetScore;
    }
    float minScoreUpSpeed = 30.0f;

    [Range(1f, 100.0f)]
    public float scoreupSpeed = 50.0f;

    bool onDie = false;
    public Action<int> StageClearScore;
    private void Awake()
    {
        Transform child = transform.GetChild(1);                  //두번째 자식(Number) 가져오기
        score = child.GetComponentInChildren<TextMeshProUGUI>(); //두번째 자식 컴포넌트 가져오기
    }
    private void Start()
    {
        StartCoroutine(StartScore());
        player = FindObjectOfType<Player>();
        player.onScoreChange += ScoreUpdate;
        player.onDie += StopScore;
    }


    private void StopScore(Player obj)
    {
        onDie = true;
        currentScore = targetScore;
    }

    IEnumerator StartScore()
    {
        score.text = "Start!";

        yield return new WaitForSeconds(1);

        score.text = $"{player.Score}";
    }

    void ScoreUpdate(int newScore)
    {
        targetScore = newScore;
    }

    void RefreshScore()
    {
        if (currentScore < targetScore)
        {
            float speed = Mathf.Max((targetScore - currentScore) * 10.0f, minScoreUpSpeed);
            if (targetScore - currentScore > 1000)
            {
                speed = Mathf.Max((targetScore - currentScore) * 100f, minScoreUpSpeed);
            }
            currentScore += Time.deltaTime * scoreupSpeed; //currentScore를 초당 1씩 증가시킨다. 
            currentScore = Mathf.Min(currentScore, targetScore);
            score.text = $"{currentScore:f0}"; // 소수점아래 0개까지 증가시킨다.
            //currentScore를 점수 차이에 비례해서 증가시킨다.(최저 minScoreUpSpeed);
        }

    }

    private void Update()
    {
            RefreshScore();   
    }

}
