using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Skin : Accessories
{
	public enum DartPart 
	{
		Body,
		Tip,
		Flight
	}
	public enum Tier 
	{
		Tier1,
		Tier2,
		Tier3
	}
	public Material material;
	public DartPart dartPart;
	public int returnedCurrency;

	//hexadecimals to change color of the material with the shader
	public string _PrimaryColor;
	public string _SecondaryColor;
	public string _TertiaryColor;

	public Mesh mesh;
	public Tier tier;
	public string tierStar;
}
