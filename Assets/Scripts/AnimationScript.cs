using UnityEngine;

public class AnimationScript : MonoBehaviour
{
	[SerializeField] private RaycastingScript raycastingScript;

	[SerializeField] private HUDScript hudScript;

	public Animator animator;

	public bool isAnimationPlaying;

	private float minSpeed = 0f;
	private float maxSpeed = 2.0f;


	private void Update()
	{
		float sliderValue = hudScript.speedSlider.value;
		float animationSpeed = Mathf.Lerp(minSpeed, maxSpeed, sliderValue);

		if(animator != null)
		{
			animator.speed = animationSpeed;
		}
	}

	public void SetAnimator(Animator newAnimator)
	{
		animator = newAnimator;
	}

	public void StopAnimation()
	{
		animator.SetBool("PausePressed", true);
		animator.SetBool(raycastingScript.currentlyHighlightedArea, false);
		isAnimationPlaying = false;
		raycastingScript.currentlyHighlightedArea = "";
	}

	public void StartAnimation()
	{
		animator.SetBool("PausePressed", false);
		animator.SetBool(raycastingScript.currentlyHighlightedArea, true);
		isAnimationPlaying = true;
	}
}
