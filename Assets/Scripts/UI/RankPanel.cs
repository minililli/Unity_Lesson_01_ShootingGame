using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    Player player;
    RankLine[] ranklines = null;
    int[] highScores = null;
    string[] rankerNames = null;
    TMP_InputField inputField;
    int rankCount;
    int NotUpdated = -1;
    int updatedIndex;

    private void Awake()
    {
        ranklines = GetComponentsInChildren<RankLine>();
        inputField = GetComponentInChildren<TMP_InputField>();
        int rankCount = ranklines.Length;
        highScores = new int[rankCount];
        rankerNames= new string[rankCount];

        LoadRankingData();
    }
    private void Start()
    {
        inputField.gameObject.SetActive(false);

    }

    void onNameInput()
    {
        
    }

    void RankUpdate(Player player)
    {
        bool isUpdated = false;
        int newScore = player.Score;                            //새점수
            
        for(int i=0; i < rankCount; i++)                        //랭크카운트갯수만큼 반복문실행
        {
            if (highScores[i] < newScore)
            {
                for(int j = rankCount-1; j>i; j--)
                {
                    highScores[j] = highScores[j - i];
                    rankerNames[j] = rankerNames[j - i];
                }
                highScores[i] = newScore;
                rankerNames[i] = "";
                updatedIndex = i;

                Vector3 newPos = inputField.transform.position;
                newPos.y = ranklines[i].transform.position.y;
                inputField.transform.position = newPos;
                inputField.gameObject.SetActive(true);
                break;

            }
        }
        inputField.gameObject.SetActive(false);
    }
    bool LoadRankingData()
    {
        bool result = false;
        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";

        result = Directory.Exists(path) && File.Exists(fullPath);

        if (result)
        {
            string json = File.ReadAllText(fullPath);
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);
            highScores = loadData.highScores;
            rankerNames = loadData.rankerNames;
        }
        else
        {
            int size = ranklines.Length;
            for(int i = 0; i < size; i++)
            {
                int resultScore = 1;
                for (int j = size - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; // 10만, 1만, 천, 백, 십

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}";
            }
        }
        RefreshRankLines();
        return result;
    }
    void RefreshRankLines()
    {
        for(int i=0; i<ranklines.Length; i++)
        {
            ranklines[i].SetData(rankerNames[i], highScores[i]);
        }
    }


}
