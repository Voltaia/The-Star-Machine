using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSky : MonoBehaviour
{
	public float RotationSpeed;

	private void Update()
	{
		transform.Rotate(-Vector3.forward * RotationSpeed * Time.deltaTime);
	}
}
