using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    Player player;
    int life;
    TextMeshProUGUI Text;
    private void Awake()
    {
        player = GetComponent<Player>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        player.onLifeChange += updateLife;

        void updateLife(int changedlife)
        {
            changedlife = life;
        }
    }
}
