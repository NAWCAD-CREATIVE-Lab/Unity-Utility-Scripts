using UnityEngine;

namespace CREATIVE.Utility
{
	/**
		A class that exposes the Debug.Log (and related) functionality in a
		MonoBehaviour so that it can be called by UnityEvents
	*/
	[CreateAssetMenu(fileName = "Debug Logger", menuName = "NAWCAD CREATIVE Lab/Utilities/Debug Logger")]
	public class DebugLogger : ScriptableObject
	{
		public void Log(string message)
		{
			Debug.Log(message);
		}

		public void LogWarning(string message)
		{
			Debug.LogWarning(message);
		}

		public void LogError(string message)
		{
			Debug.LogError(message);
		}
	}
}