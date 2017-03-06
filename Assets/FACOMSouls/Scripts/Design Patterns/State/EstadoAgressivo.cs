using UnityEngine;
using System.Collections;

public class EstadoAgressivo : EstadoInimigo {
	
	public EstadoAgressivo(GameObject skeletonReference) : base("agressivo", skeletonReference)
	{
		Debug.Log ("Estado Agressivo! RAWR!");
		this.getSkeleton().GetComponent<Skeleton>().GetAngry();
	}
	
	public override void verificarMudancaEstado(bool perigo, Vector3 position)
	{
		if(!perigo)
		{
			this.getSkeletonAnimator().SetBool("GetThem", false);
			this.getSkeleton().transform.LookAt(position);
			this.getSkeleton().GetComponent<Skeleton>().setEstado(new EstadoAlerta(this.getSkeleton ()));
		}
	}
}
