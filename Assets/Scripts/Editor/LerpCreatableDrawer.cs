using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(LerpCreatable))]
class LerpCreatableDrawer : PropertyDrawer {
	
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
		Rect prefabRect = new Rect (position.x, position.y, 120, position.height);
		Rect initialMinRect = new Rect (position.x+125, position.y, 25, position.height);
		Rect endMinRect = new Rect (position.x+150, position.y, 25, position.height);
		Rect initialMaxRect = new Rect (position.x+175, position.y, 25, position.height);
		Rect endMaxRect = new Rect (position.x+200, position.y, position.width-200, position.height);
		
		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField (prefabRect, property.FindPropertyRelative ("Prefab"), GUIContent.none);
		EditorGUI.PropertyField (initialMinRect, property.FindPropertyRelative ("InitialMin"), GUIContent.none);
		EditorGUI.PropertyField (endMinRect, property.FindPropertyRelative ("EndMin"), GUIContent.none);
		EditorGUI.PropertyField (initialMaxRect, property.FindPropertyRelative ("InitialMax"), GUIContent.none);
		EditorGUI.PropertyField (endMaxRect, property.FindPropertyRelative ("EndMax"), GUIContent.none);
		
		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty ();
	}
	
}

