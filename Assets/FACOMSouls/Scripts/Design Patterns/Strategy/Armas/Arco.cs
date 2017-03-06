using System.Collections;
using UnityEngine;

public class Arco : ArmaStrategy
{
	public Arco(Animator animator) : base("Arco", 40, animator)
	{
		Debug.Log ("Arco Criado!");
		this.getAnimator ().SetBool("hasWeapon", true);
		this.getAnimator ().SetBool("Bow", true);
		this.getAnimator ().SetBool("Sword", false);
		this.getAnimator ().SetBool("TPose", false);
	}
}
