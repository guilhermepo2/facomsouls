using System.Collections;
using UnityEngine;

public class Espada : ArmaStrategy
{
	public Espada(Animator animator) : base("Espada", 40, animator)
	{
		Debug.Log ("Espada Criada!");
		this.getAnimator ().SetBool("hasWeapon", true);
		this.getAnimator ().SetBool("Bow", false);
		this.getAnimator ().SetBool("Sword", true);
		this.getAnimator ().SetBool("TPose", false);
	}
}
