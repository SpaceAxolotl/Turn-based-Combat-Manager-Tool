using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AllySelector))]
public class AllySelectorEditor : Editor
{
    private SerializedProperty allyLibraryProp;
    private SerializedProperty selectedAllyProp;
    private string[] allyNames;
    private int selectedIndex;

    private void OnEnable()
    {
        allyLibraryProp = serializedObject.FindProperty("allyLibrary");
        selectedAllyProp = serializedObject.FindProperty("selectedAlly");
        
        UpdateAllyNames();
    }

    private void UpdateAllyNames()
    {
        if (allyLibraryProp == null) return;

        AllyLibrary library = allyLibraryProp.objectReferenceValue as AllyLibrary;
        if (library != null && library.allies != null)
        {
            allyNames = new string[library.allies.Count];
            for (int i = 0; i < library.allies.Count; i++)
            {
                allyNames[i] = library.allies[i] != null ? library.allies[i].allyName : "Null Ally";
            }

            // Find current selection
            selectedIndex = 0;
            if (selectedAllyProp != null && selectedAllyProp.objectReferenceValue != null)
            {
                AllyData currentAlly = selectedAllyProp.objectReferenceValue as AllyData;
                selectedIndex = library.allies.FindIndex(a => a == currentAlly);
                if (selectedIndex == -1) selectedIndex = 0;
            }
        }
        else
        {
            allyNames = new string[] { "No AllyLibrary assigned" };
            selectedIndex = 0;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(allyLibraryProp);

        EditorGUILayout.Space();

        // Update names if library changed
        if (GUI.changed)
        {
            UpdateAllyNames();
        }

        // Draw dropdown
        EditorGUI.BeginChangeCheck();
        selectedIndex = EditorGUILayout.Popup("Select Ally", selectedIndex, allyNames);
        if (EditorGUI.EndChangeCheck() && allyLibraryProp.objectReferenceValue != null)
        {
            AllyLibrary library = allyLibraryProp.objectReferenceValue as AllyLibrary;
            if (library != null && library.allies != null && selectedIndex < library.allies.Count)
            {
                selectedAllyProp.objectReferenceValue = library.allies[selectedIndex];
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
} 