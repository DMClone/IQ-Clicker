using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

// created with the help of https://youtu.be/qc7J0iei3BU?si=VGe6SZ5vnxwL2daQ
public class Timer : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField][ReadOnly] private float timeRemaining;
    private bool timerGoing;

    void Start()
    {
        text.text = "";
    }

    public void BeginTimer()
    {
        timerGoing = true;
        timeRemaining = GameManager.Instance.gameSettings.gameDuration;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            text.text = timeRemaining.ToString("0.00");
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerGoing = false;
                GameManager.Instance.FinishGame();
            }
            yield return null;
        }
    }

    // public string GetTimerString()
    // {
    //     elapsedTime += Time.deltaTime;
    //     _timePlaying = TimeSpan.FromSeconds(elapsedTime);
    //     string timePlayingString;
    //     if (elapsedTime > 60)
    //     {
    //         timePlayingString = _timePlaying.ToString("mm':'s'.'ff");
    //     }
    //     else
    //     {
    //         timePlayingString = _timePlaying.ToString("s'.'ff");
    //     }
    //     return timePlayingString;
    // }
}