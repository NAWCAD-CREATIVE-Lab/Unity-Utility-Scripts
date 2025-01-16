using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CREATIVE.Utility
{
	/**
		This component listens for a particular Input Action from the
		project-wide Input Actions and links it to a Unity Event callback.
	*/
	public class InputActionProcessor : MonoBehaviour
	{
		/**
			A reference to the InputAction that should be listened for.
		*/
		public InputActionReference Action;

		/**
			An enum only used for the ActionStage field.
		*/
		public enum e_ActionStage
		{
			Started,
			Performed,
			Cancelled
		}

		/**
			Which stage of the InputAction should be listened for.

			Performed is the most commonly used stage. It happens once when a
			button is pressed.

			Started and Cancelled can be used to detect when a button starts and
			stops being held down.
		*/
		public e_ActionStage ActionStage = e_ActionStage.Performed;

		/**
			The UnityEvent that is invoked by the InputAction
		*/
		public UnityEvent Callback;

		private e_ActionStage registeredStage;

		private UnityEvent registeredCallback = null;

		void Start()		=> ReRegister();
		void OnValidate()	=> ReRegister();
		void OnEnable()		=> ReRegister();

		void OnDisable()	=> UnRegister();
		void OnDestroy()	=> UnRegister();

		private void ReRegister()
		{
			UnRegister();

			if (Application.isPlaying && isActiveAndEnabled && Action!=null)
			{
				registeredStage = ActionStage;

				if (registeredStage == e_ActionStage.Started)
					Action.action.started += Invoke;
				
				if (registeredStage == e_ActionStage.Performed)
					Action.action.performed += Invoke;
				
				if (registeredStage == e_ActionStage.Cancelled)
					Action.action.canceled += Invoke;

				registeredCallback = Callback;
			}
		}

		private void UnRegister()
		{
			if (Action != null && registeredCallback != null)
			{
				if (registeredStage == e_ActionStage.Started)
					Action.action.started -= Invoke;
				
				if (registeredStage == e_ActionStage.Performed)
					Action.action.performed -= Invoke;
				
				if (registeredStage == e_ActionStage.Cancelled)
					Action.action.canceled -= Invoke;

				registeredCallback = null;
			}
		}

		private void Invoke(InputAction.CallbackContext context)
		{
			if (registeredCallback != null)
				registeredCallback.Invoke();
		}
	}
}