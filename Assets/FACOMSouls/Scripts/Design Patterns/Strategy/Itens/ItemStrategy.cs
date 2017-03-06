using UnityEngine;
using System.Collections;

public abstract class ItemStrategy {

	private string nome;
	private int quantidade;
	private int id;
	
	protected ItemStrategy(string nome, int quantidade, int id)
	{
		this.nome = nome;
		this.quantidade = quantidade;
		this.id = id;
	}
	
	public void setQuantidade(int quantidade) { this.quantidade = quantidade; }
	public string getNome() {  return this.nome; }
	public int getQuantidade() { return this.quantidade; }
	public int getId() { return this.id; }
	
	public abstract void usarItem();
}

/*--------------------------------------------
		Potion        id: 0
		Chave         id: 1
		StaminaPotin: id: 2
--------------------------------------------*/
