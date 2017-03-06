using UnityEngine;
using System.Collections;

public class PlayerDamageCollider : MonoBehaviour 
{
	public int damageToDeal;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<EnemyAI>())
		{
			CombatMediator.getInstance ().PlayerAttackEnemy(other.GetComponent<EnemyAI>(),damageToDeal);
		}
	}
}
