using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/StatsEditorWindow")]
    public static void ShowExample()
    {
        StatsEditorWindow wnd = GetWindow<StatsEditorWindow>();
        wnd.titleContent = new GUIContent("StatsEditorWindow");
    }

    public void CreateGUI()
    {
        // Load the UXML file
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/StatsEditorWindow.uxml");
      
        
    }    
    
}
