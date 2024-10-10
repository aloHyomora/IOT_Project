using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    [Header(("Time Limit"))] public float timeLimit;
    private float _remainingTime; // 남은 시간을 초로 저장
    private int lastDisplayedSeconds; // 마지막으로 표시한 초를 저장해서 중복 계산 방지
    
    
    void Start()
    {
        InitializeTimer();
    }

    public void InitializeTimer()
    {
        Debug.Log("Initialize Timer");
        _remainingTime = timeLimit * 60; // 20분을 초로 변환
        UpdateTimerText(); // 처음 텍스트를 업데이트
    }
    void Update()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;

            // 현재 초를 int형으로 표현
            int currentSeconds = Mathf.FloorToInt(_remainingTime);

            // 초 단위에서 변화가 있을 때만 텍스트 업데이트
            if (currentSeconds != lastDisplayedSeconds)
            {
                UpdateTimerText();
                lastDisplayedSeconds = currentSeconds; // 업데이트된 시간을 저장
            }
        }
        else
        {
            _remainingTime = 0;
            UpdateTimerText();
        }
    }

    // 시간을 mm:ss 형식으로 업데이트하는 함수
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
