using UnityEngine;
using System.Collections;

public class Chave : ItemStrategy {
	
	public Chave() : base("chave", 0, 1)
	{
		Debug.Log ("chave criada!");
	}
	
	public override void usarItem()
	{
		Debug.Log ("chave usada! uhul");
	}
}
