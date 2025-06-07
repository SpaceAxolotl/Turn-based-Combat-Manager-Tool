using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyEditorWindow : EditorWindow
{
    /*private EnemyLibrary enemyLibrary;
    private Vector2 scroll;

    [MenuItem("Tools/Enemy Editor")]
    public static void OpenWindow()
    {
        GetWindow<EnemyEditorWindow>("Enemy Editor");
    }

    private void OnGUI()
    {
        enemyLibrary = (EnemyLibrary)EditorGUILayout.ObjectField("Enemy Library", enemyLibrary, typeof(EnemyLibrary), false);

        if (enemyLibrary == null || enemyLibrary.statLibrary == null || enemyLibrary.typeLibrary == null)
        {
            EditorGUILayout.HelpBox("Assign a valid EnemyLibrary with both a StatLibrary and TypeLibrary to begin.", MessageType.Info);
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        if (enemyLibrary.enemies == null)
            enemyLibrary.enemies = new List<EnemyData>();

        for (int e = 0; e < enemyLibrary.enemies.Count; e++)
        {
            var enemy = enemyLibrary.enemies[e];
            if (enemy == null) continue;

            EditorGUILayout.BeginVertical("box");
            enemy.enemyName = EditorGUILayout.TextField("Enemy Name", enemy.enemyName);

            DrawEnemyTypes(enemy);
            SyncStats(enemy, enemyLibrary.statLibrary);

            // Stats
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            foreach (var stat in enemy.stats)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(stat.statDefinition.statName, GUILayout.Width(150));
                stat.value = EditorGUILayout.IntField(stat.value);
                EditorGUILayout.EndHorizontal();
            }

            // Moves
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Moves", EditorStyles.boldLabel);

            if (enemy.moves == null)
                enemy.moves = new List<MoveData>();

            for (int i = 0; i < enemy.moves.Count; i++)
            {
                MoveData move = enemy.moves[i];

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginHorizontal();
                enemy.moves[i] = (MoveData)EditorGUILayout.ObjectField(move, typeof(MoveData), false);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    enemy.moves.RemoveAt(i);
                    i--;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    continue;
                }
                EditorGUILayout.EndHorizontal();

                if (move != null)
                {
                    EditorGUI.BeginChangeCheck();

                    move.moveName = EditorGUILayout.TextField("Name", move.moveName);
                    move.description = EditorGUILayout.TextField("Description",move.description);
                    move.power = EditorGUILayout.IntField("Power", move.power);
                    move.accuracy = EditorGUILayout.IntField("Accuracy", move.accuracy);

                    int typeIndex = enemyLibrary.typeLibrary.allTypes.IndexOf(move.type);
                    typeIndex = EditorGUILayout.Popup("Type", typeIndex,
                        enemyLibrary.typeLibrary.allTypes.ConvertAll(t => t.typeName).ToArray());
                    move.type = typeIndex >= 0 ? enemyLibrary.typeLibrary.allTypes[typeIndex] : null;

                    int statIndex = enemyLibrary.statLibrary.statDefinitions.IndexOf(move.attackStat);
                    statIndex = EditorGUILayout.Popup("Uses Stat", statIndex,
                        enemyLibrary.statLibrary.statDefinitions.ConvertAll(s => s.statName).ToArray());
                    move.attackStat = statIndex >= 0 ? enemyLibrary.statLibrary.statDefinitions[statIndex] : null;

                    if (EditorGUI.EndChangeCheck())
                        EditorUtility.SetDirty(move);
                }
                else
                {
                    EditorGUILayout.HelpBox("No Move assigned. You can assign an existing move asset.", MessageType.Info);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            if (GUILayout.Button("+ Add Move Slot"))
            {
                enemy.moves.Add(null);
                EditorUtility.SetDirty(enemy);
            }

            EditorUtility.SetDirty(enemy);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        if (GUILayout.Button("+ Create New Enemy"))
        {
            CreateNewEnemy();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawEnemyTypes(EnemyData enemy)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Types", EditorStyles.boldLabel);

        if (enemy.types == null)
            enemy.types = new List<TypeDefinition>();

        for (int t = 0; t < enemy.types.Count; t++)
        {
            EditorGUILayout.BeginHorizontal();
            enemy.types[t] = (TypeDefinition)EditorGUILayout.ObjectField(enemy.types[t], typeof(TypeDefinition), false);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                enemy.types.RemoveAt(t);
                t--;
                EditorGUILayout.EndHorizontal();
                continue;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (enemyLibrary.typeLibrary.allTypes != null && enemyLibrary.typeLibrary.allTypes.Count > 0)
        {
            var typeNames = enemyLibrary.typeLibrary.allTypes.ConvertAll(t => t.typeName).ToArray();
            int addIndex = EditorGUILayout.Popup("Add Type", -1, typeNames);
            if (addIndex >= 0)
            {
                var selected = enemyLibrary.typeLibrary.allTypes[addIndex];
                if (!enemy.types.Contains(selected))
                {
                    enemy.types.Add(selected);
                    EditorUtility.SetDirty(enemy);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("TypeLibrary is empty or missing types.", MessageType.Warning);
        }
    }

    private void SyncStats(EnemyData enemy, StatLibrary statLibrary)
    {
        if (enemy.stats == null)
            enemy.stats = new List<EnemyStat>();

        foreach (var def in statLibrary.statDefinitions)
        {
            if (!enemy.stats.Exists(s => s.statDefinition == def))
            {
                enemy.stats.Add(new EnemyStat
                {
                    statDefinition = def,
                    value = def.defaultValue
                });
            }
        }

        enemy.stats.RemoveAll(s => !statLibrary.statDefinitions.Contains(s.statDefinition));
    }

    private void CreateNewEnemy()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create New Enemy", "NewEnemy", "asset", "Enter a name for the new enemy.");
        if (!string.IsNullOrEmpty(path))
        {
            var newEnemy = ScriptableObject.CreateInstance<EnemyData>();
            AssetDatabase.CreateAsset(newEnemy, path);
            AssetDatabase.SaveAssets();
            enemyLibrary.enemies.Add(newEnemy);
            EditorUtility.SetDirty(enemyLibrary);
        }
    }*/
}
