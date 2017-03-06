using UnityEngine;
using System.Collections;

public class EstadoAlerta : EstadoInimigo {

	public EstadoAlerta(GameObject skeletonReference) : base("alerta", skeletonReference)
	{
		Debug.Log ("Estado Alerta! Get Ready Mate :))))!");
	}
	
	public override void verificarMudancaEstado(bool perigo, Vector3 position)
	{
		if(perigo)
		{
			this.getSkeletonAnimator().SetBool("GetThem", true);
			this.getSkeleton().GetComponent<Skeleton>().setEstado(new EstadoAgressivo(this.getSkeleton ()));
		}
		else
		{
			this.getSkeletonAnimator().SetBool("Danger", false);
			this.getSkeleton().GetComponent<Skeleton>().setEstado(new EstadoIdle(this.getSkeleton ()));
		}
	}
}
