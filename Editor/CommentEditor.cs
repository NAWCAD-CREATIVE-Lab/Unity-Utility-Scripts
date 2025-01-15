using UnityEngine;
using UnityEditor;
using CREATIVE.Utility;

namespace CREATIVE.UtilityEditor
{
	[CustomEditor(typeof(Comment))]
	public class CommentEditor : UnityEditor.Editor
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
}