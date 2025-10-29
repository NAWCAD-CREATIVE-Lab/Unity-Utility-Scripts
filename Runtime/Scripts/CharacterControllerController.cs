// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CREATIVE.Utility
{
	/**
		A script used to provide Move calls to a CharacterController based on
		input from InputActions.
	*/
	[RequireComponent(typeof(CharacterController))]
	public class CharacterControllerController : MonoBehaviour
	{
		[field: SerializeField] float GravityAcceleration = -9.81f;
								float registeredGravityAcceleration;

		[field: SerializeField]	InputActionReference ForwardInputAction;
								InputActionReference registeredForwardInputAction;
		
		[field: SerializeField]	InputActionReference BackwardInputAction;
								InputActionReference registeredBackwardInputAction;

		[field: SerializeField]	InputActionReference LeftInputAction;
								InputActionReference registeredLeftInputAction;
		
		[field: SerializeField]	InputActionReference RightInputAction;
								InputActionReference registeredRightInputAction;

		Dictionary<InputAction, float> currentInputValues;

		bool forwardDominant = true;

		bool leftDominant = true;

		CharacterController registeredCharacterController;

		bool registered = false;

		void Start()		=> reRegister();
		void OnEnable()		=> reRegister();

#if UNITY_EDITOR
		void OnValidate()	=> reRegister();
#endif

		void OnDisable()	=> unRegister();
		void OnDestroy()	=> unRegister();

		void reRegister()
		{
			unRegister();

			registeredForwardInputAction = ForwardInputAction;
			registeredBackwardInputAction = BackwardInputAction;
			registeredLeftInputAction = LeftInputAction;
			registeredRightInputAction = RightInputAction;

			registeredGravityAcceleration = GravityAcceleration;

			if
			(
				Application.isPlaying && isActiveAndEnabled &&
				registeredForwardInputAction != null &&
				registeredBackwardInputAction != null &&
				registeredLeftInputAction != null &&
				registeredRightInputAction != null
			)
			{
				registeredForwardInputAction.action.started += processInput;
				registeredForwardInputAction.action.canceled += processInput;

				registeredBackwardInputAction.action.started += processInput;
				registeredBackwardInputAction.action.canceled += processInput;

				registeredLeftInputAction.action.started += processInput;
				registeredLeftInputAction.action.canceled += processInput;

				registeredRightInputAction.action.started += processInput;
				registeredRightInputAction.action.canceled += processInput;

				registeredCharacterController = GetComponent<CharacterController>();

				currentInputValues = new Dictionary<InputAction, float>();

				currentInputValues[registeredForwardInputAction.action] = 0f;
				currentInputValues[registeredBackwardInputAction.action] = 0f;
				currentInputValues[registeredLeftInputAction.action] = 0f;
				currentInputValues[registeredRightInputAction.action] = 0f;

				registered = true;
			}
		}

		void unRegister()
		{
			if (registered)
			{
				registeredForwardInputAction.action.started -= processInput;
				registeredForwardInputAction.action.canceled -= processInput;

				registeredBackwardInputAction.action.started -= processInput;
				registeredBackwardInputAction.action.canceled -= processInput;

				registeredLeftInputAction.action.started -= processInput;
				registeredLeftInputAction.action.canceled -= processInput;

				registeredRightInputAction.action.started -= processInput;
				registeredRightInputAction.action.canceled -= processInput;

				registered = false;
			}
		}

		void processInput(InputAction.CallbackContext context)
		{
			if (context.action == registeredForwardInputAction.action)
				forwardDominant = (context.phase == InputActionPhase.Started);
			
			if (context.action == registeredBackwardInputAction.action)
				forwardDominant = (context.phase != InputActionPhase.Started);

			if (context.action == registeredLeftInputAction.action)
				leftDominant = (context.phase == InputActionPhase.Started);

			if (context.action == registeredRightInputAction.action)
				leftDominant = (context.phase != InputActionPhase.Started);

			currentInputValues[context.action] = context.ReadValue<float>();
		}

		void Update()
		{
			if (Application.isPlaying && isActiveAndEnabled && registered)
			{
				float forwardSpeed, leftSpeed;

				if (forwardDominant)
					forwardSpeed = currentInputValues[registeredForwardInputAction.action];

				else
					forwardSpeed = -currentInputValues[registeredBackwardInputAction.action];

				if (leftDominant)
					leftSpeed = currentInputValues[registeredLeftInputAction.action];

				else
					leftSpeed = -currentInputValues[registeredRightInputAction.action];


				registeredCharacterController.Move
				(
					(
						(
							(
								(transform.forward * forwardSpeed).normalized +
								(transform.right * -leftSpeed).normalized
							).normalized *
							MathF.Max(MathF.Abs(forwardSpeed), MathF.Abs(leftSpeed))
						) +
						// TODO: Actually use this variable as gravitational acceleration, instead of speed
						transform.up * registeredGravityAcceleration
					) *
					Time.deltaTime
				);
			}
		}
	}
}