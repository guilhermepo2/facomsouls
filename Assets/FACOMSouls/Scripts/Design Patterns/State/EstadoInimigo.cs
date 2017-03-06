using System.Collections;
using UnityEngine;

public abstract class EstadoInimigo
{
	private string estado;
	private GameObject skeletonReference;
	private Animator skeletonAnimator;
	
	protected EstadoInimigo(string estado, GameObject skeletonReference)
	{
		this.estado = estado;
		this.skeletonReference = skeletonReference;
		this.skeletonAnimator = this.skeletonReference.GetComponent<Animator>();
	}
	
	public string getEstado()
	{
		return this.estado;
	}
	
	public Animator getSkeletonAnimator()
	{
		return this.skeletonAnimator;
	}
	
	public GameObject getSkeleton()
	{
		return this.skeletonReference;
	}
	
	public abstract void verificarMudancaEstado(bool perigo, Vector3 position); //false -> perigo diminuiu, true -> perigo aumentou
}
