using UnityEditor;
using UnityEngine;

public class AllyEditorWindow : EditorWindow
{
    AllyData allyData;
    private AllyLibrary allyLibrary;
    private Vector2 scroll;

    [MenuItem("Tools/Ally Editor")]
    public static void ShowWindow()
    {
        GetWindow<AllyEditorWindow>("Ally Editor");
    }

    private void OnGUI()
    {
        allyLibrary = (AllyLibrary)EditorGUILayout.ObjectField("Ally Library", allyLibrary, typeof(AllyLibrary), false);

        if (allyLibrary == null || allyLibrary.statLibrary == null)
        {
            EditorGUILayout.HelpBox("Assign an AllyLibrary with a valid StatLibrary to begin.", MessageType.Info);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        if (allyLibrary.allies == null)
            allyLibrary.allies = new System.Collections.Generic.List<AllyData>();

        for (int e = 0; e < allyLibrary.allies.Count; e++)
        {
            var ally = allyLibrary.allies[e];
            if (ally == null) continue;

            EditorGUILayout.BeginVertical("box");
            ally.allyName = EditorGUILayout.TextField("Ally Name", ally.allyName);

            SyncStats(ally, allyLibrary.statLibrary);

            // Stats Section
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            foreach (var stat in ally.stats)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(stat.statDefinition.statName, GUILayout.Width(150));
                stat.value = EditorGUILayout.IntField(stat.value);
                EditorGUILayout.EndHorizontal();
            }

            // Moves Section
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Moves", EditorStyles.boldLabel);

            if (ally.moves == null)
                ally.moves = new System.Collections.Generic.List<MoveData>();

            for (int i = 0; i < ally.moves.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                ally.moves[i] = (MoveData)EditorGUILayout.ObjectField(ally.moves[i], typeof(MoveData), false);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    ally.moves.RemoveAt(i);
                    i--; // adjust index after removal
                    EditorGUILayout.EndHorizontal(); // ensure GUI ends properly
                    continue; // continue to avoid double-draw
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ Add Move"))
            {
                ally.moves.Add(null);
            }

            EditorUtility.SetDirty(ally);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        if (GUILayout.Button("+ Create New Ally"))
        {
            CreateNewAlly();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SyncStats(AllyData ally, StatLibrary statLibrary)
    {
        if (ally.stats == null)
            ally.stats = new System.Collections.Generic.List<AllyStat>();

        foreach (var def in statLibrary.statDefinitions)
        {
            if (!ally.stats.Exists(s => s.statDefinition == def))
            {
                ally.stats.Add(new AllyStat
                {
                    statDefinition = def,
                    value = def.defaultValue
                });
            }
        }

        // Remove any orphaned stats (no longer in the library)
        ally.stats.RemoveAll(s => !statLibrary.statDefinitions.Contains(s.statDefinition));
    }

    private void CreateNewAlly()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create New Ally", "NewAlly", "asset", "Enter a name for the new ally.");
        if (!string.IsNullOrEmpty(path))
        {
            var newAlly = ScriptableObject.CreateInstance<AllyData>();
            AssetDatabase.CreateAsset(newAlly, path);
            AssetDatabase.SaveAssets();
            allyLibrary.allies.Add(newAlly);
            EditorUtility.SetDirty(allyLibrary);
        }
    }
}
