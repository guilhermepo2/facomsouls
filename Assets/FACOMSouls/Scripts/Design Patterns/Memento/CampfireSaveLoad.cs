using UnityEngine;
using System.Collections;

// ---------------------------------------------------------------------------------------------------------------
//					PADRAO MEMENTO - CARE TAKER
// 		A Campfire faz o papel de Save e Load - dando opçoes para criar o memento ou retorna-lo
// 				em outras palavras, a campfire serve como uma interface para acessar o memento pelo jogo
// ---------------------------------------------------------------------------------------------------------------

public class CampfireSaveLoad : MonoBehaviour 
{
	private Memento memento;
	private UserInterface UI;
	private bool UIOpen = false;
	
	void Start()
	{
		//UI = GameObject.Find ("_User Interface").GetComponent<UserInterface>();
		UI = UserInterface.getInstance();
	}
	
	public Memento getMemento()
	{
		return memento;
	}
	
	public void setMemento(Memento valor)
	{
		memento = valor;
	}
	
	public static Attributes loadGameState()
	{
		Vector3 pos = new Vector3(PlayerPrefs.GetFloat("M_posicaoX"),
		                          PlayerPrefs.GetFloat("M_posicaoY"),
		                          PlayerPrefs.GetFloat("M_posicaoZ"));
		
		Attributes estado = new Attributes(pos);
		
		estado.setLevel(					PlayerPrefs.GetInt ("M_level"));
		
		estado.setVidaTotal(				PlayerPrefs.GetInt ("M_vidaTotal"));
		estado.setVidaAtual(				PlayerPrefs.GetInt ("M_vidaAtual"));
		estado.setStaminaTotal( 			PlayerPrefs.GetInt ("M_staminaTotal"));
		estado.setStaminaAtual( 			PlayerPrefs.GetInt ("M_staminaAtual"));
		
		estado.setIdArmaEquipada(			PlayerPrefs.GetInt ("M_idArmaEquipada"));
		estado.setEscudoEquipado(			PlayerPrefs.GetInt ("M_escudoEquipado"));
		estado.setIdItemCambiavel(			PlayerPrefs.GetInt ("M_idItemCambiavel"));
		
		estado.setVitalidade(				PlayerPrefs.GetInt ("M_vitalidade"));
		estado.setEnergia(					PlayerPrefs.GetInt ("M_energia"));
		estado.setForca (					PlayerPrefs.GetInt ("M_forca"));
		
		return estado;
	}
	
	public static void saveGameState(Memento atributoMemento)
	{
		PlayerPrefs.SetInt("M_level",atributoMemento.getAtributos().getLevel());
		
		PlayerPrefs.SetInt 	("M_vidaTotal",atributoMemento.getAtributos().getVidaTotal());
		PlayerPrefs.SetInt 	("M_vidaAtual", atributoMemento.getAtributos().getVidaAtual());
		PlayerPrefs.SetInt 	("M_staminaTotal", atributoMemento.getAtributos().getStaminaTotal());
		PlayerPrefs.SetInt 	("M_staminaAtual", atributoMemento.getAtributos().getStaminaAtual());
		
		
		PlayerPrefs.SetInt 	("M_idArmaEquipada", atributoMemento.getAtributos().getIdArmaEquipada());
		PlayerPrefs.SetInt	("M_escudoEquipado", atributoMemento.getAtributos().getEscudoEquipado());
		PlayerPrefs.SetInt 	("M_idItemCambiavel", atributoMemento.getAtributos().getIdItemCambiavel());
		
		PlayerPrefs.SetFloat ("M_posicaoX", atributoMemento.getAtributos().getPosicaoX());
		PlayerPrefs.SetFloat ("M_posicaoY", atributoMemento.getAtributos().getPosicaoY());
		PlayerPrefs.SetFloat ("M_posicaoZ", atributoMemento.getAtributos().getPosicaoZ());
		
		PlayerPrefs.SetInt 	("M_vitalidade", atributoMemento.getAtributos().getVitalidade());
		PlayerPrefs.SetInt 	("M_energia", atributoMemento.getAtributos().getEnergia());
		PlayerPrefs.SetInt 	("M_forca", atributoMemento.getAtributos().getForca());
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			UI.inspecionar(true);
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			Debug.Log ("PLayer is on campfire :)))");
			if(Input.GetButtonDown ("CONFIRM") && UI.getCampFireOpen() == false)
			{
				UI.inspecionar(false);
				UI.openCampFire(true);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			UI.openCampFire(false);
			UI.inspecionar(false);
			UIOpen = false;
		}
	}
	
	// ---------------------------------------------------------------------------------------------------------------
	// FAZER A INTERFACE ENTRE FOGUEIRA E SAVE/LOAD
	// ---------------------------------------------------------------------------------------------------------------
}
