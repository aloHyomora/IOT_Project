using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadManager : MonoBehaviour
{
    [SerializeField] public GameObject[] lights; // 0 : red, 1 : yellow, 2 : blue
    [SerializeField] private CrossWalkManager[] crosswalks; // 
    private void Awake()
    {
        InitializeLoad();
    }

    public void InitializeLoad()
    {
        // TODO : 교차로에 접근한 경우(콜라이더 충돌 시)에 이 메서드 호출하기
        // 운전자가 근처에 왔을 때 교차로 상황을 초기화하고 진행합니다.
        Debug.Log("교차로 상황 초기화, 시작 (20초 후 초록불 신호 켜짐)");
        StartCoroutine(CrossroadRoutine());
    }

    private void TurnLight(int index)
    {
        foreach (var light in lights)
        {
            light.SetActive(false);
        }
        lights[index].SetActive(true);
    }

    private IEnumerator CrossroadRoutine()
    {
        yield return StartCoroutine(Phase1());
        yield return StartCoroutine(Phase2());
    }
    private IEnumerator Phase1()
    {
        // 운전자가 사거리에 도착한 직후, 횡단보도로 사람들이 걸어다닌다.
        crosswalks[0].TriggerPhase1Animation();
        
        // 20초 대기
        yield return new WaitForSeconds(20f);
    }
    private IEnumerator Phase2()
    {
        // 운전자가 사거리에 도착한 직후, 횡단보도로 사람들이 걸어다닌다.
        TurnLight(2);
        crosswalks[0].TriggerPhase2Animation();

        yield return null;
    }
}


