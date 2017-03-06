using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

	public GameObject selfReference;
	EnemyAI enAI;
	GameObject character;
	
	EstadoInimigo estado;
	
	void Start () 
	{
		character = GameObject.Find ("maincharacter");
		estado = new EstadoIdle(selfReference);
		enAI = GetComponent<EnemyAI>();
	}
	
	public void setEstado(EstadoInimigo estado)
	{
		if(character.GetComponent<MyThirdCharacter.MainCharacterControl>().getDead ())
		{
			enAI.stop ();
			processarEstado (false, character.transform.position);
		}
		this.estado = estado;
	}
	
	public void processarEstado(bool perigo, Vector3 position)
	{
		estado.verificarMudancaEstado(perigo, position);
	}
	
	public void GetAngry()
	{
		enAI.curTarget = character.transform;
	}
}
