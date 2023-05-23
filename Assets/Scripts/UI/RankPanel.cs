using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLines = null;
    int[] highScores = null;    //랭킹별 최고점으로, 인덱스 작을수록 높은점수, 인덱스 클수록 낮은점수
    string[] rankerNames = null;

    int rankCount;

    /// <summary>
    /// 랭킹이 업데이트 되지 않았음을 표시하는 상수
    /// </summary>
    const int NotUpdated = -1;

    // updateIndex : 현재 업데이트가 된 랭킹의 인덱스
    int updatedIndex = NotUpdated;
    bool isUpdated;

    TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onEndEdit.AddListener(OnNameInputEnd);

        rankLines = GetComponentsInChildren<RankLine>();

        rankCount = rankLines.Length;
        highScores = new int[rankCount];        //배열 공간 확보
        rankerNames = new string[rankCount];

    }
    private void Start()
    {
        inputField.gameObject.SetActive(false);
        Player player = FindObjectOfType<Player>();
        player.onDie += RankUpdate;
        LoadRankingData();//데이터 읽기, 파일 없으면 디폴트값 주기
        SaveRankingData();
    }

    /// <summary>
    /// 이름 입력이 완료되었을 때 실행되는 함수
    /// </summary>
    /// <param name="text">입력받은 이름</param>
    private void OnNameInputEnd(string text)
    {
        rankerNames[updatedIndex] = text;       // 입력받은 텍스트를 해당 랭커의 이름으로 지정
        inputField.gameObject.SetActive(false); // 입력 완료되었으니 다시 안보이게 만들기
        SaveRankingData();  // 새로 저장하고 
        RefreshRankLines(); // UI 갱신
    }

    void RankUpdate(Player player)
    {
        int newScore = player.Score;                            //새점수

        for (int i = 0; i < rankCount; i++)                        //랭킹 1등부터 5등까지 확인
        {
            if (highScores[i] < newScore)
            {
                for (int j = rankCount - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - i];
                    rankerNames[j] = rankerNames[j - i];
                }
                highScores[i] = newScore;
                rankerNames[i] = "";
                updatedIndex = i;

                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                inputField.transform.position = newPos;
                inputField.gameObject.SetActive(true);
                break;

            }
        }
    }
    void SaveRankingData()
    {

        SaveData saveData = new SaveData();
        saveData.rankerNames = rankerNames;
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
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";

        result = Directory.Exists(path) && File.Exists(fullPath);

        if (result) //폴더와 파일이 있으면,
        {
            string json = File.ReadAllText(fullPath);    //파일읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json); //Save데이터 형식으로 넣어주기
            highScores = loadData.highScores;           //실제로 최고 점 수 넣기
            rankerNames = loadData.rankerNames;         //이름넣기

        }
        else
        {
            //파일에서 못읽었으면 디폴트 값 주기
            int size = rankLines.Length;
            for (int i = 0; i < size; i++)
            {
                int resultScore = 1;
                for (int j = size - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; // 10만, 1만, 천, 백, 십

                char temp = 'A';
                temp = (char)((byte)temp + i);
                rankerNames[i] = $"{temp}{temp}{temp}"; // "AAA", "BBB", "CCC" ...
            }
           
        }
        RefreshRankLines();         //로딩되고 RankLines갱신하기
        return result;
    }
    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

}
