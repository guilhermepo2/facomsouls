using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace MyThirdCharacter
{
	[RequireComponent(typeof (MainCharacterController))]
	public class MainCharacterControl : MonoBehaviour
	{
		private MainCharacterController m_Character; 					// A reference to the ThirdPersonCharacter on the object
		private Transform m_Cam;                  						// A reference to the main camera in the scenes transform
		private Vector3 m_CamForward;             						// The current forward direction of the camera
		private Vector3 m_Move;
		private bool paused = false;
		private bool mainMenuOpen = false;
		private UserInterface UI;										// Referencia a Interface do Usuario
		private bool dead;
		
		// processar ataque e defesa
		private bool m_defending = false;
		private bool ableToMove = true;
		
		//idle
		float idleCount = 0.0f;
		
		/*-------------------------------------------------------
			COMBATE - JOGADOR
		-------------------------------------------------------*/
		public float damCollMaxTime = 1;
		float damCollTimer;
		public GameObject damageCollider;
		bool currentlyAttacking;
		
		/*-------------------------------------------------------
					SINGLETON
				O JOGADOR TAMBEM E UM SINGLETON
		-------------------------------------------------------*/
		
		public static MainCharacterControl instance = null;
		
		public static MainCharacterControl getInstance()
		{
			return instance;
		}
		
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
		/* --------------------------------------------------------*/
		
		// ---------------------------------------------------------------------------------------------------------------
		// APLICACAO DO PADRAO MEMENTO																					//
		// ---------------------------------------------------------------------------------------------------------------
		// MAINCHARACTERCONTROL -> ORIGINATOR
		// O Estado a ser salvo sao os Atributos
		Attributes myAttributes;								// instancia de uma classe que vai conter os atributos
		
		// ---------------------------------------------------------------------------------------------------------------
		
		//com vai funcionar o memento:
		// o personagem vai na fogueira e pode dar load e save
		// ao dar save e enviado para um caretaker a mensagem que deseja salvar e os estados atuais
		// ao dar load, e enviado para um caretaker a mensagem que deseja dar load, ele retorna os estados salvos

		// ---------------------------------------------------------------------------------------------------------------
		// APLICACAO DO PADRAO STRATEGY																					//
		// ---------------------------------------------------------------------------------------------------------------
		// ARMAS E INSTANCIAS																							//
		// ---------------------------------------------------------------------------------------------------------------
		private Animator animatorReference;									// referencia ao animator					//
		public GameObject swordPrefab;										// prefab da espada							//		
		public GameObject swordEquipedPrefab;								// prefab da espada equipada				//
		public GameObject axePrefab;										// prefab do machado						//
		public GameObject axeEquippedPrefab;								// prefab do machado equipado				//
		public GameObject shieldPrefab;										// prefab do escudo							//
		public GameObject shieldEquipedPrefab;								// prefab do escudo equipado				//
		public GameObject bowPrefab;										// prefab do arco							//
		public GameObject bowEquippedPrefab;								// prefab do arco equipado					//
		public GameObject rightHand;										// referencia da mao direita				//
		public GameObject leftForeArm;										// referencia do antebraco esquerdo			//
		public GameObject leftHand;											// referencia da mao esquerda				//
		private GameObject weaponInstance = null;							// referencia a instancia da arma			//
		private GameObject shieldInstance = null;							// referencia a instancia do escudo			//
		// ---------------------------------------------------------------------------------------------------------------
		// STRATEGY																										//
		// ---------------------------------------------------------------------------------------------------------------
		ArmaStrategy minhaArma;	    										// estrategia da arma						//
		ItemStrategy minhaPotion = new Potion();							// estrategia da pocao						//	
		// ---------------------------------------------------------------------------------------------------------------
		
		private void Start()
		{
			animatorReference = GetComponent<Animator>();
			// ---------------------------------------------------------------------------------------------------------------
			// STRATEGY
			// ---------------------------------------------------------------------------------------------------------------
			minhaArma = new Nenhuma(animatorReference); // strategy
			// ---------------------------------------------------------------------------------------------------------------
			//UI = GameObject.Find ("_User Interface").GetComponent<UserInterface>();
			UI = UserInterface.getInstance();
			
			// get the transform of the main camera
			if (Camera.main != null)
			{
				m_Cam = Camera.main.transform;
			}
			else
			{
				Debug.LogWarning(
					"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
				// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
			}
			
			// get the third person character ( this should never be null due to require component )
			m_Character = GetComponent<MainCharacterController>();
			
			// ---------------------------------------------------------------------------------------------------------------
			//					PADRAO MEMENTO - ULTIMA COISA A SER CARREGADA
			// ---------------------------------------------------------------------------------------------------------------
			switch(PlayerPrefs.GetString ("VaiTerLoad"))								// verificando se vai ter load
			{
			case "Sim":
				Debug.Log ("vai ter load sim!");
				myAttributes = CampfireSaveLoad.loadGameState();						// pegar o estado salvo - faz o papel de setMemento();
				processarEstado();
				// fazer o load usando memento
				break;
			case "Nao":
				Debug.Log ("Nao vai ter load!");
				myAttributes = new Attributes(transform.position); 					// criando novo estado de memento
				break;	
			}
			// ---------------------------------------------------------------------------------------------------------------
		}
		
		private void Update()
		{
			if(myAttributes.getVidaAtual() == 0) // PERSONAGEM MORREU!
			{
				dead = true;
				m_Character.die();
				
				// FADE DO PERSONAGEM MORRENDO;
				UI.playerDied(true);
			}
			
			if(!dead)
			{
				if(Input.GetButtonDown ("Start Button"))
				{
					paused = !paused;
					
					if(paused)
					{
						UI.gamePaused(true);
						Time.timeScale = 0;
					}
					else
					{
						UI.gamePaused(false);
						Time.timeScale = 1.0f;
					}
				}
				
				if(Input.GetButtonDown ("Select Button"))
				{
					mainMenuOpen = !mainMenuOpen;
					if(mainMenuOpen)
					{
						m_Character.setTPose();
					}
					else
					{
						m_Character.endTPose();
					}
					UI.openMainMenu(mainMenuOpen);
				}
			}	
			
		}
		
		
		// Fixed update is called in sync with physics
		private void FixedUpdate()
		{		
			if(!paused && !dead)
			{	
				if(myAttributes.getStaminaAtual() < myAttributes.getStaminaTotal ())
				{
					myAttributes.setStaminaAtual(myAttributes.getStaminaAtual() + 1);
				}
				// read inputs
				float h = CrossPlatformInputManager.GetAxis("Horizontal");
				float v = CrossPlatformInputManager.GetAxis("Vertical");
				
				if(h == 0f && v == 0f)
				{
					idleCount += Time.deltaTime;
					if (idleCount >= 30.0f) {
						m_Character.playIdle(true);
					}
				}
				else {
					idleCount = 0f;
					m_Character.playIdle(false);
				}
				
				// calculate move direction to pass to character
				if (m_Cam != null)
				{
					// calculate camera relative direction to move:
					m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
					m_Move = v*m_CamForward + h*m_Cam.right;
				}
				else
				{
					// we use world-relative directions in the case of no main camera
					m_Move = v*Vector3.forward + h*Vector3.right;
				}
				#if !MOBILE_INPUT
				// walk speed multiplier
				if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
				#endif
				
				// pass all parameters to the character control script
				//Debug.Log (ableToMove);
				if(ableToMove)
				{
					m_Character.Move(m_Move);
				}
				
				// HANDLING INPUT FOR ATTACK & DEFENSE
				if(Input.GetButtonDown ("Defending") && myAttributes.getStaminaAtual() > 60)
				{
					m_defending = true;
					myAttributes.setStaminaAtual (myAttributes.getStaminaAtual() - 60);
				} else if(Input.GetButtonDown ("Heavy Attack") && myAttributes.getStaminaAtual() > 60) {
					m_Character.processAttack(0);
					myAttributes.setStaminaAtual (myAttributes.getStaminaAtual() - 60);
					currentlyAttacking = true;
					HandleAttack();
				} else if(Input.GetButtonDown ("Light Attack") && myAttributes.getStaminaAtual() > 60) {
					m_Character.processAttack(1);
					myAttributes.setStaminaAtual (myAttributes.getStaminaAtual() - 60);
					currentlyAttacking = true;
					HandleAttack();
				} else if (Input.GetButtonUp ("Defending"))
				{
					m_defending = false;
				}
				
				//USING ITEMS
				if(Input.GetButtonDown ("Heal")) {
					minhaPotion.usarItem();
					m_Character.healCharacter();
				}
				
				if(Input.GetButtonDown ("Special Item")) {
					m_Character.healCharacter ();
				}
				
				m_Character.processBlock(m_defending);
				m_Character.checkIdle();
			}
		}
		
		public void setAbleToMove(bool move)
		{
			this.ableToMove = move;
		}
		
		// ---------------------------------------------------------------------------------------------------------------
		// APLICACAO DO PADRAO STRATEGY																					//
		// ---------------------------------------------------------------------------------------------------------------
		// ---------------------------------------------------------------------------------------------------------------
		// CRIANDO, DESTRUINDO E INSTANCIANDO AS ARMAS																	//
		// ---------------------------------------------------------------------------------------------------------------
		public void equipWeapon(String weapon)
		{
			switch(weapon)
			{
			case "sword":
			if(weaponInstance != null)
				Destroy(weaponInstance);
			minhaArma = new Espada(animatorReference);
			myAttributes.setIdArmaEquipada(0);
			
			// criando instancia
			weaponInstance = Instantiate (swordPrefab, swordPrefab.transform.position, swordPrefab.transform.rotation) as GameObject;
			weaponInstance.transform.parent = rightHand.transform;
			weaponInstance.transform.localPosition = swordEquipedPrefab.transform.position;
			weaponInstance.transform.localRotation = swordEquipedPrefab.transform.rotation;
			break;
			
			case "axe":
			if(weaponInstance != null)
				Destroy (weaponInstance);
			minhaArma = new Machado(animatorReference);
			myAttributes.setIdArmaEquipada(1);
			
			// criando instancia
			weaponInstance = Instantiate(axePrefab, axePrefab.transform.position, axePrefab.transform.rotation) as GameObject;
			weaponInstance.transform.parent = rightHand.transform;
			weaponInstance.transform.localPosition = axeEquippedPrefab.transform.position;
			weaponInstance.transform.localRotation = axeEquippedPrefab.transform.rotation;
			break;
			
			case "bow":
			if(weaponInstance!= null)
				Destroy (weaponInstance);
			if(shieldInstance != null)
				Destroy (shieldInstance);
				
				
			minhaArma = new Arco(animatorReference);
			myAttributes.setIdArmaEquipada(2);
			myAttributes.setEscudoEquipado(0);
		
			// criando instancia
			weaponInstance = Instantiate (bowPrefab, bowPrefab.transform.position, bowPrefab.transform.rotation) as GameObject;
			weaponInstance.transform.parent = leftHand.transform;
			weaponInstance.transform.localPosition = bowEquippedPrefab.transform.position;
			weaponInstance.transform.localRotation = bowEquippedPrefab.transform.rotation;
			break;
			
			
			case "shield":
			if(weaponInstance != null && weaponInstance.name == "bow(Clone)")
				Destroy (weaponInstance);
				
			if(shieldInstance == null)
				{
				// criando instancia
				myAttributes.setEscudoEquipado(1);
				shieldInstance = Instantiate (shieldPrefab) as GameObject;
				shieldInstance.transform.parent = leftForeArm.transform;
				shieldInstance.transform.localPosition = shieldEquipedPrefab.transform.position;
				shieldInstance.transform.localRotation = shieldEquipedPrefab.transform.rotation;
				shieldInstance.transform.localScale = shieldEquipedPrefab.transform.localScale;
				}
			m_Character.endTPose();
			break;
			}
		}
		
		
		
		public void heal(int vidaRecuperada)
		{
			myAttributes.setVidaAtual(myAttributes.getVidaAtual () + vidaRecuperada);
		}
		// ---------------------------------------------------------------------------------------------------------------
		// FIM DA APLICACAO DO STRATEGY  																				//
		// ---------------------------------------------------------------------------------------------------------------
		
		
		// ---------------------------------------------------------------------------------------------------------------
		//					COMBATE
		// ---------------------------------------------------------------------------------------------------------------
		IEnumerator AttackCoroutine(float time)
		{
			yield return new WaitForSeconds(time);
			//currentlyAttacking = false;
			damageCollider.SetActive (false);
		}
		
		void HandleAttack()
		{
			this.damageCollider.GetComponent<PlayerDamageCollider>().damageToDeal = minhaArma.getDano ();
			Debug.Log (minhaArma.getDano ());
			if(currentlyAttacking)
			{
				StartCoroutine (AttackCoroutine(0.5f));
				damageCollider.SetActive (true);
			}
			else damageCollider.SetActive(false);
		}
		
		// ---------------------------------------------------------------------------------------------------------------
		//					PADRAO MEMENTO
		// ---------------------------------------------------------------------------------------------------------------
		//
		//                 ALTERAÇOES DE STATUS PARA O COMBATE
		// ---------------------------------------------------------------------------------------------------------------
		
		
		public bool getDefense()
		{
			return this.m_defending;
		}
		
		public void loseHealth(int healthLost)
		{
			myAttributes.setVidaAtual(myAttributes.getVidaAtual () - healthLost);
			m_Character.getHit ();
		}
		
		public bool getDead()
		{
			return this.dead;
		}
		
		// ---------------------------------------------------------------------------------------------------------------
		
		public Memento criarMemento()
		{
			return new Memento(myAttributes);
		}
		
		public void recuperarMemento(Memento memento)
		{
			this.myAttributes = memento.getAtributos();
		}
		
		/*
		public void criarMemoria()
		{
			Memento.salvarEstado(myAttributes);
		} 
		
		public void carregarMemoria()
		{
			PlayerPrefs.SetString("VaiTerLoad", "Sim");
			Application.LoadLevel("Adventure");
		} */
		
		private void processarEstado()
		{
			bool shield = false;
			
			if(myAttributes.getEscudoEquipado() == 1)
			{
				equipWeapon("shield");
				shield = true;
			}
			
			Debug.Log (shield);
			
			switch(myAttributes.getIdArmaEquipada())
			{
				case 0:
				//equipar espada
				equipWeapon("sword");
				UI.renderWeapons("sword", shield);
				break;
				case 1:
				equipWeapon("axe");
				UI.renderWeapons("axe", shield);
				break;
				case 2:
				//equipar arco
				equipWeapon("bow");
				UI.renderWeapons("bow", false);
				break;
			}
			
		}
		// ---------------------------------------------------------------------------------------------------------------
		//					FIM PADRAO MEMENTO
		// ---------------------------------------------------------------------------------------------------------------
	}
}
