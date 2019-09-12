
using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;


/// <summary>
/// Is being used for the GameMode list so that handling the list is much easier.
/// </summary>
public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        GameManager obj = EditorUtility.InstanceIDToObject(instanceId) as GameManager;
        if (obj != null)
        {
            GameManagerEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            GameManagerEditorWindow.Open((GameManager)target);
        }
    }
}
