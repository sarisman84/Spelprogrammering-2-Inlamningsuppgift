
using UnityEngine;
using UnityEditor;


/// <summary>
/// Is being used for the GameMode list so that handling the list is much easier.
/// </summary>
public class GameManagerEditorWindow : ExtendedEditorWindow
{
    public static void Open(GameManager managerObject)
    {
        GameManagerEditorWindow window = GetWindow<GameManagerEditorWindow>("Game Manager Editor");
        window.serializedObject = new SerializedObject(managerObject);
        window.manager = managerObject;
    }
    GameManager manager;

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("modes");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSidebar(currentProperty);
        EditorGUILayout.Space();
        if (GUILayout.Button("Add new Element"))
        {
            manager.Add();
            serializedObject.Update();
        }
        if (GUILayout.Button("Remove latest Element"))
        {
            manager.Remove();
            serializedObject.Update();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (selectedProperty != null)
        {
            DrawProperties(selectedProperty, true);
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list ");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }
}
