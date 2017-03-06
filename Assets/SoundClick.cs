using UnityEngine;
using System.Collections;

public class SoundClick : MonoBehaviour 
{	
	public void playClick()
	{
		GetComponent<AudioSource>().Play ();
	}
}
