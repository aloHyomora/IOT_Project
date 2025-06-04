using UnityEngine;
using System.Collections.Generic;

public class PoseCommandCollector : MonoBehaviour
{
    public static PoseCommandCollector Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private List<int> poseSequence = new List<int>();

    private Dictionary<int, int> numberMap = new Dictionary<int, int> {
        { 5, 1 }, { 6, 2 }, { 7, 3 }, { 8, 4 }, { 9, 5 }
    };

    private int ledPoseIndex = 0;
    private HashSet<int> requiredTimePoses = new HashSet<int> { 1, 2, 3, 4 };
    private int ledOnIndex = 10;
    private int ledOffIndex = 11;

    private int lastPoseIndex = -1;
    private float lastPoseTime = 0f;
    private float poseCooldown = 1.0f;

    public void OnPoseRecognized(int poseIndex)
    {
        float now = Time.time;
        if (poseIndex == lastPoseIndex && now - lastPoseTime < poseCooldown)
            return;

        lastPoseIndex = poseIndex;
        lastPoseTime = now;


        if (!IsValidNextPose(poseIndex))
        {
            Debug.Log("⛔ 무효한 포즈 입력: " + poseIndex);
            return;
        }
        
        if (requiredTimePoses.Contains(poseIndex) && poseSequence.Contains(poseIndex))
        {
            Debug.Log($"⏭️ 중복된 시간 포즈 무시: {poseIndex}");
            return;
        }

        poseSequence.Add(poseIndex);
        Debug.Log("현재 시퀀스: [" + string.Join(", ", poseSequence) + "]");
        TryBuildCommand();

        if (poseSequence.Count > 10)
            poseSequence.Clear();
    }

    private bool IsValidNextPose(int poseIndex)
    {
        int count = poseSequence.Count;

        if (count == 0)
            return IsNumber(poseIndex); // n

        if (count == 1)
            return poseIndex == ledPoseIndex; // LED

        if (count == 2)
            return requiredTimePoses.Contains(poseIndex) || IsOnOff(poseIndex); // 시간 or ONOFF

        if (count >= 3 && count <= 5)
            return requiredTimePoses.Contains(poseIndex); // 시간 이어짐

        if (count == 6)
            return IsNumber(poseIndex); // k

        if (count == 7)
            return IsOnOff(poseIndex); // ON/OFF

        return false;
    }

    private void TryBuildCommand()
    {
        int count = poseSequence.Count;

        if (count == 8 &&
            IsNumber(poseSequence[0]) &&
            poseSequence[1] == ledPoseIndex &&
            ContainsAllRequiredTimePoses(poseSequence.GetRange(2, 4)) &&
            IsNumber(poseSequence[6]) &&
            IsOnOff(poseSequence[7]))
        {
            int ledNum = numberMap[poseSequence[0]];
            int duration = numberMap[poseSequence[6]];
            string action = GetAction(poseSequence[7]);
            string cmd = $"CMD:LED:{ledNum}:{action}:{duration}";
            Debug.Log("✅ 전송: " + cmd);
            Send(cmd);
            poseSequence.Clear();
            return;
        }

        if (count == 3 &&
            IsNumber(poseSequence[0]) &&
            poseSequence[1] == ledPoseIndex &&
            IsOnOff(poseSequence[2]))
        {
            int ledNum = numberMap[poseSequence[0]];
            string action = GetAction(poseSequence[2]);
            string cmd = $"CMD:LED:{ledNum}:{action}";
            Debug.Log("✅ 전송: " + cmd);
            Send(cmd);
            poseSequence.Clear();
        }
    }

    private bool ContainsAllRequiredTimePoses(List<int> segment)
    {
        HashSet<int> found = new HashSet<int>();
        foreach (int p in segment)
            if (requiredTimePoses.Contains(p))
                found.Add(p);
        return found.SetEquals(requiredTimePoses);
    }

    private bool IsNumber(int index) => numberMap.ContainsKey(index);
    private bool IsOnOff(int index) => index == ledOnIndex || index == ledOffIndex;
    private string GetAction(int index) => (index == ledOnIndex) ? "ON" : "OFF";

    private void Send(string cmd)
    {
        Debug.Log("📤 하드웨어 전송: " + cmd);
        // 시리얼 전송 또는 네트워크 송신 로직
    }
}
