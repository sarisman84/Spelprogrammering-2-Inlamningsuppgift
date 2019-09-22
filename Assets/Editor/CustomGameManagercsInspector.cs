using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GameManagercs))]
public class CustomGameManagercsInspector : Editor
{
    GameManagercs gm;

    private void OnEnable()
    {
        gm = target as GameManagercs;
    }
    List<Vector2> scrollPos = new List<Vector2>();
    public override void OnInspectorGUI()
    {
        

        for (int cnt = 0; cnt < gm.mode.Count; cnt++)
        {
            if (scrollPos.Count < gm.mode.Count)
            {
                scrollPos.Add(Vector2.zero);
            }
            if (scrollPos.Count > gm.mode.Count)
            {
                scrollPos.RemoveAt(scrollPos.Count - 1);
            }

            GUILayout.Space(20);
            gm.mode[cnt].modeName = GUILayout.TextField(gm.mode[cnt].modeName, GUILayout.MinWidth(100));
            scrollPos[cnt] = GUILayout.BeginScrollView(scrollPos[cnt]);
            GUILayout.BeginHorizontal("box");
            for (int i = 0; i < gm.mode[cnt].ammOfPlayers.Count; i++)
            {
                DrawMatchups(gm.mode[cnt].ammOfPlayers[i], cnt, i);




                if (i == gm.mode[cnt].ammOfPlayers.Count - 1)
                {
                    GUILayout.BeginVertical();

                    if (GUILayout.Button("Add \n player", GUILayout.MinWidth(50), GUILayout.MinHeight(120)))
                    {
                        gm.mode[cnt].ammOfPlayers.Add(new Player());
                    }
                    GUILayout.EndVertical();
                }


            }

            if (gm.mode[cnt].ammOfPlayers.Count == 0)
            {
                GUILayout.BeginVertical();
                if (GUILayout.Button("Add a player"))
                {
                    gm.mode[cnt].ammOfPlayers.Add(new Player());
                }


                if (GUILayout.Button("Remove Mode"))
                {
                    RemoveElement(cnt);
                    return;
                }
                GUILayout.EndVertical();
            }
            else
            {
                if (GUILayout.Button("Remove \n Mode", GUILayout.MinWidth(60), GUILayout.MinHeight(120)))
                {
                    RemoveElement(cnt);
                    return;
                }
            }



            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();


        }

        GUILayout.Space(30);
        if (GUILayout.Button("Add GameMode"))
        {
            AddElement();
        }

        GUILayout.Space(10);

        base.OnInspectorGUI();
        serializedObject.Update();
        Undo.RecordObject(gm, "GameManager");
        serializedObject.ApplyModifiedProperties();
    }

    void DrawMatchups(Player matchup, int cnt, int i)
    {
        GUILayout.BeginVertical("box");

        DrawPlayer(matchup, "Current Player");
        if (GUILayout.Button("Remove \n player", GUILayout.Width(120), GUILayout.Height(40)))
        {
            gm.mode[cnt].ammOfPlayers.RemoveAt(i);
        }
        GUILayout.EndVertical();
    }

    void DrawPlayer(Player player, string name)
    {
        GUILayout.Space(10);
        GUILayout.BeginVertical("box");
        GUILayout.Label(name);
        GUILayout.Space(10);
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
        player.isComputer = GUILayout.Toggle(player.isComputer, "Computer Driven?");
        player.player = (Team)EditorGUILayout.EnumPopup(player.player);
        GUILayout.EndVertical();
    }

    private void AddElement()
    {
        gm.mode.Add(new GameMode());
    }

    private void RemoveElement(int index)
    {
        gm.mode.RemoveAt(index);
    }
}
