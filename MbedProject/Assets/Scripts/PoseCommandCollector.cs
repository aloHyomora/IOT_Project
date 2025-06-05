using UnityEngine;
using System.Collections.Generic;

public class PoseCommandCollector : MonoBehaviour
{
    public static PoseCommandCollector Instance { get; private set; }
    public SerialController serialController;

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
            Debug.Log("유효하지 않은 포즈 입력: " + poseIndex);
            return;
        }

        if (requiredTimePoses.Contains(poseIndex) && poseSequence.Contains(poseIndex))
        {
            Debug.Log($"중복된 시간 포즈 무시: {poseIndex}");
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
            return IsNumber(poseIndex); // 숫자 (LED 번호)

        if (count >= 3 && count <= 6){
            if (count == 3 && IsOnOff(poseIndex))
                {return true; // ON/OFF 포즈     
                }
            else{
                return requiredTimePoses.Contains(poseIndex); // 시간 포즈 (중복 없이 4개)
                }

            
        }
            

               

        if (count == 7)
            return IsNumber(poseIndex); // k

        if (count == 8)
            return IsOnOff(poseIndex); // ON/OFF

        return false;
    }

    private void TryBuildCommand()
    {
        int count = poseSequence.Count;

        if (count == 9 &&
            IsNumber(poseSequence[0]) &&
            poseSequence[1] == ledPoseIndex &&
            IsNumber(poseSequence[2]) &&
            ContainsAllRequiredTimePoses(poseSequence.GetRange(3, 4)) &&
            IsNumber(poseSequence[7]) &&
            IsOnOff(poseSequence[8]))
        {
            int group = numberMap[poseSequence[0]];
            int sub = numberMap[poseSequence[2]];
            int ledNum = group * 10 + sub;
            int duration = numberMap[poseSequence[7]];
            string action = GetAction(poseSequence[8]);
            string cmd = $"LED:{ledNum}:{action}:{duration}";
            Debug.Log("✅ 전송: " + cmd);
            Send(cmd);
            poseSequence.Clear();
            return;
        }

        if (count == 4 &&
            IsNumber(poseSequence[0]) &&
            poseSequence[1] == ledPoseIndex &&
            IsNumber(poseSequence[2]) &&
            IsOnOff(poseSequence[3]))
        {
            int group = numberMap[poseSequence[0]];
            int sub = numberMap[poseSequence[2]];
            int ledNum = group * 10 + sub;
            string action = GetAction(poseSequence[3]);
            string cmd = $"LED:{ledNum}:{action}";
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
        Debug.Log("하드웨어 전송: " + cmd);
        serialController.SendSerialMessage(cmd);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Send("LED:11:ON:5000");
        if (Input.GetKeyDown(KeyCode.Alpha2)) Send("LED:11:OFF:3000");
        if (Input.GetKeyDown(KeyCode.Alpha3)) Send("LED:11:ON");
        if (Input.GetKeyDown(KeyCode.Alpha4)) Send("LED:11:OFF");

        if (Input.GetKeyDown(KeyCode.Alpha5)) Send("LED:12:ON:5000");
        if (Input.GetKeyDown(KeyCode.Alpha6)) Send("LED:12:OFF:3000");
        if (Input.GetKeyDown(KeyCode.Alpha7)) Send("LED:12:ON");
        if (Input.GetKeyDown(KeyCode.Alpha8)) Send("LED:12:OFF");

        if (Input.GetKeyDown(KeyCode.Alpha9)) Send("LED:13:ON:5000");
        if (Input.GetKeyDown(KeyCode.Alpha0)) Send("LED:13:OFF:3000");
        if (Input.GetKeyDown(KeyCode.Q)) Send("LED:13:ON");
        if (Input.GetKeyDown(KeyCode.W)) Send("LED:13:OFF");

        if (Input.GetKeyDown(KeyCode.E)) Send("LED:21:ON:5000");
        if (Input.GetKeyDown(KeyCode.R)) Send("LED:21:OFF:3000");
        if (Input.GetKeyDown(KeyCode.T)) Send("LED:21:ON");
        if (Input.GetKeyDown(KeyCode.Y)) Send("LED:21:OFF");

        if (Input.GetKeyDown(KeyCode.U)) Send("LED:22:ON:5000");
        if (Input.GetKeyDown(KeyCode.I)) Send("LED:22:OFF:3000");
        if (Input.GetKeyDown(KeyCode.O)) Send("LED:22:ON");
        if (Input.GetKeyDown(KeyCode.P)) Send("LED:22:OFF");

        if (Input.GetKeyDown(KeyCode.A)) Send("LED:23:ON:5000");
        if (Input.GetKeyDown(KeyCode.S)) Send("LED:23:OFF:3000");
        if (Input.GetKeyDown(KeyCode.D)) Send("LED:23:ON");
        if (Input.GetKeyDown(KeyCode.F)) Send("LED:23:OFF");

        if (Input.GetKeyDown(KeyCode.G)) Send("LED:31:ON:5000");
        if (Input.GetKeyDown(KeyCode.H)) Send("LED:31:OFF:3000");
        if (Input.GetKeyDown(KeyCode.J)) Send("LED:31:ON");
        if (Input.GetKeyDown(KeyCode.K)) Send("LED:31:OFF");

        if (Input.GetKeyDown(KeyCode.L)) Send("LED:32:ON:5000");
        if (Input.GetKeyDown(KeyCode.Z)) Send("LED:32:OFF:3000");
        if (Input.GetKeyDown(KeyCode.X)) Send("LED:32:ON");
        if (Input.GetKeyDown(KeyCode.C)) Send("LED:32:OFF");

        if (Input.GetKeyDown(KeyCode.V)) Send("LED:33:ON:5000");
        if (Input.GetKeyDown(KeyCode.B)) Send("LED:33:OFF:3000");
        if (Input.GetKeyDown(KeyCode.N)) Send("LED:33:ON");
        if (Input.GetKeyDown(KeyCode.M)) Send("LED:33:OFF");

        if (Input.GetKeyDown(KeyCode.Space)) Send("LED:4:ON:5000");
        if (Input.GetKeyDown(KeyCode.Return)) Send("LED:4:OFF:3000");
        if (Input.GetKeyDown(KeyCode.Backspace)) Send("LED:4:ON");
        if (Input.GetKeyDown(KeyCode.Tab)) Send("LED:4:OFF");
    }
}
