using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastingScript : MonoBehaviour
{
	public string currentlyHighlightedArea = "";

	private GameObject currentHitObject;

	public Color newColor;

	[SerializeField] private Camera mainCamera;

	[SerializeField] private GameObject crosshair;

	[SerializeField] private Button playPauseButton;

	[SerializeField] private AnimationScript animationScript;
	void Update()
	{
		Ray ray = mainCamera.ScreenPointToRay(crosshair.transform.position);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit) && !animationScript.isAnimationPlaying)
		{
			// New object is hit.
			GameObject hitObject = hit.collider.gameObject;

			// Check if the currentlyHighlightedArea is different from the new hit object.
			if (currentHitObject != hitObject)
			{
				// Disable the previously highlighted object's mesh renderer and fade it out.
				if (currentHitObject != null)
				{
					StartCoroutine(FadeOutMaterial(currentHitObject.GetComponent<MeshRenderer>()));
				}

				// Set the new object as the currently highlighted object.
				currentHitObject = hitObject;
			}

			// Update the highlight for the newly hit object.
			currentlyHighlightedArea = hitObject.tag + "InCrosshair";
			var hitObjectMeshRenderer = hitObject.GetComponent<MeshRenderer>();
			if(hitObjectMeshRenderer != null)
			{
				hitObjectMeshRenderer.enabled = true;
				StartCoroutine(FadeInMaterial(hitObjectMeshRenderer));
			}
		}
		else
		{
			currentlyHighlightedArea = "";
			if (currentHitObject != null)
			{
				StartCoroutine(FadeOutMaterial(currentHitObject.GetComponent<MeshRenderer>()));
				currentHitObject = null;
			}
		}
	}

	IEnumerator FadeInMaterial(MeshRenderer mesh)
	{
		var material = mesh.material;
		if (material != null)
		{
			Color color = material.color;
			while (color.a < 0.5f)
			{
				color.a += 0.1f;
				material.color = color;
				yield return null;
			}
		}
	}

	IEnumerator FadeOutMaterial(MeshRenderer mesh)
	{
		if(mesh.gameObject.activeSelf)
		{
			float targetAlpha = 0f;
			var material = mesh.material;
			Color color = material.color;
			while (color.a > targetAlpha)
			{
				color.a -= 0.1f;
				material.color = color;
				yield return null;
			}

			if (targetAlpha == 0f)
			{
				mesh.enabled = false;
			}
		}
	}

}
