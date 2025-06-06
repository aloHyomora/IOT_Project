using System;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public UXTestManager UXTestManager;
    public TextMeshProUGUI messageText;

    public string taskFocused = "동영상을 불러오고 있습니다. 잠시만 기다려주세요.";
    public string emotionDistracting = "오늘도 좋은 하루 되세요. 영상 곧 나옵니다!";
    public string plainStatus = "진행 중입니다.";

    private void OnEnable()
    {
        switch (UXTestManager.messageType)
        {
            case MessageType.작업중심형메세지:
                messageText.text = taskFocused;
                break;
            case MessageType.감성분산형메세지:
                messageText.text = emotionDistracting;
                break;
            case MessageType.단순상태형메세지:
                messageText.text = plainStatus;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}