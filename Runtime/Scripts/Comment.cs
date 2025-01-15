using UnityEngine;

namespace CREATIVE.Utility
{
	/**
		A monobehaviour script for adding a text field to an object

		Useful for explaining an object with a lot of UnityEvent calls
	*/
	public class Comment: MonoBehaviour
	{
		[TextArea]
		public string CommentText;
	}
}