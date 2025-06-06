using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCircularSliderController : MonoBehaviour
{
    public TestUIController testUIController;
    public Slider slider;

    private float duration;
    private float elapsed;
    private bool isRunning;

    private void OnEnable()
    {
        duration = testUIController.duration;
        elapsed = 0f;
        slider.value = 0f;
        isRunning = true;
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
}