using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour 
{
	public Texture novojogo;
	public Texture novojogoE;
	public Texture carregarjogo;
	public Texture carregarjogoE;
	public Texture creditos;
	public Texture creditosE;
	public Texture creditosTexture;
	public AudioSource audioClick;
	
	int escolha;
	bool mostrarCreditos = false;
	private bool showMenu;
	
	void Start()
	{
		showMenu = true;
		escolha = 0;
	}
	
	void Update()
	{
		pegarInput();
		if(Input.GetButtonDown ("CONFIRM"))
			processar(escolha);
			
		if(Input.GetButtonDown ("CANCEL") && mostrarCreditos)
			mostrarCreditos = false;
	}
	void OnGUI()
	{
		if(showMenu)
		{
			if(escolha == 0) GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 450, 250, 75), novojogoE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 450, 250, 75), novojogo);
			if(escolha == 1) GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 550, 250, 75), carregarjogoE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 550, 250, 75), carregarjogo);
			if(escolha == 2) GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 650, 250, 75), creditosE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 200, 650, 250, 75), creditos);
		
			if(mostrarCreditos)
			{
				GUI.DrawTexture (new Rect(100, 0, 1012, 734), creditosTexture);
			}
		}
	}
	
	void pegarInput()
	{
		if(Input.GetButtonDown ("DOWN"))
		{
			if(escolha == 2)
				escolha = 0;
			else escolha += 1;
			GameObject.Find ("clickSound").GetComponent<SoundClick>().playClick();
		}
		else if (Input.GetButtonDown("UP"))
		{
			if(escolha == 0)
				escolha = 2;
			else escolha -= 1;
			GameObject.Find ("clickSound").GetComponent<SoundClick>().playClick();
		}
	}
	
	void processar(int escolha)
	{
		GameObject.Find ("choiceSound").GetComponent<SoundChoice>().playChoice();
		if(escolha == 0) // new game
		{
			PlayerPrefs.SetString ("VaiTerLoad","Nao");
			showMenu = false;
			LoadingScreen.show ();
			Application.LoadLevel ("Adventure");
		}
		else if(escolha == 1) // carregar
		{
			PlayerPrefs.SetString ("VaiTerLoad","Sim");
			showMenu = false;
			LoadingScreen.show ();
			Application.LoadLevel ("Adventure");
		}
		else if (escolha == 2) // creditos
		{
			mostrarCreditos = true;
		}
	}
}
