/*
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeController))]
public class NodeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NodeController myNode = (NodeController)target;

        myNode._activateEnemies = EditorGUILayout.Toggle("Enemy Event?", myNode._activateEnemies);

        if(myNode._activateEnemies)
        {
            SerializedProperty listProperty = serializedObject.FindProperty("_enemyList");
            EditorGUILayout.PropertyField(listProperty, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
*/
