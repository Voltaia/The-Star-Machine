using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
	public float DespawnSeconds;

	[HideInInspector] public Rigidbody Rigidbody;
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
		Rigidbody = GetComponent<Rigidbody>();
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(DespawnSeconds);

		while (true)
		{
			yield return null;
			Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
			Debug.Log(viewportPosition);
			if (
				viewportPosition.x < 0 || viewportPosition.x > 1
				|| viewportPosition.y < 0 || viewportPosition.y > 1
			) Destroy(gameObject);
		}
	}
}