using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TypeEditorWindow : EditorWindow
{
    private TypeLibrary typeLibrary;

    [MenuItem("Tools/Type Editor")]
    public static void OpenWindow()
    {
        GetWindow<TypeEditorWindow>("Type Editor");
    }

    private Vector2 scroll;

    private void OnGUI()
    {
        typeLibrary = (TypeLibrary)EditorGUILayout.ObjectField("Type Library", typeLibrary, typeof(TypeLibrary), false);

        if (typeLibrary == null)
        {
            EditorGUILayout.HelpBox("Assign a TypeLibrary asset to begin editing types.", MessageType.Info);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var type in typeLibrary.allTypes)
        {
            DrawType(type);
            EditorGUILayout.Space(20);
        }

        if (GUILayout.Button("+ Add New Type"))
        {
            CreateNewType();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawType(TypeDefinition type)
    {
        if (type == null) return;

        EditorGUILayout.BeginVertical("box");
        type.typeName = EditorGUILayout.TextField("Name", type.typeName);
        EditorGUILayout.LabelField("Description");
        type.description = EditorGUILayout.TextArea(type.description);

        DrawTypeList("Offensive Strengths", type.offensiveStrengths);
        DrawTypeList("Offensive Weaknesses", type.offensiveWeaknesses);
        DrawTypeList("Defensive Strengths", type.defensiveStrengths);
        DrawTypeList("Defensive Weaknesses", type.defensiveWeaknesses);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(type);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawTypeList(string label, List<TypeDefinition> list)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        if (list == null) return;

        int removeIndex = -1;
        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            list[i] = (TypeDefinition)EditorGUILayout.ObjectField(list[i], typeof(TypeDefinition), false);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                removeIndex = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex >= 0)
            list.RemoveAt(removeIndex);

        if (GUILayout.Button("+ Add Type to " + label))
        {
            list.Add(null);
        }
    }

    private void CreateNewType()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create New Type", "NewType", "asset", "Enter a name for the new type.");
        if (!string.IsNullOrEmpty(path))
        {
            TypeDefinition newType = ScriptableObject.CreateInstance<TypeDefinition>();
            AssetDatabase.CreateAsset(newType, path);
            AssetDatabase.SaveAssets();
            typeLibrary.allTypes.Add(newType);
            EditorUtility.SetDirty(typeLibrary);
        }
    }
}