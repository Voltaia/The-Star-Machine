using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMachine : MonoBehaviour
{
	// Inspector
	public Transform Target;
	public Transform EndEffector;
	public Transform[] Joints;

	private void Update()
	{
		foreach (Transform joint in Joints)
		{
			Vector2 targetVector = Target.position - joint.position;
			Vector2 effectorVector = EndEffector.position - joint.position;
			float angleBetween = Vector3.SignedAngle(targetVector, effectorVector, -Vector3.forward);
			joint.Rotate(Vector3.forward, angleBetween);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(Target.position, 0.25f);

		foreach (Transform joint in Joints)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(joint.position, EndEffector.position);

			Gizmos.color = Color.green;
			Gizmos.DrawLine(joint.position, Target.position);
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
}