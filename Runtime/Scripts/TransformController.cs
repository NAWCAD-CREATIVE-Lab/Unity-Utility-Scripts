using System;
using UnityEngine;

namespace CREATIVE.Utility
{
	/**
		This class provides methods to modify and animate properties of a
		GameObject's Transform.
	*/
	[RequireComponent(typeof(Transform))]
	public class TransformController : MonoBehaviour
	{
		/**
			Slowly start and slowly stop animations.
		*/
		public bool EaseInAndOutAnimation = true;

		/**
			How long (in seconds) animations will last.
		*/
		[Range(1, 10)]
		public int AnimationDuration = 1;

		private enum Property { Position, Rotation }

		private enum Dimension { X, Y, Z }
		
		private Vector3 initialPosition;

		private Vector3 initialScale;

		private Quaternion initialRotation;

		private Animation animationInProgress = null;

		void Start()
		{
			initialPosition = transform.localPosition;
			
			initialScale = transform.localScale;

			initialRotation = transform.localRotation;
		}

		/**
			If an animation is in progress, run the next increment.
		*/
		void Update()
		{
			if (animationInProgress != null)
				if (animationInProgress.Increment(Time.deltaTime))
					animationInProgress = null;
		}

		/**
			Sets the position of the object to its position at the start of the
			scene (local space, not world space).
		*/
		public void SetInitialPosition() =>
			transform.localPosition = initialPosition;
		
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
		public void SetInitialRotation() =>
			transform.localRotation = initialRotation;
		
		/**
			Sets the position of the object to the position of another Transform
			(local space, not world space).
		*/
		public void SetPositionFromTransform(Transform reference) =>
			transform.localPosition = reference.localPosition;
		
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
		public void SetRotationFromTransform(Transform reference) =>
			transform.localRotation = reference.localRotation;
		
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
			private readonly Transform Transform;
			private readonly Property Property;
			private readonly Dimension Dimension;
			private readonly Vector3 VectorStart;
			private readonly float PropertyValueEnd;
			private readonly float Duration;
			private readonly bool Ease;

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