using System;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public UXTestManager UXTestManager;
    public TextMeshProUGUI messageText;

    public string taskFocused = "동영상을 불러오고 있습니다. 잠시만 기다려주세요.";
    public string emotionDistracting = "열심히 노력하면 할수록 성과가 나오는 날입니다. \n 책을 붙잡고 매달릴수록 점점 재미가 늘어서 계속 파고들 수 있는 날입니다.";
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