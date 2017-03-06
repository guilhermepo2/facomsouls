using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingScreen : MonoBehaviour
{
	public List<Texture> loadingScreens = new List<Texture>();
	static LoadingScreen instance;
	
	void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			hide();
			return;
		}
		instance = this;    
		gameObject.AddComponent<GUITexture>().enabled = false;
		int choice = (int)Random.Range(0, loadingScreens.Count);
		Debug.Log (choice);
		GetComponent<GUITexture>().texture = loadingScreens[choice];
		transform.position = new Vector3(0.5f, 0.5f, 1f);
		DontDestroyOnLoad(this); 
	}
	
	public static void show()
	{
		if (!InstanceExists()) 
		{
			return;
		}
		
		instance.GetComponent<GUITexture>().enabled = true;
	}
	
	public static void hide()
	{
		if (!InstanceExists())  
		{
			return;
		}
		instance.GetComponent<GUITexture>().enabled = false;
	}
	
	static bool InstanceExists()
	{
		if (!instance)
		{
			return false;
		}
		return true; 
		
	}
	void Update()
	{
		if(!Application.isLoadingLevel)
			hide();
	}
	
}