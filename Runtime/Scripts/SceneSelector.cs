using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CREATIVE.Utility
{
	/**
		A script used to load different scenes.

		If LoadSceneImmediately is checked on the Awake call, the script checks
		for a --scene flag on the command line (followed by the name of a scene)
		and loads it.

		If LoadSceneImmediately is checked, but a --scene flag is not found,
		The scene with the name stored in DefaultScene is loaded.

		If LoadSceneImmediately is not checked, nothing will happen on Awake,
		and the script will wait for LoadScene to be called.
	*/
	public class SceneSelector : MonoBehaviour
	{
		public bool LoadSceneImmediately = false;
		
		/**
			The name of the scene to open if no scene is specified.
		*/
		public string DefaultScene;
		
		void Awake()
		{
			if (LoadSceneImmediately)
			{
				string[] args = System.Environment.GetCommandLineArgs();
				
				bool loaded = false;
				for (int i=0; i<args.Length; i++)
				{
					if (args[i].Equals("--scene"))
					{
						if ((i+1)==args.Length || args[i+1].StartsWith("-"))
							throw new InvalidOperationException
								("\"scene\" flag was provided on the command line, but no scene was specified.");
						
						loaded = true;
						
						if (args[i+1].Equals(SceneManager.GetActiveScene().name))
							SceneManager.LoadScene(DefaultScene);
						else
							SceneManager.LoadScene(args[i+1]);
					}
				}

				if (loaded == false)
					SceneManager.LoadScene(DefaultScene);
			}
		}

		public void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}
