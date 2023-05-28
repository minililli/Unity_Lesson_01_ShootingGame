using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// UI에서 표시하는 랭킹 한줄들을 모아둔 배열(0번째가 1등, 4번째가 5등)
    /// </summary>
    RankLine[] rankLines = null;

    /// <summary>
    /// 랭킹별 최고점(0번째가 1등, 4번째가 5등)
    /// </summary>
    int[] highScores = null;

    /// <summary>
    /// 랭킹에 들어간 사람 이름(0번째가 1등, 4번째가 5등)
    /// </summary>
    string[] rankerNames = null;

    /// <summary>
    /// 최대 랭킹 표시 수
    /// </summary>
    int rankCount = 5;

    /// <summary>
    /// 랭킹이 업데이트 되지 않았음을 표시하는 상수
    /// </summary>
    const int NotUpdated = -1;

    /// <summary>
    /// 현재 업데이트 된 랭킹의 인덱스
    /// </summary>
    int updatedIndex = NotUpdated;

    /// <summary>
    /// 인풋 필드 컴포넌트
    /// </summary>
    TMP_InputField inputField;

    private void Awake()
    {
        // 인풋필드 컴포넌트 찾기
        inputField = GetComponentInChildren<TMP_InputField>();  
        // 인풋필드 입력이 끝났을 때 실행될 함수 등록
        inputField.onEndEdit.AddListener(OnNameInputEnd);       
         
        // 랭킹 정보 라인들 가져오기
        rankLines = GetComponentsInChildren<RankLine>();       
        
        rankCount = rankLines.Length;           
        // 순위표 내 rankLines 갯수 만큼 랭커 정보 배열 공간 확보
        highScores = new int[rankCount];        
        rankerNames = new string[rankCount];    
    }

    private void Start()
    {
        inputField.gameObject.SetActive(false);     // 시작할 때 인풋 필드 안보이게 만들기
        Player player = FindObjectOfType<Player>();
        player.onDie += RankUpdate;         // 플레이어가 죽었을 때 랭크 업데이트 함수 호출
        LoadRankingData();                  // 데이터 읽기(파일 없으면 디폴트)

    }

    /// <summary>
    /// 이름 입력이 완료되었을 때 실행되는 함수
    /// </summary>
    /// <param name="text">입력텍스트</param>
    private void OnNameInputEnd(string text)
    {
        // 해당 랭커이름 = text
        rankerNames[updatedIndex] = text; 
        inputField.gameObject.SetActive(false);
        SaveRankingData();  // 새로 저장하고 
        RefreshRankLines(); // UI 갱신
    }

    /// 랭킹 업데이트 하는 함수
    /// </summary>
    /// <param name="player"> 점수를 가지고 있는 플레이어 </param>
    private void RankUpdate(Player player)
    {
           int newScore = player.Score;    // 새 점수

        for (int i = 0; i < rankCount; i++)   // 랭킹 1등부터 5등까지 확인
        {
            if (highScores[i] < newScore)    // 새 점수가 현재 랭킹보다 높으면
            {
                // 현재 랭킹부터 한칸씩 아래로 밀기
                for (int j = rankCount - 1; j > i; j--)           
                {
                    highScores[j] = highScores[j - 1];
                    rankerNames[j] = rankerNames[j - 1];
                }

                //i번째에 player점수 갱신
                highScores[i] = newScore;   
                rankerNames[i] = "";
                updatedIndex = i;
                Vector3 newPos = inputField.transform.position;
                newPos.y = rankLines[i].transform.position.y;
                // 인풋 필드의 위치 조정
                inputField.transform.position = newPos;     
                // 인풋 필드 활성화
                inputField.gameObject.SetActive(true);
                // 인풋 필드 입력 활성화
                inputField.ActivateInputField();
                break;
            }
        }
    }





    void SaveRankingData()
    {
        //SaveData 인스턴스 생성
        SaveData saveData = new SaveData();
        //saveData에 있는 정보와 RankPanel 정보 연결하기
        saveData.rankerNames = rankerNames;
        saveData.highScores = highScores;

        // saveData 정보를 json 양식의 string으로 변경
        string json = JsonUtility.ToJson(saveData);

        // 저장될 경로 구하기({Appliation.dataPath} 유니티에서는 Assets 폴더)
        string path = $"{Application.dataPath}/Save/";

        if (!Directory.Exists(path) )// path에 폴더가 없으면                     
        {
            //path에 디렉터리 만들기
            Directory.CreateDirectory(path);            
        }
        // 전체 경로 = {경로:Assets폴더}파일이름 + 파일확장자
        string fullPath = $"{path}Save.json";
        // fullPath에 .json 파일로 기록하기(쓰기)        
        File.WriteAllText(fullPath, json);              
    }

    /// <summary>
    /// 랭킹데이터 읽기
    /// </summary>
    /// <returns> 읽었는지 여부, true면 파일 읽음, 아니면 디폴트값설정</returns>
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";

        result = Directory.Exists(path) && File.Exists(fullPath);

        if (result) //폴더와 파일이 있으면,
        {
            //Save.json파일 정보 읽기
            string json = File.ReadAllText(fullPath);  
            SaveData loadData = JsonUtility.FromJson<SaveData>(json); 
            //loadData의 정보와 RankPanel의 정보 연결하기
            highScores = loadData.highScores; 
            rankerNames = loadData.rankerNames;        
        }

        else //파일에서 못읽었으면 
        {
            //디폴트 값 주기
            int size = rankLines.Length;
            for (int i = 0; i < size; i++)
            {
                //100000, 10000, 1000, 100, 10
                int resultScore = 1;
                for (int j = size - i; j > 0; j--)
                {
                    resultScore *= 10;
                }
                highScores[i] = resultScore; 

                char temp = 'A';
                temp = (char)((byte)temp + i);
                // "AAA", "BBB", "CCC" ...로 설정하기
                rankerNames[i] = $"{temp}{temp}{temp}"; 
            }
           
        }
        //RankLines갱신하기
        RefreshRankLines(); 
        return result;
    }
    /// <summary>
    /// 랭크 갱신하는 함수
    /// </summary>
    void RefreshRankLines()
    {
        for (int i = 0; i < rankLines.Length; i++)
        {
            rankLines[i].SetData(rankerNames[i], highScores[i]);
        }
    }

}
