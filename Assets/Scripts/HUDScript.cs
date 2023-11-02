using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
	[SerializeField] private ModelPlacer modelPlacer;

	[SerializeField] private RaycastingScript raycastingScript;

	[SerializeField] private AnimationScript animationScript;

	[SerializeField] private GameObject highligtedAreaLine;

	[SerializeField] private TextMeshProUGUI highligtedAreaLineText;

	private TextMeshProUGUI helperText;

	private Button closeButton;

	private Button playPauseButton;

	public Slider speedSlider;


	private void Start()
	{
		Button[] buttons = GetComponentsInChildren<Button>();

		foreach (Button button in buttons)
		{
			if (button.CompareTag("PlayPauseButton"))
			{
				playPauseButton = button;
				playPauseButton.onClick.AddListener(PlayPauseButtonClicked);
			}
			else if (button.CompareTag("CloseButton"))
			{
				closeButton = button;
				closeButton.onClick.AddListener(CloseButtonClicked);
			}
		}

		helperText = GetComponentInChildren<TextMeshProUGUI>();
		speedSlider.value = 0.5f;
	}
	void Update()
	{
		closeButton.interactable = modelPlacer.isModelPlaced;

		if (!animationScript.isAnimationPlaying)
		{
			playPauseButton.interactable = raycastingScript.currentlyHighlightedArea == "" || raycastingScript.currentlyHighlightedArea == "UntaggedInCrosshair" ? false : true;
		}
		else
		{
			playPauseButton.interactable = true;
		}

		highligtedAreaLine.gameObject.SetActive(raycastingScript.currentlyHighlightedArea == "" || raycastingScript.currentlyHighlightedArea == "UntaggedInCrosshair" ? false : true);
		helperText.enabled = raycastingScript.currentlyHighlightedArea == "" || raycastingScript.currentlyHighlightedArea == "UntaggedInCrosshair" ? true : false;

		speedSlider.gameObject.SetActive(animationScript.isAnimationPlaying);

		if (highligtedAreaLine.gameObject.activeSelf)
		{
			highligtedAreaLineText.text = raycastingScript.currentlyHighlightedArea.Substring(0, raycastingScript.currentlyHighlightedArea.Length - 11);
		}
	}

	private void PlayPauseButtonClicked()
	{
		GameObject instantiatedObject = GameObject.Find("FitnessBuddy(Clone)");
		if (instantiatedObject != null && animationScript.isAnimationPlaying)
		{
			animationScript.StopAnimation();
			playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = ">";
			speedSlider.value = 0.5f;
		}
		else if (instantiatedObject != null  && !animationScript.isAnimationPlaying)
		{
			animationScript.StartAnimation();
			playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "⏹";
			speedSlider.value = 0.5f;
		}
	}

	private void CloseButtonClicked()
	{
		GameObject instantiatedObject = GameObject.Find("FitnessBuddy(Clone)");
		if (instantiatedObject != null && modelPlacer != null)
		{
			animationScript.StopAnimation();
			Destroy(instantiatedObject);
			modelPlacer.isModelPlaced = false;
			playPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = ">";
		}
	}
}