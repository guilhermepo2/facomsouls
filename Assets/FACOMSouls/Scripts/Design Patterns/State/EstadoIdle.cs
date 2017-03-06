using UnityEngine;
using System.Collections;

public class EstadoIdle : EstadoInimigo
{
	public EstadoIdle(GameObject skeletonReference) : base("idle", skeletonReference)
	{
		Debug.Log ("Estado Idle! Leave me Alone :(((((!");
	}
	
	public override void verificarMudancaEstado(bool perigo, Vector3 position)
	{
		if(perigo)
		{
			this.getSkeletonAnimator().SetBool("Danger", true);
			this.getSkeleton().transform.LookAt(position);
			this.getSkeleton().GetComponent<Skeleton>().setEstado(new EstadoAlerta(this.getSkeleton ()));
		}
	}
}
