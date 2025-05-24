using UnityEditor;
using System;
using System.Reflection;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButton))]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    private SerializedProperty clickAudioResId;
    protected override void OnEnable()
    {
        base.OnEnable();
        clickAudioResId = serializedObject.FindProperty("clickAudioResId");
    }
    public override void OnInspectorGUI()
    { 
        EditorGUILayout.PropertyField(clickAudioResId);
        serializedObject.ApplyModifiedProperties();
        
        base.OnInspectorGUI();
      
    }
}