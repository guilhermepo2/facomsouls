using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour 
{
	/*-------------------------------------------------------
		SINGLETON
	-------------------------------------------------------*/
	
	public static UserInterface instance = null;
	
	public static UserInterface getInstance()
	{
		return instance;
	}
	
	
	/*--------------------------------------------------------
		Variaveis privadas para utilizaçao da Interface
	--------------------------------------------------------*/
	private bool paused;												// jogo pausado
	private bool mainMenuOpened;                                        // menu principal aberto
	private bool mainMenuChoice;                                        // o jogador esta escolhendo no menu principal
	private int mainMenuIndex = 0; 	                                    // index para identificar a escolha do jogador no menu
	private bool weaponMenu;                                            // o jogador esta escolhendo no menu de armas
	private int weaponMenuIndex;	                                    // index para identificar a escolha do jogador no menu de armas
	private MyThirdCharacter.MainCharacterControl characterReference;   // referencia ao script do personagem principal
	
	/*--------------------------------------------------------
		Texturas que serao renderizadas na tela
	--------------------------------------------------------*/
	[Header("Texturas Vida e Stamina")]
	public Texture blackTexture;										// textura preta
	public Texture greenTexture;										// textura verde
	public Texture yellowTexture;										// textura amarela
	
	[Header("Texturas para Tela")]
	public Texture pauseGameTexture;									// textura renderizada ao pausar o jogo
	public Texture inspectTexture;										// textura da imagem de inspecionar
	public Texture naoDisponivel;
	private bool b_naoDisponivel;
	
	[Header("Botoes")]
	public Texture salvarJogo;
	public Texture salvarJogoE;
	public Texture carregarJogo;
	public Texture carregarJogoE;
	public Texture uparLevel;
	public Texture uparLevelE;
	
	[Header("Menu de Armas")]
	public Texture background;
	public Texture equiparEspada;
	public Texture equiparEspadaE;
	public Texture equiparEscudo;
	public Texture equiparEscudoE;
	public Texture equiparMachado;
	public Texture equiparMachadoE;
	public Texture equiparArco;
	public Texture equiparArcoE;
	
	[Header("Texturas para Equipamentos")]
	public Texture boxTexture;											// textura das caixas da interface
	public Texture axeTexture;											// textura no machado mostrado nos equipamentos
	public Texture bowTexture;											// textura do arco mostrado nos equipamentos
	public Texture shieldTexture;										// textura do escudo mostrado nos equipamentos
	public Texture swordTexture;										// textura da espada mostrada nos equipamentos
	public Texture potionTexture;										// textura da pocao mostrada nos equipamentos
	
	
	
	/*--------------------------------------------------------
		Booleanas para saber qual arma esta sendo renderizada
	--------------------------------------------------------*/
	private bool sword = false;											// espada sendo renderizada
	private bool bow = false;											// arco sendo renderizado
	private bool axe = false;											// machado sendo renderizado
	private bool shield = false;										// escudo sendo renderizado
	
	private bool inspect = false;
	
	/*--------------------------------------------------------
		Campfire e Padrao Memento
	--------------------------------------------------------*/
	private bool campFireMenu;
	private int campFireIndex;
	private Memento atributoMemento;
	
	/*--------------------------------------------------------
		RENDER DEATH
	--------------------------------------------------------*/
	public Texture UDIED;
	private bool dead = false;
	private int drawDepth = -1000;
	private float fadeSpeed = 0.1f;
	private float alpha = 0.0f;
	private float fadeDir = 1f;
	
	
	void Awake()
	{
		/*-------------------------------------------------------
			Aplicaçao Singleton
		-------------------------------------------------------*/
			if(instance == null)											// se a instancia for nula
				instance = this;											// a instancia passa a ser uma referencia desse objeto
			else if (instance != this)                                      // caso a instancia nao seja nula e nao seja esse objeto
				Destroy (gameObject);                                     	// destroi esse objeto
			DontDestroyOnLoad(gameObject);                                  // caso o objeto nao seja destruido, ou seja, ele e a instancia
																		// 			ele nao pode ser destruido
		/* --------------------------------------------------------*/
	}
	/*-----------------------------------------------------------
		A funcao e executada uma unica vez no inicio da execucao
	------------------------------------------------------------*/
	private void Start()
	{	
		campFireMenu = false;
		characterReference = GameObject.Find ("maincharacter").GetComponent<MyThirdCharacter.MainCharacterControl>();
	}
	
	public IEnumerator Restart()
	{
		yield return new WaitForSeconds(5.0f);
		LoadingScreen.show ();
		dead = false;
		Destroy (gameObject);
		Destroy (GameObject.Find ("maincharacter"));
		Destroy (GameObject.Find ("_CombatMediator"));
		Application.LoadLevel ("Intro");
	}
	
	/*--------------------------------------------------------
		A Funcao update e executada a cada frame
	--------------------------------------------------------*/
	private void Update()
	{	
		//atributos = characterReference.getMementoState();
		atributoMemento = characterReference.criarMemento();
		
		if(mainMenuOpened)																	// SE O MENU PRINCIPAL ESTIVER ABERTO
		{
			if(mainMenuChoice)																// se a escolha for feita no menu principal
				getMenuInput ();															// pega input do menu principal
			//else if(weaponMenu) getWeaponMenuInput();										// senao, se a escolha for nas armas, pega input nesse menu
																																						
			if(Input.GetButtonDown ("CONFIRM"))												// se o botao de confirmar for apertado
				processSubMenu(mainMenuIndex);												// processa a escolha do jogador
			
			if(Input.GetButtonDown ("CANCEL"))												// se o botao de cancelar for apertado
				processCancel(mainMenuIndex);												// processa o menu cancelado
		}
		
		if(campFireMenu)
		{																					// menu da fogueira aberto														
			getCampFireMenuInput();
			
			if(Input.GetButtonDown ("CONFIRM"))												// se o botao de confirmar for apertado
				processCampfireMenu(campFireIndex);											// processa a escolha do jogador
			
			if(Input.GetButtonDown ("CANCEL"))												// se o botao de cancelar for apertado
				processCampfireCancel();
		}
	}
	
	/*--------------------------------------------------------
		Funcao que mostrara os objetos na tela
	--------------------------------------------------------*/
	void OnGUI()
	{
		/*--------------------------------------------------------
				Renderiza a vida e a stamina
		--------------------------------------------------------*/
		
		GUI.Label(new Rect(10, 12, 50, 50), "HP: ");										// Renderiza o escrito HP:
		GUI.DrawTexture (new Rect(40, 15, atributoMemento.getAtributos ().getVidaTotal() + 2, 15), blackTexture); 							// barra dos fundos da vida
		GUI.DrawTexture (new Rect(41, 17.5f, atributoMemento.getAtributos ().getVidaAtual (), 10), greenTexture); 						// barra de vida por cima
		GUI.Label(new Rect(10,32, 50, 50), "ST: ");											// Renderiza o escrito ST:
		GUI.DrawTexture (new Rect(40, 35, atributoMemento.getAtributos ().getStaminaTotal() + 2, 15), blackTexture);							// barra dos fundos da stamina
		GUI.DrawTexture (new Rect(41, 37.5f, atributoMemento.getAtributos ().getStaminaAtual(), 10), yellowTexture);						// barra de stamina por cima
		
		/*--------------------------------------------------------
				Renderiza as Souls
		--------------------------------------------------------*/
		GUI.Label (new Rect(Screen.width - 75, Screen.height - 35, 100, 50), "SOULS");      // Renderiza as almas no canto inferior da tela
		
		/*--------------------------------------------------------
			Renderiza as caixas que mostram os itens equipados
		--------------------------------------------------------*/
		
		if(bow) GUI.DrawTexture(new Rect(25,Screen.height - 185, 100,75), bowTexture);		// arco na caixa esquerda
		if(shield) GUI.DrawTexture(new Rect(10,Screen.height - 190, 150,80), shieldTexture);// escudo na caixa esquerda
		GUI.DrawTexture (new Rect(25,Screen.height - 200,100,100), boxTexture);				// caixa do lado esquerdo
		
		
		GUI.Label (new Rect(195,Screen.height - 205,150,125), "qtd");						// quantidae de pocoes na caixa de cima
		GUI.DrawTexture (new Rect(110,Screen.height - 300,150,125), potionTexture);			// textura da pocao na caixa de cima
		GUI.DrawTexture (new Rect(125,Screen.height - 300,100,125), boxTexture);			// caixa de cima
		
		if(axe) GUI.DrawTexture(new Rect(205,Screen.height - 185, 150,75), axeTexture);		// machado na caixa direita
		if(sword) GUI.DrawTexture(new Rect(200,Screen.height - 185, 150,75), swordTexture); // espada na caixa direita
		if(bow) GUI.DrawTexture(new Rect(225,Screen.height - 185, 100,75), bowTexture);     // arco na caixa direita
		GUI.DrawTexture (new Rect(225,Screen.height - 200,100,100), boxTexture);			// caixa do lado direito
		
		
		
		GUI.DrawTexture (new Rect(125,Screen.height - 150,100,125), boxTexture);			// caixa de baixo
		
		if(paused)																			//SE O JOGO ESTIVER PAUSADO
		{																					//
			GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height), pauseGameTexture);	// renderiza a textura de jogo pausado
		}																					//
		
		/*
		if(mainMenuOpened)																	// SE O MENU PRINCIPAL ESTIVER ABERTO
		{																					//
			if(mainMenuIndex == 0) GUI.Label (new Rect(90,100,10,50), ">");					//
			GUI.Label (new Rect(100,100, 100,50), "Equipar Armas");							//
			if(mainMenuIndex == 1) GUI.Label (new Rect(90,110,10,50), ">");					//
			GUI.Label (new Rect(100,110, 100,50), "Opçao 2");								//
			if(mainMenuIndex == 2) GUI.Label (new Rect(90,120,10,50), ">");					//
			GUI.Label (new Rect(100,120, 100,50), "Opçao 3");								//
			if(mainMenuIndex == 3) GUI.Label (new Rect(90,130,10,50), ">");					//
			GUI.Label (new Rect(100,130, 100,50), "Opçao 4");								//
		} MENU PRINCIPAL VIROU APENAS O MENU DE ARMAS */ 
		
		if(mainMenuOpened)																	// SE O MENU DE ARMAS ESTIVER ABERTO
		{			
			GUI.DrawTexture (new Rect(400,200,500,500), background);
			if(mainMenuIndex == 0) GUI.DrawTexture (new Rect(500, 225, 250,75), equiparEspadaE);
			else GUI.DrawTexture (new Rect(500, 225, 250,75), equiparEspada);	
			
			if(mainMenuIndex == 1) GUI.DrawTexture (new Rect(500, 325, 250,75), equiparEscudoE);
			else GUI.DrawTexture (new Rect(500, 325, 250,75), equiparEscudo);
			
			if(mainMenuIndex == 2) GUI.DrawTexture (new Rect(500, 425, 250,75), equiparMachadoE);
			else GUI.DrawTexture (new Rect(500, 425, 250,75), equiparMachado);
			
			if(mainMenuIndex == 3) GUI.DrawTexture (new Rect(500, 525, 250,75), equiparArcoE);
			else GUI.DrawTexture (new Rect(500, 525, 250,75), equiparArco);

		}
		
		if(campFireMenu)
		{
			if(campFireIndex == 0) GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 450, 250, 75), salvarJogoE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 450, 250, 75), salvarJogo);
			if(campFireIndex == 1) GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 550, 250, 75), carregarJogoE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 550, 250, 75), carregarJogo);
			if(campFireIndex == 2) GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 650, 250, 75), uparLevelE);
			else GUI.DrawTexture (new Rect((Screen.width / 2) - 150, 650, 250, 75), uparLevel);	
		}
		
		if(b_naoDisponivel)
			GUI.DrawTexture (new Rect((Screen.width / 2) - 100, Screen.height / 2, 250, 75), naoDisponivel);
		
		if(inspect) GUI.DrawTexture(new Rect(500, Screen.height - 100, 250, 75), inspectTexture);
		
		if(dead)
		{
			alpha += fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01 (alpha);
			Color temp = GUI.color;
			temp.a = alpha;
			GUI.color = temp;
			GUI.depth = drawDepth;
			GUI.DrawTexture (new Rect(0,0, Screen.width, Screen.height), blackTexture);
			
			if(GUI.color.a >= 0.75)
			{
				GUI.DrawTexture (new Rect(350, (Screen.height / 2) - 200, 600, 200), UDIED);
				StartCoroutine ("Restart"); 
			}
				
		}
		
	}
	
	/*--------------------------------------------------------
		Processa as entradas escolhidas pelo usuario
	--------------------------------------------------------*/	
	private void processSubMenu(int mainIndex)
	{
		switch(mainIndex)
		{
			case 0:
				characterReference.equipWeapon ("sword");
				sword = true;
				axe = false;
				bow = false;
				openMainMenu (false);
				break;
			case 1:
				characterReference.equipWeapon ("shield");
				bow = false;
				shield = true;
				openMainMenu (false);
				break;
			case 2:
				characterReference.equipWeapon ("axe");
				sword = false;
				axe = true;
				bow = false;
				openMainMenu (false);
				break;
			case 3:
				characterReference.equipWeapon ("bow");
				bow = true;
				shield = false;
				axe = false;
				sword = false;
				openMainMenu (false);
				break;
		}
				
	}
	
	/*--------------------------------------------------------
		Processa o ato de cancelamento em um menu
	--------------------------------------------------------*/	
	private void processCancel(int mainIndex)
	{
		mainMenuOpened = false;
		mainMenuChoice = false;
	}
	
	/*----------------------------------------------------------
		Pega o input do jogador no menu de equipamentos
	----------------------------------------------------------*/
	private void getWeaponMenuInput()
	{
		if(Input.GetButtonDown ("DOWN"))
		{
			if(weaponMenuIndex == 3)
				weaponMenuIndex = 0;
			else weaponMenuIndex += 1;
		}
		else if(Input.GetButtonDown ("UP"))
		{
			if(weaponMenuIndex == 0)
				weaponMenuIndex = 3;
			else weaponMenuIndex -= 1;
		}
	}
	
	/*--------------------------------------------------------
		Pega o input do jogador no menu principal
	--------------------------------------------------------*/
	private void getMenuInput()
	{
		if(Input.GetButtonDown ("DOWN"))
		{
			if(mainMenuIndex == 3)
				mainMenuIndex = 0;
			else mainMenuIndex += 1;
		}
		else if(Input.GetButtonDown ("UP"))
		{
			if(mainMenuIndex == 0)
				mainMenuIndex = 3;
			else mainMenuIndex -= 1;
		}
	}
	
	/*--------------------------------------------------------
		Pausa ou despausa o jogo
	--------------------------------------------------------*/
	public void gamePaused(bool paused)
	{
		this.paused = paused;
	}
	
	/*--------------------------------------------------------
		Abre ou Fecha o menu principal
	--------------------------------------------------------*/
	public void openMainMenu(bool open)
	{
		if(open)
		{
			this.mainMenuOpened = true;
			this.mainMenuChoice = true;
			this.mainMenuIndex = 0;
		}
		else {
			this.mainMenuOpened = false;
			this.mainMenuChoice = false;
			this.weaponMenu = false;
		}
	}
	
	/*--------------------------------------------------------
		Renderizar as Armas no Menuzinho
	--------------------------------------------------------*/
	public void renderWeapons(string weapon, bool shield)
	{
		if(shield) this.shield = true;
		switch(weapon) 
		{
			case "sword":
				sword = true;
				axe = false;
				bow = false;
			break;
			case "axe":
				axe = true;
				sword = false;
				bow = false;
				break;
			case "bow":
				bow = true;
				sword = false;
				shield = false;
				axe = false;
				break;
		}
	}
	
	/*--------------------------------------------------------
		FUNCOES REFERENTES A CAMPFIRE
		PADRAO MEMENTO E LEVEL UP (Mediator)
	--------------------------------------------------------*/
	public void openCampFire(bool value)
	{
		campFireIndex = 0;
		campFireMenu = value;
	}
	
	void getCampFireMenuInput()
	{
		if(Input.GetButtonDown ("DOWN"))
		{
			if(campFireIndex == 2)
				campFireIndex = 0;
			else campFireIndex += 1;
		}
		else if(Input.GetButtonDown ("UP"))
		{
			if(campFireIndex == 0)
				campFireIndex = 2;
			else campFireIndex -= 1;
		}
	}
	
	IEnumerator NaoDisponivelFalso()
	{
		yield return new WaitForSeconds(3.0f);
		b_naoDisponivel = false;
	}
	
	void processCampfireMenu(int index)
	{
		Debug.Log (index);
		switch(index)
		{
			case 0:
			//salvar jogo
			atributoMemento = characterReference.GetComponent<MyThirdCharacter.MainCharacterControl>().criarMemento();
			CampfireSaveLoad.saveGameState(atributoMemento);
			break;
			case 1:
			StartCoroutine ("Restart");
			campFireMenu = false;
			break;
			case 2:
			b_naoDisponivel = true;
			StartCoroutine ("NaoDisponivelFalso");
			campFireMenu = false;
			break;
		}
	}
	
	public bool getCampFireOpen()
	{
		return this.campFireMenu;
	}
	
	void processCampfireCancel()
	{
		campFireMenu = false;
		campFireIndex = 0;
	}
	
	/*--------------------------------------------------------*/
	/*--------------------------------------------------------*/
	public void inspecionar(bool ins)
	{
		this.inspect = ins;
	}
	
	/*--------------------------------------------------------*/
	//				MORTE
	/*--------------------------------------------------------*/
	
	public void playerDied(bool value)
	{
		dead = value;
	}
}
