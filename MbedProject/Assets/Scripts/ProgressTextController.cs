using System;
using TMPro;
using UnityEngine;

public class ProgressTextController : MonoBehaviour
{
    public TestUIController testUIController;
    public TextMeshProUGUI progressText;

    private float duration;
    private float elapsed;
    private bool isRunning;

    private void OnEnable()
    {
        duration = testUIController.duration;
        elapsed = 0f;
        isRunning = true;
        progressText.text = "0%";
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
}