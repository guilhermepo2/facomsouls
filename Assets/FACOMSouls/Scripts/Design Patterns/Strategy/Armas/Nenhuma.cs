using System.Collections;
using UnityEngine;

public class Nenhuma : ArmaStrategy
{
	public Nenhuma(Animator animator) : base("Nenhuma", 0, animator)
	{
		Debug.Log ("Nenhuma arma sendo utilizada");
		this.getAnimator ().SetBool("hasWeapon", false);
		this.getAnimator ().SetBool("Bow", false);
		this.getAnimator ().SetBool("Sword", false);
		this.getAnimator ().SetBool("TPose", false);
	}
}
