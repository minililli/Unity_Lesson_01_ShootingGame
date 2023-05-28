using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossAlertPanel : MonoBehaviour
{
    
    TimeManager timer;
    CanvasGroup canvasgroup;
    private void Awake()
    {
        timer= FindObjectOfType<TimeManager>();
        canvasgroup= GetComponent<CanvasGroup>();
        canvasgroup.alpha = 0.0f;

    }

    private void Start()
    {
        timer.BossTime += () => StartCoroutine(Alert());
    }

    IEnumerator Alert()
    {
        canvasgroup.alpha = 0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 1.0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 1.0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 1.0f;
        yield return new WaitForSeconds(0.5f);
        canvasgroup.alpha = 0f;
        StopCoroutine(Alert());
    }
}
