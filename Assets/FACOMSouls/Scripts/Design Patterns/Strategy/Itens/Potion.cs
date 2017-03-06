using UnityEngine;
using System.Collections;

public class Potion : ItemStrategy {

	private int vidaRecuperada = 100;

	public Potion() : base("Potion", 0, 0)
	{
		Debug.Log ("potion criada!");
	}
	
	public override void usarItem()
	{
		Debug.Log ("Vida Recuperada! Uhul");
		MyThirdCharacter.MainCharacterControl.getInstance ().heal(vidaRecuperada);
	}
}
