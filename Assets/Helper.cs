using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
	//public static Rect GUI3dRectWithObject(GameObject go)
	//{
	//
	//	Vector3 cen = go.GetComponentInChildren<Renderer>().bounds.center;
	//	Vector3 ext = go.GetComponentInChildren<Renderer>().bounds.extents;
	//	Vector2[] extentPoints = new Vector2[8]
	//	{
	//		WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
	//		WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
	//	};
	//	Vector2 min = extentPoints[0];
	//	Vector2 max = extentPoints[0];
	//	foreach (Vector2 v in extentPoints)
	//	{
	//		min = Vector2.Min(min, v);
	//		max = Vector2.Max(max, v);
	//	}
	//	return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
	//}

	public static Rect GUI2dRectWithObject(GameObject go, Camera cam)
	{
		var vertices = go.GetComponentInChildren<MeshFilter>().mesh.vertices;

		float x1 = float.MaxValue, y1 = float.MaxValue, x2 = 0.0f, y2 = 0.0f;

		foreach (Vector3 vert in vertices)
		{
			Vector2 tmp = WorldToGUIPoint(go.transform.TransformPoint(vert), cam);

			if (tmp.x < x1) x1 = tmp.x;
			if (tmp.x > x2) x2 = tmp.x;
			if (tmp.y < y1) y1 = tmp.y;
			if (tmp.y > y2) y2 = tmp.y;
		}

		Rect bbox = new Rect(x1, y1, x2 - x1, y2 - y1);
		Debug.Log(bbox);
		return bbox;
	}

	public static Vector2 WorldToGUIPoint(Vector3 world, Camera cam)
	{
		Vector2 screenPoint = cam.WorldToScreenPoint(world);
		//Debug.Log("screenPoint: " + screenPoint);
		//Debug.Log("height " + cam.pixelHeight);
		screenPoint.y = cam.pixelHeight - screenPoint.y;
		return screenPoint;
	}
}
