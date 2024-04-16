using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public TextMeshProUGUI timerCounter;
    private TimeSpan timePlaying;
    private bool timerGoing;
    private float elapsedTime;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timerCounter.text = "00:00.00";
        timerGoing = false;
    }
    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }
    void Update()
    {
        
    }
    public void EndTimer()
    {
        timerGoing = false;
    }
    IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayerStr = timePlaying.ToString("mm':'ss'.'ff");
            timerCounter.text = timePlayerStr;
            yield return null;
        }
    }
}
