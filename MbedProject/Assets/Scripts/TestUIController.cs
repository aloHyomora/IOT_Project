using System;
using TMPro;
using UnityEngine;

public class TestUIController : MonoBehaviour
{
    public UXTestManager UXTestManager;
    [Header("UI Elements")] 
    public GameObject circleSlider;
    public GameObject text;
    public GameObject sandTimer;
    
    [Header("Message")]
    public GameObject message;

    [Header("Configs")] 
    public float duration;
    
    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        Debug.Log($"Create {UXTestManager.uiType}:{UXTestManager.reactionType}:{UXTestManager.messageType}!");
        GameObject targetGameObject;
        switch (UXTestManager.uiType)
        {
            case UIType.원형슬라이드:
                targetGameObject = circleSlider;
                break;
            case UIType.텍스트:
                targetGameObject = text;
                break;
            case UIType.모래시계:
                targetGameObject = sandTimer;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        targetGameObject.SetActive(true);
        message.SetActive(true);
        
        // duration 뒤에 GameObject 비활성화
        DG.Tweening.DOVirtual.DelayedCall(duration, () =>
        {
            targetGameObject.SetActive(false);
            message.SetActive(false);
            UXTestManager.PlayVideo2();
            gameObject.SetActive(false);
        });
    }
}
