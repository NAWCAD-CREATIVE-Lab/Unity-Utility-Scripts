using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CREATIVE.Utility
{
	/**
		This class provides methods to modify and animate properties of a
		GameObject's Transform.
	*/
	[RequireComponent(typeof(Transform))]
	public class TransformController : MonoBehaviour
	{
		private enum Property { Position, Rotation }

		private enum Dimension { X, Y, Z }
		
		/**
			Slowly start and slowly stop animations.
		*/
		[field: SerializeField]
		private bool EaseInAndOutAnimation = true;

		/**
			How long (in seconds) animations will last.
		*/
		[field: SerializeField]
		[Range(1, 10)]
		private int AnimationDuration = 1;

		/**
			A reference to an Input action that will continuously affect a
			property of this Transform.
		*/
		[field: SerializeField]
		private InputActionReference DeltaInputAction;
		private InputActionReference registeredDeltaInputAction;

		/**
			The property of this transform that an Input Action will
			continuously affect.
		*/
		[field: SerializeField]
		private Property DeltaInputProperty;
		private Property registeredDeltaInputProperty;

		/**
			The dimension of the property in this transform that an Input Action
			will continuously affect.
		*/
		[field: SerializeField]
		private Dimension DeltaInputDimension;
		private Dimension registeredDeltaInputDimension;

		/**
			Whether or not the Input action that will continuously affect a
			property of this Transform will be started and later cancelled to
			produce a continous effect, as opposed to rapidly performed.
		*/
		[field: SerializeField]
		private bool DeltaInputIsWillBeHeld;
		private bool registeredDeltaInputWillBeHeld;

		/**
			Whether or not the property of this Transform continuously affected
			by an Input Action should stop being affected below a certain
			minimum value.
		*/
		[field: SerializeField]
		private bool DeltaInputHasMinimum = false;
		private bool registeredDeltaInputHasMinimum;

		/**
			The minimum value at which the property of this Transform
			continuously affected by an Input Action should stop decreasing. 
		*/
		[field: SerializeField]
		private float DeltaInputMinimum;
		private float registeredDeltaInputMinimum;

		/**
			Whether or not the property of this Transform continuously affected
			by an Input Action should stop being affected above a certain
			maximum value.
		*/
		[field: SerializeField]
		private bool DeltaInputHasMaximum = false;
		private bool registeredDeltaInputHasMaximum;

		/**
			The maximum value at which the property of this Transform
			continuously affected by an Input Action should stop increasing. 
		*/
		[field: SerializeField]
		private float DeltaInputMaximum;
		private float registeredDeltaInputMaximum;

		private float registeredDeltaInputCurrent;

		private float registeredDeltaInputIncrement;

		private InputAction registeredDeltaInputInProgress;

		private bool registered = false;
		
		private Vector3 initialPosition;

		private Vector3 initialScale;

		private Quaternion initialRotation;

		private Animation animationInProgress = null;

		void OnValidate()	=> ReRegister();
		void OnEnable()		=> ReRegister();

		void OnDisable()	=> UnRegister();
		void OnDestroy()	=> UnRegister();

		void Start()
		{
			initialPosition = transform.localPosition;
			
			initialScale = transform.localScale;

			initialRotation = transform.localRotation;

			ReRegister();
		}

		/**
			If an animation is in progress, run the next increment.
		*/
		void Update()
		{
			if (animationInProgress != null)
				if (animationInProgress.Increment(Time.deltaTime))
					animationInProgress = null;

			if (registeredDeltaInputInProgress != null)
				IncrementDeltaInput(true);
		}

		private void ReRegister()
		{
			UnRegister();

			registeredDeltaInputAction = DeltaInputAction;
			registeredDeltaInputProperty = DeltaInputProperty;
			registeredDeltaInputDimension = DeltaInputDimension;
			registeredDeltaInputWillBeHeld = DeltaInputIsWillBeHeld;
			registeredDeltaInputHasMinimum = DeltaInputHasMinimum;
			registeredDeltaInputHasMaximum = DeltaInputHasMaximum;
			registeredDeltaInputMinimum = DeltaInputMinimum;
			registeredDeltaInputMaximum = DeltaInputMaximum;

			if (Application.isPlaying && isActiveAndEnabled && registeredDeltaInputAction!=null)
			{
				if
				(
					registeredDeltaInputHasMinimum && registeredDeltaInputHasMaximum &&
					registeredDeltaInputMinimum >= registeredDeltaInputMaximum
				)
					Debug.LogError (nameof(DeltaInputMinimum) + " must be less than " + nameof(DeltaInputMaximum));
				
				else
				{
					registeredDeltaInputCurrent = 0f;
					
					registeredDeltaInputAction.action.started += ProcessDeltaInput;
					registeredDeltaInputAction.action.canceled += ProcessDeltaInput;

					registered = true;
				}
			}
		}

		private void UnRegister()
		{
			if (registered)
			{
				registeredDeltaInputAction.action.started -= ProcessDeltaInput;
				registeredDeltaInputAction.action.canceled -= ProcessDeltaInput;

				registered = false;
			}
		}

		private void ProcessDeltaInput(InputAction.CallbackContext context)
		{
			if (context.phase==InputActionPhase.Canceled && registeredDeltaInputInProgress==context.action)
				registeredDeltaInputInProgress = null;

			else if (context.phase == InputActionPhase.Started)
			{
				registeredDeltaInputIncrement = context.ReadValue<float>();

				if (registeredDeltaInputWillBeHeld)
					registeredDeltaInputInProgress = context.action;

				else
					IncrementDeltaInput(false);
			}
		}

		private void IncrementDeltaInput(bool scaleByTime)
		{
			if
			(
				Application.isPlaying && isActiveAndEnabled && registered &&
				(
					animationInProgress == null ||
					registeredDeltaInputDimension != animationInProgress.Dimension ||
					registeredDeltaInputProperty != animationInProgress.Property
				)
			)
			{
				float delta = registeredDeltaInputIncrement;

				if (scaleByTime)
					delta *= Time.deltaTime;
				
				float current = 0f;

				if (registeredDeltaInputProperty == Property.Position)
				{
					if (registeredDeltaInputDimension == Dimension.X) current = transform.localPosition.x;
					if (registeredDeltaInputDimension == Dimension.Y) current = transform.localPosition.y;
					if (registeredDeltaInputDimension == Dimension.Z) current = transform.localPosition.z;
				}

				if (registeredDeltaInputProperty == Property.Rotation)
					current = registeredDeltaInputCurrent;

				if
				(
					(
						registeredDeltaInputProperty == Property.Position &&
						(
							!registeredDeltaInputHasMinimum ||
							current > registeredDeltaInputMinimum ||
							(current<=registeredDeltaInputMinimum && delta>0)
						) &&
						(
							!registeredDeltaInputHasMaximum ||
							current<registeredDeltaInputMaximum ||
							(current>=registeredDeltaInputMaximum && delta<0)
						)
					) ||
					(
						registeredDeltaInputProperty == Property.Rotation &&
						(
							!registeredDeltaInputHasMinimum || !registeredDeltaInputHasMaximum ||
							(
								(
									current > registeredDeltaInputMinimum ||
									(current<=registeredDeltaInputMinimum && delta>0)
								) &&
								(
									current<registeredDeltaInputMaximum ||
									(current>=registeredDeltaInputMaximum && delta<0)
								)
							)
						)
					)
				)
				{
					if (registeredDeltaInputProperty == Property.Rotation)
					{
						transform.Rotate(new Vector3
						(
							registeredDeltaInputDimension==Dimension.X? delta : 0,
							registeredDeltaInputDimension==Dimension.Y? delta : 0,
							registeredDeltaInputDimension==Dimension.Z? delta : 0
						));

						registeredDeltaInputCurrent += delta;
					}

					if (registeredDeltaInputProperty == Property.Position)
					{
						transform.Translate(new Vector3
						(
							registeredDeltaInputDimension==Dimension.X? delta : 0,
							registeredDeltaInputDimension==Dimension.Y? delta : 0,
							registeredDeltaInputDimension==Dimension.Z? delta : 0
						));
					}
				}
			}
		}

		/**
			Sets the position of the object to its position at the start of the
			scene (local space, not world space).
		*/
		public void SetInitialPosition()
		{
			if (animationInProgress==null || animationInProgress.Property != Property.Position)
				transform.localPosition = initialPosition;
		}
		
		/**
			Sets the scale of the object to its scale at the start of the scene
			(local space, not world space).
		*/
		public void SetInitialScale() =>
			transform.localScale = initialScale;
		
		/**
			Sets the rotation of the object to its rotation at the start of the
			scene (local space, not world space).
		*/
		public void SetInitialRotation()
		{
			if (animationInProgress==null || animationInProgress.Property != Property.Rotation)
				transform.localRotation = initialRotation;
		}
		
		/**
			Sets the position of the object to the position of another Transform
			(local space, not world space).
		*/
		public void SetPositionFromTransform(Transform reference)
		{
			if (animationInProgress==null || animationInProgress.Property != Property.Position)
				transform.localPosition = reference.localPosition;
		}
		
		/**
			Sets the scale of the object to the scale of another Transform
			(local space, not world space).
		*/
		public void SetScaleFromTransform(Transform reference) =>
			transform.localScale = reference.localScale;
		
		/**
			Sets the rotation of the object to the rotation of another Transform
			(local space, not world space).
		*/
		public void SetRotationFromTransform(Transform reference)
		{
			if (animationInProgress==null || animationInProgress.Property != Property.Rotation)
				transform.localRotation = reference.localRotation;
		}
		
		/**
			Changes the parent of this GameObject while allowing the world
			position to change. This this will preserve the local position of
			the object.
		*/
		public void SetParentChangeWorldPosition(Transform newParent)
		{
			transform.SetParent(newParent, false);
		}

		/**
			Start an animation to move the transform delta meters along the
			X axis, positively or negatively.
		*/
		public void MoveX(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Position,
					Dimension.X,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		/**
			Start an animation to move the transform delta meters along the
			Y axis, positively or negatively.
		*/
		public void MoveY(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Position,
					Dimension.Y,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		/**
			Start an animation to move the transform delta meters along the
			Z axis, positively or negatively.
		*/
		public void MoveZ(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Position,
					Dimension.Z,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		/**
			Start an animation to rotate the transform delta degrees around the
			X axis, positively or negatively.
		*/
		public void RotateX(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Rotation,
					Dimension.X,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		/**
			Start an animation to rotate the transform delta degrees around the
			Y axis, positively or negatively.
		*/
		public void RotateY(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Rotation,
					Dimension.Y,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		/**
			Start an animation to rotate the transform delta degrees around the
			Z axis, positively or negatively.
		*/
		public void RotateZ(float delta)
		{
			if (animationInProgress == null)
			{
				animationInProgress = new Animation
				(
					gameObject.transform,
					Property.Rotation,
					Dimension.Z,
					delta,
					AnimationDuration,
					EaseInAndOutAnimation
				);
			}
		}

		// Internal class used to keep track of animation state.
		private sealed class Animation
		{
			public readonly Transform Transform;
			public readonly Property Property;
			public readonly Dimension Dimension;
			public readonly Vector3 VectorStart;
			public readonly float PropertyValueEnd;
			public readonly float Duration;
			public readonly bool Ease;

			private float TimeElapsed = 0f;
			private bool AnimationComplete = false;

			public float PropertyValueStart
			{
				get
				{
					if (Dimension == Dimension.X) return VectorStart.x;
					if (Dimension == Dimension.Y) return VectorStart.y;
					if (Dimension == Dimension.Z) return VectorStart.z;

					return Mathf.NegativeInfinity;
				}
			}

			public Animation
			(
				Transform transform,
				Property property,
				Dimension dimension,
				float propertyValueDelta,
				float duration,
				bool ease
			)
			{
				if (transform == null)
					throw new ArgumentNullException("transform");
				
				if (propertyValueDelta <= 0)
					throw new InvalidOperationException("Property Value Delta must be greater than zero");
				
				if (duration <= 0)
					throw new InvalidOperationException("Duration must be greater than zero");
				
				this.Transform = transform;
				this.Property = property;
				this.Dimension = dimension;
				this.Duration = duration;
				this.Ease = ease;

				if (property == Property.Position) VectorStart = transform.localPosition;
				if (property == Property.Rotation) VectorStart = transform.localEulerAngles;

				if (dimension == Dimension.X) PropertyValueEnd = VectorStart.x + propertyValueDelta;
				if (dimension == Dimension.Y) PropertyValueEnd = VectorStart.y + propertyValueDelta;
				if (dimension == Dimension.Z) PropertyValueEnd = VectorStart.z + propertyValueDelta;
			}

			// Play one more step of the animation, based on how much time has
			// passed since the last increment.
			public bool Increment(float deltaTime)
			{
				if (AnimationComplete)
					throw new InvalidOperationException("Animation is already complete");
				
				if (deltaTime < 0)
					throw new InvalidOperationException("Delta Time cannot be less than zero");
				
				float propertyValue;

				if (TimeElapsed + deltaTime >= Duration)
				{
					AnimationComplete = true;

					TimeElapsed = Duration;
					
					propertyValue = PropertyValueEnd;
				}

				else
				{
					TimeElapsed += deltaTime;

					if (Ease)
						propertyValue =
						(
							0.5f * (PropertyValueEnd - PropertyValueStart) *
							(
								- Mathf.Cos
								(
									TimeElapsed *
									Mathf.PI /
									Duration
								) + 1
							)
						) + PropertyValueStart;
					
					else
						propertyValue = (PropertyValueEnd - PropertyValueStart) * (TimeElapsed / Duration);
				}

				Vector3 propertyVector = VectorStart;

				if (Dimension == Dimension.X) propertyVector.x = propertyValue;
				if (Dimension == Dimension.Y) propertyVector.y = propertyValue;
				if (Dimension == Dimension.Z) propertyVector.z = propertyValue;

				if (Property == Property.Position) Transform.localPosition = propertyVector;
				if (Property == Property.Rotation) Transform.localEulerAngles = propertyVector;

				return AnimationComplete;
			}
		}
	}
}