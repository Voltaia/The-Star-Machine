using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
	[Header("Spawn Settings")]
	public GameObject StarPrefab;
	public float MinSpawnSeconds;
	public float MaxSpawnSeconds;
	public Transform Spawn;
	public Transform SpawnTarget;
	public float SpawnSpread;

	[Header("Physics Settings")]
	public float MinTorque;
	public float MaxTorque;
	public float MinShootForce;
	public float MaxShootForce;

	private IEnumerator Start()
	{
		while (true)
		{
			// Wait for fixed update for physics stuff
			yield return new WaitForFixedUpdate();

			// Spawn star
			Star star = Instantiate(StarPrefab, transform).GetComponent<Star>();
			star.transform.position = Spawn.position;

			// Add torque to star
			float torqueForce = Random.Range(MinTorque, MaxTorque);
			star.Rigidbody.AddTorque(Random.onUnitSphere * torqueForce);

			// Shoot star
			Vector3 targetDirection = (SpawnTarget.position - Spawn.position).normalized;
			float angleOffset = Random.Range(-SpawnSpread, SpawnSpread);
			Vector3 spawnDirection = Quaternion.AngleAxis(angleOffset, Vector3.forward) * targetDirection;
			float shootForce = Random.Range(MinShootForce, MaxShootForce);
			star.Rigidbody.AddForce(spawnDirection * shootForce, ForceMode.Impulse);

			// Wait some time before spawning a new star
			float spawnSeconds = Random.Range(MinSpawnSeconds, MaxSpawnSeconds);
			yield return new WaitForSeconds(spawnSeconds);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(Spawn.position, 0.25f);
		Vector3 targetDirection = (SpawnTarget.position - Spawn.position).normalized;
		Vector3 upperSpread = Quaternion.AngleAxis(-SpawnSpread, Vector3.forward) * targetDirection;
		Vector3 lowerSpread = Quaternion.AngleAxis(SpawnSpread, Vector3.forward) * targetDirection;
		Gizmos.DrawRay(Spawn.position, upperSpread);
		Gizmos.DrawRay(Spawn.position, lowerSpread);

		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(SpawnTarget.position, 0.25f);
	}
}
