using UnityEngine;
using System.Collections;

public class StaminaPotion : ItemStrategy {
	
	public StaminaPotion() : base("STAPotion", 0, 2)
	{
		Debug.Log ("Potion de Stamina criada!");
	}
	
	public override void usarItem()
	{
		Debug.Log ("potion de stamina utilizada! uhul!!");
	}
}
