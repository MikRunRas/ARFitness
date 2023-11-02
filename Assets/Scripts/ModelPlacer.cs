using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ModelPlacer : MonoBehaviour
{
	[SerializeField] private ARRaycastManager raycastManager;
	[SerializeField] private GameObject objectToPlace;
	[SerializeField] private AnimationScript animationScript;
	[SerializeField] private Camera mainCamera;

	private GameObject fitnessBuddy;
	private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

	private bool isPinching;
	private bool isRotating; 
	private float initialPinchDistance;
	private Vector3 initialScale;

	public bool isModelPlaced = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isModelPlaced)
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
			{
				var hitpose = hits[0].pose;
				fitnessBuddy = Instantiate(objectToPlace, hitpose.position, Quaternion.Euler(0, 180, 0));
				fitnessBuddy.transform.localScale = Vector3.one * 0.6f;
				animationScript.SetAnimator(fitnessBuddy.GetComponent <Animator>());
				isModelPlaced = true;
			}
		}

		if (isModelPlaced)
		{
			// Rotation
			if (Input.touchCount == 1 && !animationScript.isAnimationPlaying)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					isRotating = true;
				}
				else if (touch.phase == TouchPhase.Moved && isRotating)
				{
					float rotationSpeed = 0.25f;
					fitnessBuddy.transform.Rotate(0, -touch.deltaPosition.x * rotationSpeed, 0);
				}
				else if (touch.phase == TouchPhase.Ended)
				{
					isRotating = false;
				}
			}
			// Zoom
			if (Input.touchCount == 2)
			{
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);

				if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
				{
					isPinching = true;
					initialPinchDistance = Vector2.Distance(touch0.position, touch1.position);
					initialScale = fitnessBuddy.transform.localScale;
				}
				else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
				{
					isPinching = false;
				}

				if (isPinching)
				{
					float currentPinchDistance = Vector2.Distance(touch0.position, touch1.position);
					float scaleFactor = currentPinchDistance / initialPinchDistance;
					fitnessBuddy.transform.localScale = initialScale * scaleFactor;
				}
			}
		}
	}
}
