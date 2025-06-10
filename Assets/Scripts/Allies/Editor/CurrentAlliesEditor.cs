using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CurrentAllies))]
public class CurrentAlliesEditor : Editor
{
    private Vector2 scrollPosition;
    private bool showActiveAllies = true;
    private bool showBackupAllies = true;
    private bool showAllAllies = true;

    public override void OnInspectorGUI()
    {
        CurrentAllies currentAllies = (CurrentAllies)target;
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Current Allies Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Draw the serialized lists
        SerializedProperty activeAllyGameObjects = serializedObject.FindProperty("activeAllyGameObjects");
        SerializedProperty backupAllyGameObjects = serializedObject.FindProperty("backupAllyGameObjects");

        EditorGUILayout.PropertyField(activeAllyGameObjects, new GUIContent("Active Allies"), true);
        EditorGUILayout.PropertyField(backupAllyGameObjects, new GUIContent("Backup Allies"), true);

        EditorGUILayout.Space(10);

        // Active Ally Section
        showActiveAllies = EditorGUILayout.Foldout(showActiveAllies, "Active Ally Details", true);
        if (showActiveAllies)
        {
            EditorGUI.indentLevel++;
            var activeAlly = currentAllies.ActiveAllyGameObject;
            if (activeAlly != null)
            {
                EditorGUILayout.ObjectField("Active Ally", activeAlly, typeof(GameObject), true);
                var activeData = currentAllies.ActiveAllyData;
                if (activeData != null)
                {
                    EditorGUILayout.LabelField("Name", activeData.allyName);
                    EditorGUILayout.LabelField("Types", string.Join(", ", activeData.types.ConvertAll(t => t.typeName)));
                }
            }
            else
            {
                EditorGUILayout.LabelField("No active ally");
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(5);

        // Backup Allies Section
        showBackupAllies = EditorGUILayout.Foldout(showBackupAllies, "Backup Allies Details", true);
        if (showBackupAllies)
        {
            EditorGUI.indentLevel++;
            var backupAllies = currentAllies.BackupAllyGameObjects;
            if (backupAllies.Count > 0)
            {
                for (int i = 0; i < backupAllies.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField($"Backup {i + 1}", backupAllies[i], typeof(GameObject), true);
                    if (GUILayout.Button("Promote", GUILayout.Width(60)))
                    {
                        // Remove from backup and add to active
                        currentAllies.RemoveAllyGameObject(backupAllies[i]);
                        currentAllies.AddAllyGameObject(backupAllies[i]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No backup allies");
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(5);

        // All Allies Section
        showAllAllies = EditorGUILayout.Foldout(showAllAllies, "All Allies", true);
        if (showAllAllies)
        {
            EditorGUI.indentLevel++;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            var allAllies = currentAllies.GetAllyGameObjects();
            for (int i = 0; i < allAllies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField($"Ally {i + 1}", allAllies[i], typeof(GameObject), true);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    currentAllies.RemoveAllyGameObject(allAllies[i]);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Clear All Button
        if (GUILayout.Button("Clear All Allies"))
        {
            if (EditorUtility.DisplayDialog("Clear All Allies",
                "Are you sure you want to clear all allies?",
                "Yes", "No"))
            {
                currentAllies.ClearAllAllies();
            }
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
} 