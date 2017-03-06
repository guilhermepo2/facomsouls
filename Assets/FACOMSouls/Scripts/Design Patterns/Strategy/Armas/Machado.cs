using System.Collections;
using UnityEngine;

public class Machado : ArmaStrategy
{
	public Machado(Animator animator) : base("Machado", 50, animator)
	{
		Debug.Log ("Machado Criado!");
		this.getAnimator ().SetBool("hasWeapon", true);
		this.getAnimator ().SetBool("Bow", false);
		this.getAnimator ().SetBool("Sword", true);
		this.getAnimator ().SetBool("TPose", false);
	}
}
