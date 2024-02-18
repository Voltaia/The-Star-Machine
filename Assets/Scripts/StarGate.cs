using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StarGate : MonoBehaviour
{
	public Transform Center;
	public VisualEffect[] VisualEffects;
	public float ActivationDistance;

	private float activationAmount = 0;

	private void Update()
	{
		// Get closest star
		Star closestStar = null;
		float closestStarDistance = Mathf.Infinity;
		foreach (Star star in Star.Instances)
		{
			float distance = Vector3.Distance(Center.position, star.transform.position);
			if (distance < closestStarDistance)
			{
				closestStarDistance = distance;
				closestStar = star;
			}
		}

		// Change star gate effect alpha based on distance
		if (closestStar != null && closestStarDistance <= ActivationDistance) activationAmount = 1f - (closestStarDistance / ActivationDistance);
		else activationAmount = Mathf.Clamp01(activationAmount - 0.5f * Time.deltaTime);
		SetActivation(activationAmount);
	}

	private void SetActivation(float amount)
	{
		foreach (VisualEffect visualEffects in VisualEffects)
		{
			visualEffects.SetFloat("Activation", amount);
		}
	}
}
