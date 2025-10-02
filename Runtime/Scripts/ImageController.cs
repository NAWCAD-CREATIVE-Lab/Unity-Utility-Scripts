// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using UnityEngine;
using UnityEngine.UI;

namespace CREATIVE.Utility
{
	[RequireComponent(typeof(Image))]
	public class ImageController : MonoBehaviour
	{
		[field: SerializeField]
		bool OverrideAlphaHitTestMinimumThresholdOnStart = false;

		[field: SerializeField]
		float NewAlphaHitTestMinimumThreshold = 0.0f;

		void Start()
		{
			if (OverrideAlphaHitTestMinimumThresholdOnStart)
				GetComponent<Image>().alphaHitTestMinimumThreshold = NewAlphaHitTestMinimumThreshold;
		}
	}
}
