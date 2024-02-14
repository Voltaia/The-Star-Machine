using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectCleanup : MonoBehaviour
{
	private VisualEffect visualEffect;

	private void Awake()
	{
		visualEffect = GetComponent<VisualEffect>();
	}

	private void Update()
	{
		if (!visualEffect.HasAnySystemAwake()) Destroy(gameObject);
	}
}
