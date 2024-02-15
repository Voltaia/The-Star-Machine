using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMachine : MonoBehaviour
{
	// Inspector

	[Header("Reference Transforms")]
	public Transform GrabPoint;
	public Transform IdleTarget;
	public Transform FurnaceTarget;
	public Transform GrabBarrier;

	[Header("Joints")]
	public Transform[] Joints;

	[Header("Tweaks")]
	public bool MouseControlled;
	public float InteractionRadius;
	public float MovementSpeed;

	// General
	private Star grabbedStar;
	public static Camera MainCamera;

	// Settings
	private const float AngleTolerance = 0.1f;

	private void Awake()
	{
		MainCamera = Camera.main;
	}

	private void Update()
	{
		// Check if grabbed star yet
		if (MouseControlled)
		{
			Vector2 mouseScreenPosition = Input.mousePosition;
			Vector3 mousePosition = new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -MainCamera.transform.position.z);
			Vector3 mouseWorldPosition = MainCamera.ScreenToWorldPoint(mousePosition);
			MoveTowards(mouseWorldPosition);
		}
		else
		{
			if (!grabbedStar) GrabStar();
			else DepositStar();
		}
	}

	private void GrabStar()
	{
		// Get the closest star
		Star closestStar = null;
		float closestStarDistance = Mathf.Infinity;
		foreach (Star star in Star.Instances)
		{
			float distance = Vector3.Distance(GrabPoint.position, star.transform.position);
			if (distance < closestStarDistance)
			{
				closestStarDistance = distance;
				closestStar = star;
			}
		}

		// Check if closest star exists
		bool returnToIdle;
		if (closestStar)
		{
			// Check if star is past barrier
			returnToIdle = closestStar.transform.position.x > GrabBarrier.position.x;

			// If star is past barrier, grab it
			if (!returnToIdle)
			{
				// Check if within grabbing distance and grab star
				if (closestStarDistance < InteractionRadius)
				{
					grabbedStar = closestStar;
					grabbedStar.Grab(GrabPoint);
				}
				else MoveTowards(closestStar.transform.position);
			}
		}
		else returnToIdle = true;

		// Return to idle position
		if (returnToIdle) MoveTowards(IdleTarget.position);
	}

	private void DepositStar()
	{
		float distanceToFurnace = Vector3.Distance(GrabPoint.position, FurnaceTarget.position);

		if (distanceToFurnace < InteractionRadius)
		{
			grabbedStar.Explode();
			grabbedStar = null;
		} else MoveTowards(FurnaceTarget.position);
	}

	private void MoveTowards(Vector3 targetPosition)
	{
		foreach (Transform joint in Joints)
		{
			// Get the angle to move between
			Vector2 targetVector = targetPosition - joint.position;
			Vector2 effectorVector = GrabPoint.position - joint.position;
			float angleBetween = Vector3.SignedAngle(targetVector, effectorVector, -Vector3.forward);

			// Get the amount to actually move by
			if (Mathf.Abs(angleBetween) <= AngleTolerance) continue;
			float moveAngle = Mathf.Sign(angleBetween) * MovementSpeed * Time.deltaTime;

			// Apply rotation
			Quaternion newRotation = joint.transform.localRotation * Quaternion.Euler(0, 0, moveAngle);
			joint.transform.localRotation = newRotation;
		}
	}

	// Keeping this around as an example. This is the most basic form of IK without any animation.
	private void MoveTowardsInstant(Vector3 targetPosition)
	{
		foreach (Transform joint in Joints)
		{
			Vector2 targetVector = targetPosition - joint.position;
			Vector2 effectorVector = GrabPoint.position - joint.position;
			float angleBetween = Vector3.SignedAngle(targetVector, effectorVector, -Vector3.forward);
			joint.Rotate(Vector3.forward, angleBetween);
		}
	}

	// This was unnecessary math but a good way to learn how this works
	private float AngleBetween(Vector2 firstVector, Vector2 secondVector)
	{
		float cosineNumerator = Vector2.Dot(firstVector, secondVector);
		float cosineDenominator = firstVector.magnitude * secondVector.magnitude;
		float cosineQuotient = cosineNumerator / cosineDenominator;
		return Mathf.Acos(cosineQuotient);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(IdleTarget.position, 0.25f);

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(FurnaceTarget.position, 0.25f);

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(GrabPoint.position, 0.25f);
		Gizmos.DrawWireSphere(GrabPoint.position, InteractionRadius);
		Gizmos.DrawLine(GrabBarrier.position + Vector3.down * 25, GrabBarrier.position + Vector3.up * 25);
	}
}