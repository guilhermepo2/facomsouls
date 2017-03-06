using UnityEngine;
using System.Collections;

public class Memento {
	// ---------------------------------------------------------------------------------------------------------------
	//					PADRAO MEMENTO
	//  A Classe memento realizara as funcoes de salvar o estado e retornar um estado salvo
	// ---------------------------------------------------------------------------------------------------------------
	
	Attributes atributos;
	
	// Construtor
	public Memento(Attributes atributos)
	{
		this.atributos = atributos;
	}
	
	public Attributes getAtributos()
	{
		return this.atributos;
	}
}
