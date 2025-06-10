using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CurrentEnemies))]
public class CurrentEnemiesEditor : Editor
{
    private Vector2 scrollPosition;
    private bool showActiveEnemies = true;
    private bool showBackupEnemies = true;
    private bool showAllEnemies = true;

    public override void OnInspectorGUI()
    {
        CurrentEnemies currentEnemies = (CurrentEnemies)target;
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Current Enemies Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Draw the serialized lists
        SerializedProperty activeEnemyGameObjects = serializedObject.FindProperty("activeEnemyGameObjects");
        SerializedProperty backupEnemyGameObjects = serializedObject.FindProperty("backupEnemyGameObjects");

        EditorGUILayout.PropertyField(activeEnemyGameObjects, new GUIContent("Active Enemies"), true);
        EditorGUILayout.PropertyField(backupEnemyGameObjects, new GUIContent("Backup Enemies"), true);

        EditorGUILayout.Space(10);

        // Active Enemy Section
        showActiveEnemies = EditorGUILayout.Foldout(showActiveEnemies, "Active Enemy Details", true);
        if (showActiveEnemies)
        {
            EditorGUI.indentLevel++;
            var activeEnemy = currentEnemies.ActiveEnemyGameObject;
            if (activeEnemy != null)
            {
                EditorGUILayout.ObjectField("Active Enemy", activeEnemy, typeof(GameObject), true);
                var activeData = currentEnemies.ActiveEnemyData;
                if (activeData != null)
                {
                    EditorGUILayout.LabelField("Name", activeData.enemyName);
                    EditorGUILayout.LabelField("Types", string.Join(", ", activeData.types.ConvertAll(t => t.typeName)));
                }
            }
            else
            {
                EditorGUILayout.LabelField("No active enemy");
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(5);

        // Backup Enemies Section
        showBackupEnemies = EditorGUILayout.Foldout(showBackupEnemies, "Backup Enemies Details", true);
        if (showBackupEnemies)
        {
            EditorGUI.indentLevel++;
            var backupEnemies = currentEnemies.BackupEnemyGameObjects;
            if (backupEnemies.Count > 0)
            {
                for (int i = 0; i < backupEnemies.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField($"Backup {i + 1}", backupEnemies[i], typeof(GameObject), true);
                    if (GUILayout.Button("Promote", GUILayout.Width(60)))
                    {
                        // Remove from backup and add to active
                        currentEnemies.RemoveEnemyGameObject(backupEnemies[i]);
                        currentEnemies.AddEnemyGameObject(backupEnemies[i]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No backup enemies");
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(5);

        // All Enemies Section
        showAllEnemies = EditorGUILayout.Foldout(showAllEnemies, "All Enemies", true);
        if (showAllEnemies)
        {
            EditorGUI.indentLevel++;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            var allEnemies = currentEnemies.GetEnemyGameObjects();
            for (int i = 0; i < allEnemies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField($"Enemy {i + 1}", allEnemies[i], typeof(GameObject), true);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    currentEnemies.RemoveEnemyGameObject(allEnemies[i]);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);

        // Clear All Button
        if (GUILayout.Button("Clear All Enemies"))
        {
            if (EditorUtility.DisplayDialog("Clear All Enemies",
                "Are you sure you want to clear all enemies?",
                "Yes", "No"))
            {
                currentEnemies.ClearAllEnemies();
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