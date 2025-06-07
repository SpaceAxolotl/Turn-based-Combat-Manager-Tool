using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*public class MoveEditorWindow : EditorWindow
{
    private MoveLibrary moveLibrary;
    private StatLibrary statLibrary;
    private TypeLibrary typeLibrary;
    private ResourceLibrary resourceLibrary;

    private Vector2 scroll;

    [MenuItem("Tools/Move Editor")]
    public static void OpenWindow()
    {
        GetWindow<MoveEditorWindow>("Move Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Move Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        moveLibrary = (MoveLibrary)EditorGUILayout.ObjectField("Move Library", moveLibrary, typeof(MoveLibrary), false);
        statLibrary = (StatLibrary)EditorGUILayout.ObjectField("Stat Library", statLibrary, typeof(StatLibrary), false);
        typeLibrary = (TypeLibrary)EditorGUILayout.ObjectField("Type Library", typeLibrary, typeof(TypeLibrary), false);
        resourceLibrary = (ResourceLibrary)EditorGUILayout.ObjectField("Resource Library", resourceLibrary, typeof(ResourceLibrary), false);

        if (moveLibrary == null || statLibrary == null || typeLibrary == null || resourceLibrary == null)
        {
            EditorGUILayout.HelpBox("Please assign all four libraries to begin.", MessageType.Info);
            return;
        }

        if (moveLibrary.moves == null)
            moveLibrary.moves = new List<MoveData>();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < moveLibrary.moves.Count; i++)
        {
            var move = moveLibrary.moves[i];
            if (move == null) continue;

            EditorGUILayout.BeginVertical("box");
            DrawMove(move);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X", GUILayout.Width(24)))
            {
                moveLibrary.moves.RemoveAt(i);
                EditorUtility.SetDirty(moveLibrary);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        if (GUILayout.Button("+ Create New Move"))
        {
            CreateNewMove();
        }
    }

    private void DrawMove(MoveData move)
    {
        EditorGUI.BeginChangeCheck();

        move.moveName = EditorGUILayout.TextField("Name", move.moveName);
        EditorGUILayout.LabelField("Description");
        move.description = EditorGUILayout.TextArea(move.description);
        move.power = EditorGUILayout.IntField("Power", move.power);
        move.accuracy = EditorGUILayout.IntField("Accuracy", move.accuracy);

        // Type dropdown
        int typeIndex = typeLibrary.allTypes.IndexOf(move.type);
        typeIndex = EditorGUILayout.Popup("Type", typeIndex, typeLibrary.allTypes.Select(t => t.typeName).ToArray());
        move.type = typeIndex >= 0 ? typeLibrary.allTypes[typeIndex] : null;

        // Uses Resource
        move.usesResource = EditorGUILayout.Toggle("Uses Resource", move.usesResource);
        if (move.usesResource)
        {
            move.resourceCost = EditorGUILayout.IntField("Resource Cost", move.resourceCost);

            int resIndex = resourceLibrary.resourceDefinitions.IndexOf(move.resourceType);
            resIndex = EditorGUILayout.Popup("Resource Type", resIndex, resourceLibrary.resourceDefinitions.Select(r => r.resourceName).ToArray());
            move.resourceType = resIndex >= 0 ? resourceLibrary.resourceDefinitions[resIndex] : null;
        }

        // Uses Attack Stat
        move.usesAttackStat = EditorGUILayout.Toggle("Uses Attack Stat", move.usesAttackStat);
        if (move.usesAttackStat)
        {
            int statIndex = statLibrary.statDefinitions.IndexOf(move.attackStat);
            statIndex = EditorGUILayout.Popup("Attack Stat", statIndex, statLibrary.statDefinitions.Select(s => s.statName).ToArray());
            move.attackStat = statIndex >= 0 ? statLibrary.statDefinitions[statIndex] : null;
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(move);
        }
    }

    private void CreateNewMove()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create New Move", "NewMove", "asset", "Enter a name for the new move.");
        if (!string.IsNullOrEmpty(path))
        {
            var newMove = ScriptableObject.CreateInstance<MoveData>();
            AssetDatabase.CreateAsset(newMove, path);
            AssetDatabase.SaveAssets();
            moveLibrary.moves.Add(newMove);
            EditorUtility.SetDirty(moveLibrary);
        }
    }
}*/
