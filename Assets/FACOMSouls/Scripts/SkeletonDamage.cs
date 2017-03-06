using UnityEngine;
using System.Collections;

public class SkeletonDamage : MonoBehaviour {

	public int damageToDeal = 150;
	MyThirdCharacter.MainCharacterControl playerRef;
	
	void OnTriggerEnter(Collider other)
	{
		playerRef = other.GetComponent<MyThirdCharacter.MainCharacterControl>();
		if(playerRef != null)
		{
			CombatMediator.getInstance ().EnemyAttackCharacter(this, damageToDeal);
		}	
	}
}
