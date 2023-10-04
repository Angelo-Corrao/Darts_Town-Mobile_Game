using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Commons
{

	public const float DPI_2_PCM = 0.393701f;

	public static Vector2 ScaleByDPI(Vector2 pixelDistance)
	{
		float dpi = Screen.dpi;

		if (dpi <= 0.0f)
			dpi = 50.0f;

		return pixelDistance /= dpi * DPI_2_PCM;
	}

	public static bool CheckVectorInRange(Vector2 vector, Vector2 reference, float angleTreshold)
	{
		return Vector2.Dot(reference.normalized, vector.normalized) >= Mathf.Cos(Mathf.Deg2Rad * angleTreshold);
	}
}

