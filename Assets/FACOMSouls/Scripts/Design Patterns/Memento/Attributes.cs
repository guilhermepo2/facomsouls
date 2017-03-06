using UnityEngine;
using System.Collections;

// ---------------------------------------------------------------------------------------------------------------
//					ESTADO DO PADRAO MEMENTO
//				ORIGINATOR
// ---------------------------------------------------------------------------------------------------------------
public class Attributes
{
	private int level;
	
	private int vidaTotal;
	private int vidaAtual;
	private int staminaTotal;
	private int staminaAtual;
	
	private int idArmaEquipada;
	private int escudoEquipado; // 0 -> nao, 1 -> sim
	private int idItemCambiavel;
	
	private	float posicaoX;
	private	float posicaoY;
	private	float posicaoZ;
	
	private	int vitalidade;
	private	int energia;
	private	int forca;
	
	public int getLevel() { return this.level; }
	public int getVidaTotal() { return this.vidaTotal; }
	public int getVidaAtual() { return this.vidaAtual; }
	public int getStaminaTotal() { return this.staminaTotal; }
	public int getStaminaAtual() { return this.staminaAtual; }
	public int getIdArmaEquipada() { return this.idArmaEquipada; }
	public int getEscudoEquipado() { return this.escudoEquipado; }
	public int getIdItemCambiavel() { return this.idItemCambiavel; }
	
	public float getPosicaoX() { return this.posicaoX; }
	public float getPosicaoY() { return this.posicaoY; }
	public float getPosicaoZ() { return this.posicaoZ; }
	public int getVitalidade() { return this.vitalidade; }
	public int getEnergia() { return this.energia; }
	public int getForca() { return this.forca; }
	
	public void setLevel(int level) { this.level = level; }
	public void setVidaTotal(int vidaTotal) { this.vidaTotal = vidaTotal; }
	
	public void setVidaAtual(int vidaAtual)
	{ 
		if(vidaAtual < 0)
			this.vidaAtual = 0;
		else this.vidaAtual = vidaAtual;
		
		if(this.vidaAtual > this.vidaTotal)
			this.vidaAtual = this.vidaTotal;
	}
	
	public void setStaminaTotal(int staminaTotal) { this.staminaTotal = staminaTotal; }
	public void setStaminaAtual(int staminaAtual) { this.staminaAtual = staminaAtual; }
	public void setIdArmaEquipada(int idArmaEquipada) { this.idArmaEquipada = idArmaEquipada; }
	public void setEscudoEquipado(int escudoEquipado) { this.escudoEquipado = escudoEquipado; }
	public void setIdItemCambiavel(int idItemCambiavel) {this.idItemCambiavel = idItemCambiavel; }
	public void setPosicaoX(float x) { this.posicaoX = x; }
	public void setPosicaoY(float y) { this.posicaoY = y; }
	public void setPosicaoZ(float z) { this.posicaoZ = z; }
	public void setVitalidade(int vitalidade) { this.vitalidade = vitalidade; }
	public void setEnergia(int energia) { this.energia = energia; }
	public void setForca(int forca) { this.forca = forca; }
	
	public Attributes(Vector3 pos)
	{
		this.level = 1;
		
		this.idArmaEquipada = -1;
		this.escudoEquipado = 0;
		this.idItemCambiavel = -1;
		
		this.posicaoX = pos.x;
		this.posicaoY = pos.y;
		this.posicaoZ = pos.z;
		
		this.vitalidade = 5;
		this.energia = 5;
		this.forca = 5;
		this.vidaTotal = vitalidade * 100;
		this.vidaAtual = vitalidade * 100;
		this.staminaTotal = energia * 60;
		this.staminaAtual = energia * 60;
	}
	
	public void recalcularStatus()
	{
		this.vidaTotal = this.vitalidade * 100;
		this.staminaTotal = this.energia * 60;
	}
}
