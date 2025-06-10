using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySelector))]
public class EnemySelectorEditor : Editor
{
    private SerializedProperty enemyLibraryProp;
    private SerializedProperty selectedEnemyProp;
    private string[] enemyNames;
    private int selectedIndex;

    private void OnEnable()
    {
        enemyLibraryProp = serializedObject.FindProperty("enemyLibrary");
        selectedEnemyProp = serializedObject.FindProperty("selectedEnemy");
        
        UpdateEnemyNames();
    }

    private void UpdateEnemyNames()
    {
        if (enemyLibraryProp == null) return;

        EnemyLibrary library = enemyLibraryProp.objectReferenceValue as EnemyLibrary;
        if (library != null && library.enemies != null)
        {
            enemyNames = new string[library.enemies.Count];
            for (int i = 0; i < library.enemies.Count; i++)
            {
                enemyNames[i] = library.enemies[i] != null ? library.enemies[i].enemyName : "Null Enemy";
            }

            // Find current selection
            selectedIndex = 0;
            if (selectedEnemyProp != null && selectedEnemyProp.objectReferenceValue != null)
            {
                EnemyData currentEnemy = selectedEnemyProp.objectReferenceValue as EnemyData;
                selectedIndex = library.enemies.FindIndex(e => e == currentEnemy);
                if (selectedIndex == -1) selectedIndex = 0;
            }
        }
        else
        {
            enemyNames = new string[] { "No EnemyLibrary assigned" };
            selectedIndex = 0;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(enemyLibraryProp);

        EditorGUILayout.Space();

        // Update names if library changed
        if (GUI.changed)
        {
            UpdateEnemyNames();
        }

        // Draw dropdown
        EditorGUI.BeginChangeCheck();
        selectedIndex = EditorGUILayout.Popup("Select Enemy", selectedIndex, enemyNames);
        if (EditorGUI.EndChangeCheck() && enemyLibraryProp.objectReferenceValue != null)
        {
            EnemyLibrary library = enemyLibraryProp.objectReferenceValue as EnemyLibrary;
            if (library != null && library.enemies != null && selectedIndex < library.enemies.Count)
            {
                selectedEnemyProp.objectReferenceValue = library.enemies[selectedIndex];
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
} 