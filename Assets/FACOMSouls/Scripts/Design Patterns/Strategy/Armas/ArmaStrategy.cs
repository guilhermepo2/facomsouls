using System.Collections;
using UnityEngine;

public abstract class ArmaStrategy 
{
	private string tipoDeArma;
	private int dano;
	private Animator animator;
	
	protected ArmaStrategy(string tipoDeArma, int dano, Animator animator)
	{
		this.tipoDeArma = tipoDeArma;
		this.dano = dano;
		this.animator = animator;
	}
	
	public int getDano()
	{
		return this.dano;
	}
	
	public string getTipoDeArma()
	{
		return this.tipoDeArma;
	}
	
	public Animator getAnimator()
	{
		return this.animator;
	}
}
