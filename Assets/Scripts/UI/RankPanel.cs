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
    bool isUpdated;
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

    void onNameInputEnd(string text)
    {
        rankerNames[updatedIndex] = text;
        inputField.gameObject.SetActive(false);
        SaveRankingData();
        RefreshRankLines();

    }

    void RankUpdate(Player player)
    {
        isUpdated = false;
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
    void SaveRankingData()
    {
        //PlayerPrefs.SetInt("Score", 10);              // 컴퓨터에 Score라는 이름으로 10을 저장

        //SaveData saveData = new SaveData();
        SaveData saveData = new();  // 윗줄과 같은 코드(타입을 알 수 있기 때문에 생략한 것)
        saveData.rankerNames = rankerNames;             // 생성한 인스턴스에 데이터 기록
        saveData.highScores = highScores;

        string json = JsonUtility.ToJson(saveData);     // saveData에 있는 내용을 json 양식으로 설정된 string으로 변경

        string path = $"{Application.dataPath}/Save/";  // 저장될 경로 구하기(에디터에서는 Assets 폴더)
        if (!Directory.Exists(path))                    // path에 저장된 폴더가 있는지 확인
        {
            Directory.CreateDirectory(path);            // 폴더가 없으면 그 폴더를 만든다.
        }

        string fullPath = $"{path}Save.json";           // 전체 경로 = 폴더 + 파일이름 + 파일확장자
        File.WriteAllText(fullPath, json);              // fullPath에 json내용 파일로 기록하기        
    }

}
