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
    
    [Range(1f,100.0f)]
    public float scoreupSpeed = 50.0f;

    private void Awake()
    {
        
        Transform child = transform.GetChild(1);                  //두번째 자식(Number) 가져오기
        score = child.GetComponentInChildren<TextMeshProUGUI>(); //두번째 자식 컴포넌트 가져오기
    }
    private void Start()
    {
        StartCoroutine(StartScore());
        player = FindObjectOfType<Player>();
        //StartCoroutine(StartScore());
        player.onScoreChange += ScoreUpdate;
        
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
        if(currentScore < targetScore)
        {

            currentScore += Time.deltaTime * scoreupSpeed; //currentScore를 초당 1씩 증가시킨다. 
            currentScore = Mathf.Min(currentScore, targetScore);
            score.text = $"{currentScore:f0}"; // 소수점아래 0개까지 증가시킨다.
        }
           
    }

    private void Update()
    {
        RefreshScore();
    }

}
