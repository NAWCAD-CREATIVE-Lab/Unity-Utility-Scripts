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

		[field: SerializeField]
		[field: HideInInspector]
		private bool editMode;

#if UNITY_EDITOR
		[CustomEditor(typeof(Comment))]
		public class Editor : UnityEditor.Editor
		{
			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				SerializedProperty commentProperty = serializedObject.FindProperty(nameof(Comment.CommentText));

				SerializedProperty editModeProperty = serializedObject.FindProperty(nameof(Comment.editMode));
				
				if (editModeProperty.boolValue)
				{
					EditorGUILayout.PropertyField
					(
						commentProperty,
						GUIContent.none,
						GUILayout.Height(100)
					);

					EditorGUILayout.Space(20);

					if (GUILayout.Button("Save"))
						editModeProperty.boolValue = false;
				}

				else
				{
					GUIStyle labelStyle = EditorStyles.label;

					labelStyle.wordWrap = true;
					
					EditorGUILayout.LabelField
					(
						commentProperty.stringValue,
						labelStyle
					);

					EditorGUILayout.Space(20);

					if (GUILayout.Button("Edit"))
						editModeProperty.boolValue = true;
				}

				serializedObject.ApplyModifiedProperties();
			}
		}
#endif
	}
}