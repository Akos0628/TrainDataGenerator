using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class GeneratedDataWrapper
{
	public List<GeneratedData> Data = new List<GeneratedData>();

	public string SaveToString()
	{
		return JsonUtility.ToJson(this, true);
	}
}

[System.Serializable]
public class GeneratedData
{
	public string image;
	public string parentCategory;
	public string category;
	public float distance;
	public float[] position;
	public float[] orientation;
}
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.