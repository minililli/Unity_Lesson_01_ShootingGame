using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    TextMeshProUGUI lifeText;
    private void Awake()
    {
        Transform textTransform = transform.GetChild(2);
        lifeText = textTransform.GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();         // 플레이어 찾아서
        player.onLifeChange += Refresh;                     //델리게이트에 함수 등록
    }

    private void Refresh(int life)
    {
        lifeText.text = life.ToString();
    }

}
