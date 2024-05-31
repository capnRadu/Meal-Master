using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    private float totalTime;
    private float currentTime;

    public void UpdateTimer(float timeSpent, float fullTime)
    {
        totalTime = fullTime;
        currentTime = timeSpent;
        GetComponent<Image>().fillAmount = currentTime / totalTime;
    }
}
