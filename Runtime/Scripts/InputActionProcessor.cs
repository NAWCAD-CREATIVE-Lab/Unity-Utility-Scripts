// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CREATIVE.Utility
{
	/**
		An enum only used for the ActionStage field.
	*/
	public enum InputActionStage
	{
		Started,
		Performed,
		Cancelled
	}
	
	/**
		This component listens for a particular Input Action from the
		project-wide Input Actions and links it to a Unity Event callback.
	*/
	public class InputActionProcessor : MonoBehaviour
	{
		/**
			A reference to the InputAction that should be listened for.
		*/
		[field: SerializeField]
		private InputActionReference Action;
		private InputActionReference registeredAction;

		/**
			Which stage of the InputAction should be listened for.

			Performed is the most commonly used stage. It happens once when a
			button is pressed.

			Started and Cancelled can be used to detect when a button starts and
			stops being held down.
		*/
		[field: SerializeField]
		private InputActionStage ActionStage = InputActionStage.Performed;
		private InputActionStage registeredActionStage;

		/**
			The UnityEvent that is invoked by the InputAction
		*/
		[field: SerializeField]
		private UnityEvent Callback;
		private UnityEvent registeredCallback;

		private bool registered = false;

		void Start()		=> ReRegister();
		void OnValidate()	=> ReRegister();
		void OnEnable()		=> ReRegister();

		void OnDisable()	=> UnRegister();
		void OnDestroy()	=> UnRegister();

		private void ReRegister()
		{
			UnRegister();

			registeredAction = Action;
			registeredActionStage = ActionStage;
			registeredCallback = Callback;

			if (Application.isPlaying && isActiveAndEnabled && registeredAction!=null && registeredCallback!=null)
			{
				registeredActionStage = ActionStage;

				if (registeredActionStage == InputActionStage.Started)
					registeredAction.action.started += Invoke;
				
				if (registeredActionStage == InputActionStage.Performed)
					registeredAction.action.performed += Invoke;
				
				if (registeredActionStage == InputActionStage.Cancelled)
					registeredAction.action.canceled += Invoke;
				
				registered = true;
			}
		}

		private void UnRegister()
		{
			if (registered)
			{
				if (registeredActionStage == InputActionStage.Started)
					registeredAction.action.started -= Invoke;
				
				if (registeredActionStage == InputActionStage.Performed)
					registeredAction.action.performed -= Invoke;
				
				if (registeredActionStage == InputActionStage.Cancelled)
					registeredAction.action.canceled -= Invoke;

				registered = false;
			}
		}

		private void Invoke(InputAction.CallbackContext context) => registeredCallback.Invoke();
	}
}