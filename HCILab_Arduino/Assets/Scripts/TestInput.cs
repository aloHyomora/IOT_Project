using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    public string dateTime = "10-04-14-24";
    public float handleValue;
    public float breakValue;
    public float speedValue;
    private void Start()
    {
        DataLoggingManager.Instance.CreateInitialData();
    }
    private void FixedUpdate()
    {
        if(!DataLoggingManager.Instance.isFinished)
            DataLoggingManager.Instance.AddLogData(dateTime, speedValue, breakValue, handleValue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {   
            // 실험이 끝나는 시점에 호출
            DataLoggingManager.Instance.SaveLoggedData();
        }
    }
}
