using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    TextMeshProUGUI Text;
    private void Awake()
    {
        Transform textTransform = transform.GetChild(2);
        Text = textTransform.GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onLifeChange += updateLife;
    }

    private void updateLife(int life)
    {
        Text.text = life.ToString();
    }

}
