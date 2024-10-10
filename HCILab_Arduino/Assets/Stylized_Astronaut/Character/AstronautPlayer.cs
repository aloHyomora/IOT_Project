using System;
using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour
	{
		public static AstronautPlayer Instance;

		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;

		public float xValue = 0;
		public float yValue = 0;
		public bool verticalPressed = false;
		public bool horizontalPressed = false;
		private void Awake()
		{
			if (Instance == null) Instance = this;
		}

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update (){
			
			if (Input.GetKey ("w") || (yValue > 0))
			{
				anim.SetInteger ("AnimationPar", 1);
			}  else {
				anim.SetInteger ("AnimationPar", 0);
			}
			
			float verticalKeyboardInputValue = Input.GetAxis("Vertical");
			float horizontalKeyboardInputValue = Input.GetAxis("Horizontal");

			if (verticalKeyboardInputValue != 0)
			{
				verticalPressed = true;
				
				if(controller.isGrounded)
					moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
			}
			else
			{
				// 조이스틱 입력 y축
				verticalPressed = false;
				moveDirection = transform.forward * yValue * speed;
			}


			float turn;
			if (horizontalKeyboardInputValue != 0)
			{
				horizontalPressed = true;
				turn = Input.GetAxis("Horizontal");
			}
			else
			{
				horizontalPressed = false;
				turn = xValue;
			}
			
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			controller.Move(moveDirection * Time.deltaTime);
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}
}
