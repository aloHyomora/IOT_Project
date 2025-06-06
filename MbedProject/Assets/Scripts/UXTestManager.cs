using System;
using UnityEngine;
using UnityEngine.Video;

public class UXTestManager : MonoBehaviour
{
    public GameObject testUIGameObject;
    [Header("Test Factor")]
    public UIType uiType;
    public ReactionType reactionType;
    public MessageType messageType;
    
    [Header("Video Component")]
    public VideoPlayer videoPlayer;
    public VideoClip videoClip1;
    public VideoClip videoClip2;
    private void Awake()
    {
        videoPlayer.time = videoPlayer.clip.length -  1f;
        videoPlayer.clip = videoClip1;
        videoPlayer.Play();
    }
    
    void Start()
    {
        // 비디오 재생이 끝나면 OnVideoEnd 함수 실행
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished!");
        videoPlayer.loopPointReached -= OnVideoEnd;
        DoSomethingAfterVideo();
    }

    void DoSomethingAfterVideo()
    {
        testUIGameObject.SetActive(true);
    }

    public void PlayVideo2()
    {
        videoPlayer.clip = videoClip2;
        videoPlayer.Play();
    }

}

public enum UIType
{
    원형슬라이드,
    텍스트,
    모래시계
}

public enum ReactionType
{
    반응없음,
    클릭하면진동,
    클릭하면색변화
}

public enum MessageType
{
    작업중심형메세지,
    감성분산형메세지,
    단순상태형메세지
}