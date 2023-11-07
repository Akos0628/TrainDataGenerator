using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PhotoMaker : MonoBehaviour
{
	public GameObject subject;
	public int maxStep;
	public Vector3 subjectPos = new Vector3(0,0,0);
	private Vector3 subjectDirection = new Vector3(1, 0, 0);


	private string defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	private static System.Random rand = new System.Random();

	public readonly GeneratedDataWrapper data = new GeneratedDataWrapper();
	private bool dataSaved = false;
	private int stepCount;
	private string newRunPath;
	private Camera _camera;
	private Camera Camera
	{
		get
		{
			if (!_camera)
			{
				_camera = Camera.main;
			}
			return _camera;
		}
	}

	private Color[] colorSet =
	{
		Color.black,
		Color.white,
		Color.red,
		Color.green,
		Color.blue,
		Color.yellow,
		Color.gray
	};

	// A kamera forgatásának beállítása
	void Start()
	{
		stepCount = 0;
		subject.transform.position = subjectPos;
		
		// A legnagyobb számozású run mappa megtalálása
		string path = defaultDir + Path.DirectorySeparatorChar + "Backgrounds";

		int maxRun = 0;
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		else
		{
			string[] folders = Directory.GetDirectories(path);
			foreach (string folder in folders)
			{
				Debug.Log(folder);
				string runName = folder.Substring(folder.LastIndexOf(Path.DirectorySeparatorChar) + 1);
				Debug.Log(runName);
				if (runName.StartsWith("run"))
				{
					int runNumber = int.Parse(runName.Substring(3));
					if (runNumber > maxRun)
					{
						maxRun = runNumber;
					}
				}
			}
		}

		dataSaved = false;

		// A következõ run mappa létrehozása
		newRunPath = path + Path.DirectorySeparatorChar + "run" + (maxRun + 1);
		Directory.CreateDirectory(newRunPath);
		Debug.Log(newRunPath);
	}

	// A kamera forgatása és a képkocka elmentése
	void Update()
	{
		if(stepCount < maxStep)
		{
			stepCount++;
			float[] posOrNeg = { 1, -1 };
			Camera.transform.position = new Vector3(
				getRandomIn(4,15, posOrNeg),
				getRandomIn(-0.5f,10),
				getRandomIn(3,15, posOrNeg)

			);

			var carRandomizedLocation = subject.transform.position + new Vector3(
				getRandomIn(0, 4, posOrNeg),
				getRandomIn(0, 2),
				getRandomIn(0, 3, posOrNeg)
			);
			Camera.transform.LookAt(carRandomizedLocation);

			subject.GetComponentInChildren<MeshRenderer>().materials[0].color = colorSet[rand.Next(colorSet.Length)];

			var imgName = "image" + stepCount + ".png";
			StoreData(imgName);
			Capture(imgName);
		} 
		else
		{
			if (!dataSaved)
			{
				//string cocoString = data.SaveToString();
				string jsonData = JsonUtility.ToJson(data, true);

				Debug.Log("Saving formatted data");
				File.WriteAllText(newRunPath + Path.DirectorySeparatorChar + "data.json", jsonData);
				Debug.Log(jsonData);
				dataSaved = true;
			}
		}
	}

	private void StoreData(string imageName)
	{
		var subjectRelativePos = Camera.transform.InverseTransformDirection(subject.transform.position - Camera.transform.position);
		var subjectRelativeDir = Camera.transform.InverseTransformDirection(subjectDirection - Camera.transform.position);

		var carDir = subjectRelativeDir - subjectRelativePos;


		data.Data.Add(
			new GeneratedData()
			{
				image = imageName,
				parentCategory = "vehicle",
				category = "car",
				distance = subjectRelativePos.magnitude,
				position = convertToArray(subjectRelativePos),
				orientation = convertToArray(carDir),
			});
	}

	public void Capture(string imageName)
	{
		RenderTexture activeRenderTexture = RenderTexture.active;
		RenderTexture.active = Camera.targetTexture;

		Camera.Render();

		Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
		image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
		image.Apply();
		RenderTexture.active = activeRenderTexture;

		byte[] bytes = image.EncodeToPNG();
		Destroy(image);

		File.WriteAllBytes(newRunPath + Path.DirectorySeparatorChar + imageName, bytes);

		//coco.images.Add(new Image()
		//{
		//	id = stepCount,
		//	width = Camera.targetTexture.width,
		//	height = Camera.targetTexture.height,
		//	file_name = "image" + stepCount + ".png",
		//
		//});
		//
		//// A tárgy bounding boxa
		//Rect bounds = Helper.GUI2dRectWithObject(subject, Camera);
		//coco.annotations.Add(new Annotation()
		//{
		//	id = stepCount,
		//	image_id = stepCount,
		//	category_id = 1,
		//	bbox = new int[4] { (int)bounds.x, (int)bounds.y, (int)bounds.width,(int)bounds.height },
		//	area = (int)bounds.width*(int)bounds.height,
		//	iscrowd = 0,
		//});
	}
	private float[] convertToArray(Vector3 vec)
	{
		return new float[] { vec.x, vec.y, vec.z };
	}

	private float getRandomIn(float from, float to, float[] posOrNeg)
	{
		return UnityEngine.Random.Range(from, to) * posOrNeg[rand.Next(posOrNeg.Length)];
	}
	
	private float getRandomIn(float from, float to)
	{
		return UnityEngine.Random.Range(from, to);
	}
}
