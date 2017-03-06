using UnityEngine;
using System.Collections;

namespace MyThirdCharacter
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class MainCharacterController : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		
		Rigidbody m_Rigidbody;
		Animator m_Animator;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		bool m_IsGrounded = true;
		
		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}
		
		public void processBlock(bool block)
		{
			m_Animator.SetBool ("Block", block);
		}
		
		public void processAttack(int atk)
		{
			if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("Sword Tree") ||
			   m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("BowTree"))
			{
				m_Animator.SetTrigger("Attack") ;
			}
			
			if(atk == 0)
			{
				if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Heavy 1"))
				{
					m_Animator.ResetTrigger("Attack");
					m_Animator.SetTrigger("Heavy Attack 1");
				} else if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Heavy 2"))
				{
					m_Animator.ResetTrigger ("Heavy Attack 1");
				}
			} else {
				if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Heavy 1"))
				{
					m_Animator.ResetTrigger("Attack");
					m_Animator.SetTrigger("Slash1");
				} else if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Slash 1")) {
					m_Animator.ResetTrigger("Slash1");
					m_Animator.SetTrigger ("Slash2");
				}	else if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Slash 2")) {
					m_Animator.ResetTrigger("Slash2");
					m_Animator.SetTrigger ("Slash3");
				} else if(m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Slash 3"))
				{
					m_Animator.ResetTrigger ("Slash3");
				}
			}	
		}
		
		public void checkIdle()
		{
			if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("Sword Tree") ||
			   m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("BowTree") ||
			   m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("NoWeapon Tree")) {
			   
			   	GameObject.Find("maincharacter").GetComponent<MainCharacterControl>().setAbleToMove(true);
			   }
			  else {
				GameObject.Find("maincharacter").GetComponent<MainCharacterControl>().setAbleToMove(false);
			}
		}
		
		public void playIdle(bool p)
		{
			m_Animator.SetBool ("GANGNAM", p);
		}
		
		public void healCharacter()
		{
			if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("Sword Tree"))
			{
				m_Animator.SetTrigger ("ROAR");
			}
		}
		
		
		public void Move(Vector3 move)
		{
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			//CheckGroundStatus();
			//move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			
			ApplyExtraTurnRotation();
			
			// send input and other state parameters to the animator
			UpdateAnimator(move);
			//
			transform.Translate(Vector3.forward * m_ForwardAmount * m_MoveSpeedMultiplier * Time.deltaTime);
			//
		}
		
		public void getHit()
		{
			m_Animator.SetTrigger ("Impact");
		}
		
		public void setTPose()
		{
			m_Animator.SetBool("TPose", true);
		}
		
		public void endTPose()
		{
			m_Animator.SetBool("TPose", false);
		}
		
		public void die()
		{
			m_Animator.SetTrigger ("Die1");
			if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName ("Die"))
				m_Animator.ResetTrigger("Die1");
		}
		
		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}
		
		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
		
	}
}
