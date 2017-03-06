using UnityEngine;
using System.Collections;

public class CombatMediator : MonoBehaviour 
{
	public static CombatMediator instance = null;
	private MyThirdCharacter.MainCharacterControl characterReference;
	
	public static CombatMediator getInstance()
	{
		return instance;	
	}
	
	void Awake()
	{
		if(instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
			
		DontDestroyOnLoad(gameObject);
	}
	
	void Start()
	{
		characterReference = MyThirdCharacter.MainCharacterControl.getInstance();
	}
	
	public void EnemyAttackCharacter(SkeletonDamage enemyDamageRef, int damage)
	{
		if(characterReference.getDefense ())
		{
			characterReference.loseHealth(damage/3);
		}
		else characterReference.loseHealth (damage);
	}
	
	public void PlayerAttackEnemy(EnemyAI enemyReference, int damage)
	{
		if(characterReference.getDead() == false)
		{
			if(enemyReference.getHealth () > 0)
			{
				enemyReference.takeDamage(damage);
			}
		}
	}
}
