using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProgressCircularSliderController : MonoBehaviour, IPointerClickHandler 
{
    public TestUIController testUIController;
    public Slider slider;
    public Image targetImage;
    private Color originalColor;

    private float duration;
    private float elapsed;
    private bool isRunning;

    private void OnEnable()
    {
        duration = testUIController.duration;
        elapsed = 0f;
        slider.value = 0f;
        isRunning = true;
        originalColor = targetImage.color;

    }

    private void Update()
    {
        if (!isRunning) return;

        elapsed += Time.deltaTime;
        slider.value = Mathf.Clamp01(elapsed / duration);

        if (elapsed >= duration)
        {
            slider.value = 1f;
            isRunning = false;
        }
    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        HandleClickReaction();
    }
    
    private void HandleClickReaction()
    {
        switch (testUIController.UXTestManager.reactionType)
        {
            case ReactionType.반응없음:
                break;
            case ReactionType.클릭하면진동:
                transform.DOShakePosition(0.3f, 10f, 20, 90, false, true);
                break;
            case ReactionType.클릭하면색변화:
                if (targetImage != null)
                {
                    targetImage.DOColor(Color.red, 0.2f)
                        .OnComplete(() =>
                            targetImage.DOColor(originalColor, 0.2f));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}