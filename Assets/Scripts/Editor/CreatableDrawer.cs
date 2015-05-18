using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(Creatable))]
class CreatableDrawer : PropertyDrawer {

	// Draw the property inside the given rect
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty (position, label, property);

		// Draw label
		//position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
		position = EditorGUI.IndentedRect(position);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		// Calculate rects
		Rect prefabRect = new Rect (position.x, position.y, 140, position.height);
		Rect minRect = new Rect (position.x+150, position.y, 35, position.height);
		Rect maxRect = new Rect (position.x+190, position.y, position.width-190, position.height);

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField (prefabRect, property.FindPropertyRelative ("Prefab"), GUIContent.none);
		EditorGUI.PropertyField (minRect, property.FindPropertyRelative ("Min"), GUIContent.none);
		EditorGUI.PropertyField (maxRect, property.FindPropertyRelative ("Max"), GUIContent.none);
		
		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty ();
	}

}
