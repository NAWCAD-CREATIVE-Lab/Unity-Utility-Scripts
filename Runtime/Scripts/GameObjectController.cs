using UnityEngine;

namespace CREATIVE.Utility
{
	public class GameObjectController : MonoBehaviour
	{
		public bool EnabledByDefault = true;

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
