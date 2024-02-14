using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
	// Inspector
	public float DespawnSeconds;
	public GameObject Explosion;

	// General
	[NonSerialized] public Rigidbody Rigidbody;
	public static List<Star> Instances = new List<Star>();

	private void Awake()
	{
		Instances.Add(this);
		Rigidbody = GetComponent<Rigidbody>();
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(DespawnSeconds);

		while (true)
		{
			yield return null;
			Vector3 viewportPosition = TheMachine.MainCamera.WorldToViewportPoint(transform.position);
			if (
				viewportPosition.x < -0.5 || viewportPosition.x > 1.5
				|| viewportPosition.y < -0.5 || viewportPosition.y > 1.5
			) Destroy(gameObject);
		}
	}

	public void Grab(Transform parentTransform)
	{
		Rigidbody.isKinematic = true;
		transform.SetParent(parentTransform);
	}

	public void Explode()
	{
		Destroy(gameObject);
		Instantiate(Explosion, transform.position, Quaternion.identity);
	}

	private void OnDestroy()
	{
		Instances.Remove(this);
	}
}