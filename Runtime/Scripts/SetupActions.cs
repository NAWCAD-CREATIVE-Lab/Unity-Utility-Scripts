// Copyright 2025 U.S. Federal Government (in countries where recognized)
// Copyright 2025 Dakota Crouchelli dakota.h.crouchelli.civ@us.navy.mil

using UnityEngine;
using UnityEngine.Events;

namespace CREATIVE.Utility
{
	/**
		This Monobehaviour allows UnityEvents to be invoked at the start of a
		scene.
		
		It also allows different UnityEvents to be invoked if the code is
		running:
			- In a development build
			- In a release build
			- In the editor
			- In an IOS platform
			- In an Android platform
	*/
	public class SetupActions : MonoBehaviour
	{
		[field: SerializeField] private UnityEvent SceneStartActions;
		[field: SerializeField] private UnityEvent DevelopmentBuildActions;
		[field: SerializeField] private UnityEvent ReleaseBuildActions;
		[field: SerializeField] private UnityEvent EditorActions;
		[field: SerializeField] private UnityEvent IOSActions;
		[field: SerializeField] private UnityEvent AndroidActions;
		
		void Start()
		{
			SceneStartActions.Invoke();
			
			if (Application.isEditor)
				EditorActions.Invoke();
			
			else if (Debug.isDebugBuild)
				DevelopmentBuildActions.Invoke();
			
			else if (!Debug.isDebugBuild)
				ReleaseBuildActions.Invoke();
			
#if UNITY_IOS
			IOSActions.Invoke();
#endif

#if UNITY_ANDROID
			AndroidActions.Invoke();
#endif
		}
	}
}