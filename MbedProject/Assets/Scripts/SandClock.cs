using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SandClock : MonoBehaviour, IPointerClickHandler
{
	public TestUIController testUIController;
	[SerializeField] Image fillTopImage;
	[SerializeField] Image fillBottomImage;
	[SerializeField] Text roundText;
	[SerializeField] Image sandDotsImage;
	[SerializeField] Image BottomMaskImage;
	[SerializeField] RectTransform sandPyramidRect;
	Color originalColor;
	//Events
	[HideInInspector] public UnityAction onAllRoundsCompleted;
	[HideInInspector] public UnityAction<int> onRoundStart;
	[HideInInspector] public UnityAction<int> onRoundEnd;

	[Space (30f)]
	public float roundDuration = 10f;
	public int totalRounds = 3;

	float defaultSandPyramidYPos;
	int currentRound = 0;

	void OnEnable ()
	{
		roundDuration = testUIController.duration;
		SetRoundText (totalRounds);
		defaultSandPyramidYPos = sandPyramidRect.anchoredPosition.y;
		sandDotsImage.DOFade (0f, 0f);
		originalColor = fillTopImage.color;
		Begin();
	}

	public void Begin ()
	{
		++currentRound;

		//start event
		if (onRoundStart != null)
			onRoundStart.Invoke (currentRound);


		sandDotsImage.DOFade (1f, .8f);
		sandDotsImage.material.DOOffset (Vector2.down * -roundDuration, roundDuration).From (Vector2.zero).SetEase (Ease.Linear);

		//Scale Pyramid
		sandPyramidRect.DOScaleY (1f, roundDuration / 3f).From (0f);
		sandPyramidRect.DOScaleY (0f, roundDuration / 1.5f).SetDelay (roundDuration / 3f).SetEase (Ease.Linear);

		sandPyramidRect.anchoredPosition = Vector2.up * defaultSandPyramidYPos;
		sandPyramidRect.DOAnchorPosY (0f, roundDuration).SetEase (Ease.Linear);

		ResetClock ();

		roundText.DOFade (1f, .8f);

		fillTopImage
			.DOFillAmount (0, roundDuration)
			.SetEase (Ease.Linear)
			.OnUpdate (OnTimeUpdate)
			.OnComplete (OnRoundTimeComplete);
	}

	void OnTimeUpdate ()
	{
		fillBottomImage.fillAmount = 1f - fillTopImage.fillAmount;
	}

	void OnRoundTimeComplete ()
	{
		// TODO: 다음 영상 재생 코드 추가
		
		//round end event
		if (onRoundEnd != null)
			onRoundEnd.Invoke (currentRound);

		sandDotsImage.DOFade (0f, 0f);

		if (currentRound < totalRounds) {
			//there is more rounds
			roundText.DOFade (0f, 0f);

			transform
			.DORotate (Vector3.forward * 180f, .8f, RotateMode.FastBeyond360)
			.From (Vector3.zero)
			.SetEase (Ease.InOutBack)
			.OnComplete (() => {
				SetRoundText (totalRounds - currentRound);
				Begin ();
			});
		} else {
			//finished all rounds

			//all rounds completed event
			if (onAllRoundsCompleted != null)
				onAllRoundsCompleted.Invoke ();

			SetRoundText (0);
			transform.DOShakeScale (.8f, .3f, 10, 90f, true);
		}
	}


	void SetRoundText (int value)
	{
		roundText.text = value.ToString ();
	}

	public void ResetClock ()
	{
		transform.rotation = Quaternion.Euler (Vector3.zero);
		fillTopImage.fillAmount = 1f;
		fillBottomImage.fillAmount = 0f;
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
				transform.DOShakePosition(0.3f, 20f, 20, 90, false, true);
				break;
			case ReactionType.클릭하면색변화:
				if (fillTopImage != null)
				{
					fillTopImage.DOColor(Color.red, 0.2f)
						.OnComplete(() =>
							fillTopImage.DOColor(originalColor, 0.2f));
				}
				if (fillBottomImage != null)
				{
					fillBottomImage.DOColor(Color.red, 0.2f)
						.OnComplete(() =>
							fillBottomImage.DOColor(originalColor, 0.2f));
				}
				if (sandDotsImage != null)
				{
					sandDotsImage.DOColor(Color.red, 0.2f)
						.OnComplete(() =>
							sandDotsImage.DOColor(originalColor, 0.2f));
				}
				if (BottomMaskImage != null)
				{
					BottomMaskImage.DOColor(Color.red, 0.2f)
						.OnComplete(() =>
							BottomMaskImage.DOColor(originalColor, 0.2f));
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
