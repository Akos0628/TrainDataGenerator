using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class Coco : MonoBehaviour
{
    public Info info;
    public List<License> licenses = new List<License>();
    public List<Image> images = new List<Image>();
    public List<Annotation> annotations = new List<Annotation>();
    public List<Category> categories = new List<Category>();

	public string SaveToString()
	{
		return JsonUtility.ToJson(this, true);
	}
}

[System.Serializable]
public class Info
{
    public string description = "NB79AX dataset";
    public string url = "";
    public string version = "1.0";
    public int year = DateTimeOffset.Now.Year;
    public string contributor = "";
    public DateTimeOffset date_created = DateTimeOffset.Now;
}

[System.Serializable]
public class License
{
    public string url;
    public int id;
    public string name;
}

[System.Serializable]
public class Image
{
    public int id;
    public int width;
    public int height;
    public string file_name;
    public int? license;
    public string? flickr_url;
	public string? coco_url;
}

[System.Serializable]
public class Annotation
{
    public int id;
    public int image_id;
    public int category_id;
    public int[] bbox;
    public int area;
    public int iscrowd = 0;
}

[System.Serializable]
public class Category
{
    public int id;
    public string name;
    public string supercategory;
}

#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.