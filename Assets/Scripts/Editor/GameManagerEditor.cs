using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ChineseCheckers;
using UnityEditor;
using UnityEditor.Callbacks;
public class AssetHandler{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line){
        GameManager obj = EditorUtility.InstanceIDToObject(instanceId) as GameManager;
        if(obj != null){
            GameManagerEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        if(GUILayout.Button("Open Editor")){
            GameManagerEditorWindow.Open((GameManager)target);
        }
    }
}
