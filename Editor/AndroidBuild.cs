using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace CREATIVE.UtilityEditor
{
	/**
		A class used for building an Android APK on the commandline
	*/
	public class AndroidBuild
	{
		/**
			Builds an android APK.

			This method should only be called by using the -executeMethod flag when
			opening the editor on the commandline. The path to the APK should be
			supplied after the method name, i.e.:

			-executeMethod AndroidBuild.Build .\Build\myAndroidApp.apk 

			Documentation for the Unity Editor CLI can be found here:

			https://docs.unity3d.com/Manual/EditorCommandLineArguments.html
		*/
		public static void Build()
		{
			String[] cmdArgs = Environment.GetCommandLineArgs();
			
			string androidBuildPath = null;
			for (int i=0; i<cmdArgs.Length; i++)
			{
				if (cmdArgs[i].Equals("-executeMethod") && (i+2)<cmdArgs.Length)
				{
					androidBuildPath = cmdArgs[i+2];
					i+=2;
				}
			}
			
			if (androidBuildPath == null)
				throw new InvalidOperationException
				(
					"Android Build method was called," +
					"but a path for an APK was not specified on the command line."
				);
			
			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
			buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
			buildPlayerOptions.locationPathName = androidBuildPath;
			buildPlayerOptions.target = BuildTarget.Android;
			buildPlayerOptions.options = BuildOptions.None;
			BuildPipeline.BuildPlayer(buildPlayerOptions);
		}
	}
}