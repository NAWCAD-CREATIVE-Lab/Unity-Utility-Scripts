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
