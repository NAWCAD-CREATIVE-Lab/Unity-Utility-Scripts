using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CREATIVE.Utility
{
	/**
		A monobehaviour script for adding a text field to an object

		Useful for explaining an object with a lot of UnityEvent calls
	*/
	public class Comment: MonoBehaviour
	{
		[TextArea]
		[field: SerializeField]
		private string CommentText;

#if UNITY_EDITOR
		[CustomEditor(typeof(Comment))]
		public class Editor : UnityEditor.Editor
		{
			private bool editMode = true;
			
			public override void OnInspectorGUI()
			{
				if (editMode)
				{
					serializedObject.Update();
					EditorGUILayout.PropertyField
					(
						serializedObject.FindProperty(nameof(Comment.CommentText)),
						GUIContent.none,
						GUILayout.Height(100)
					);
					serializedObject.ApplyModifiedProperties();

					if (GUILayout.Button("Save"))
						editMode = false;
				}

				else
				{
					EditorGUI.BeginDisabledGroup(true);

					EditorGUILayout.TextArea
					(
						(target as Comment).CommentText,
						new GUIStyle(){ wordWrap = true }
					);

					EditorGUI.EndDisabledGroup();

					if (GUILayout.Button("Edit"))
						editMode = true;
				}
			}
		}
#endif
	}
}