using UnityEngine;
using System.Collections;

public class SoundChoice : MonoBehaviour 
{	
	public void playChoice()
	{
		GetComponent<AudioSource>().Play();
	}
}
