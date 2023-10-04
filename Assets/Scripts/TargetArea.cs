using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArea : MonoBehaviour
{
    public enum DartboardSection: int
    {
        Bullseye,
        Base,
        Double,
        Triple
    }

    [SerializeField]
    private int sectionScore;

    [HideInInspector]
    public int score;

    public DartboardSection section;

	public void Awake()
	{
        if ((int)section != 0)
            score = sectionScore * (int)section;
        else
            score = sectionScore;
	}
}
