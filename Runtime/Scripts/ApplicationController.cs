// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using UnityEngine;

namespace CREATIVE.Utility
{
	[CreateAssetMenu(fileName = "Application Controller", menuName = "NAWCAD CREATIVE Lab/Utilities/Application Controller")]
	public class ApplicationController : ScriptableObject
	{
		public void SetCursorVisible(bool visible)
		{
			Cursor.visible = visible;
		}

		public void LockCursor()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void ConfineCursor()
		{
			Cursor.lockState = CursorLockMode.Confined;
		}

		public void FreeCursor()
		{
			Cursor.lockState = CursorLockMode.None;
		}

		/**
			Quit the application, or exit play mode in the Unity editor.
		*/
		public void Quit()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			
			Application.Quit();
		}
	}
}