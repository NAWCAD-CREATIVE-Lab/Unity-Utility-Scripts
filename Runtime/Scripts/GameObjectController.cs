// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using UnityEngine;

namespace CREATIVE.Utility
{
	public class GameObjectController : MonoBehaviour
	{
		[field: SerializeField]
		private bool EnabledByDefault = true;

		void Start()
		{
			gameObject.SetActive(EnabledByDefault);
		}
		
		public void ToggleEnabled()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}
	}
}
