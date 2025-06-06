using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProgressTextController : MonoBehaviour, IPointerClickHandler
{
    public TestUIController testUIController;
    public TextMeshProUGUI progressText;
    private Color originalColor;
    private float duration;
    private float elapsed;
    private bool isRunning;

    private void OnEnable()
    {
        duration = testUIController.duration;
        elapsed = 0f;
        isRunning = true;
        progressText.text = "0%";
        originalColor = progressText.color;
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsed += Time.deltaTime;
        float percent = Mathf.Clamp01(elapsed / duration);
        int displayPercent = Mathf.RoundToInt(percent * 100f);
        progressText.text = displayPercent + "%";

        if (elapsed >= duration)
        {
            progressText.text = "100%";
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
                if (progressText != null)
                {
                    progressText.DOColor(Color.red, 0.2f)
                        .OnComplete(() =>
                            progressText.DOColor(originalColor, 0.2f));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}