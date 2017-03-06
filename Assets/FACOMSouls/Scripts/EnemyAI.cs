using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
	//stats
	private int health = 100;
	private int damage = 15;
	private bool dead = false;
		
	//
	NavMeshAgent agent;
	Animator anim;
	
	public float moveSpeed = 2f;
	public Transform curTarget;
	public float attackRange = 2;
	public bool currentlyAttacking;
	public float attackRate = 2;
	
	float attTimer;
	bool moving = false;
	bool attackOnce = false;
	bool stopRotating = false;
	float attackCurve;
	
	public GameObject damageCollider;
	
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		agent.stoppingDistance = attackRange;
		//agent.updateRotation = false;
	}
	
	void Update()
	{	
		Debug.Log (this.health);
		if (!dead)
		{
			if(!currentlyAttacking)
			{
				MovementHandler();
			}
			AttackHandler();
		}
		else
		{
			//Debug.Log ("Skeleton is dead :)");
			//this.GetComponent<Rigidbody>().MoveRotation(Quaternion.identity);
			//this.GetComponent<CapsuleCollider>().height = 0.32f;
		}
		
	}
	
	void MovementHandler()
	{
		if(curTarget != null)
		{
			agent.SetDestination(curTarget.position);
			
			Vector3 relDirection = transform.InverseTransformDirection(agent.desiredVelocity);
			float distance = Vector3.Distance (transform.position, curTarget.position);
			
			if(distance <= attackRange)
			{
				anim.SetFloat ("Forward", 0.0f);
				attTimer += Time.deltaTime;
				
				if(attTimer > attackRate)
				{
					currentlyAttacking = true;
					attTimer = 0;
				}
			} else {
				transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed);
				anim.SetFloat ("Forward", 1.0f);
			}
			
		}
	}
	
	void AttackHandler()
	{
		if(currentlyAttacking)
		{
			if(!stopRotating)
			{
				Vector3 dir = curTarget.position - transform.position;
				Quaternion targetRot = Quaternion.LookRotation (dir);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * 5);
				
				float angle = Vector3.Angle (transform.forward, dir);
				
				if (angle < 30)
				{
					if(!attackOnce)
					{
						agent.Stop ();
						anim.SetBool("Attack", true);
						StartCoroutine ("CloseAttack");
						attackOnce = true;
					}
				}
			}
		}
		
		attackCurve = anim.GetFloat ("attackCurve");
		
		if(attackCurve > .5f)
		{
			stopRotating = true;
			if(attackCurve > .9f)
			{
				damageCollider.SetActive (true);
			}
			else
			{
				if(damageCollider.activeInHierarchy)
				{
					damageCollider.SetActive (false);
				}
			}
		}
	}
	
	IEnumerator CloseAttack()
	{
		yield return new WaitForSeconds(1.0f);
		anim.SetBool ("Attack", false);
		attackOnce = false;
		agent.Resume ();
		currentlyAttacking = false;
		stopRotating = false;
	}
	
	public void stop()
	{
		anim.SetBool ("Attack", false);
		anim.SetFloat ("Forward", 0.0f);
		curTarget = null;
		attackOnce = false;
		currentlyAttacking = false;
		stopRotating = false;
	}
	
	public void takeDamage(int damage)
	{
		this.health -= damage;
		if(health <= 0)
		{
			dead = true;
			anim.SetBool ("Dead", true);
		}
		else anim.SetTrigger ("Hit");
	}
	
	public int getHealth()
	{
		return this.health;
	}
}
