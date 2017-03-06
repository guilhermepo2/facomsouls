using UnityEngine;
using System.Collections;

public class ExternalCollider : MonoBehaviour {

	public GameObject mcReference;
	private Vector3 position;
	
	void Update()
	{
		position = mcReference.transform.position;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy")
		{
			other.gameObject.GetComponent<Skeleton>().processarEstado(true, position);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Enemy")
		{
			other.gameObject.GetComponent<Skeleton>().processarEstado(false, position);
		}
	}
}
