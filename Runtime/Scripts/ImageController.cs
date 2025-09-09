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

		void Start() => GetComponent<Image>().alphaHitTestMinimumThreshold = NewAlphaHitTestMinimumThreshold;
	}
}
