using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TestBoardModel;
public class TestManager : MonoBehaviour {

    #region Constant Tags
    private const int
    U = (int) NodeColor.Unoccupied,
        R = (int) NodeColor.Red,
        O = (int) NodeColor.Orange,
        Y = (int) NodeColor.Yellow,
        G = (int) NodeColor.Green,
        B = (int) NodeColor.Blue,
        M = (int) NodeColor.Magenta,
        r = (int) NodeColor.RedFade,
        o = (int) NodeColor.OrangeFade,
        y = (int) NodeColor.YellowFade,
        g = (int) NodeColor.GreenFade,
        b = (int) NodeColor.BlueFade,
        m = (int) NodeColor.MagentaFade,
        A = (int) NodeColor.RedOrange,
        C = (int) NodeColor.OrangeYellow,
        D = (int) NodeColor.YellowGreen,
        E = (int) NodeColor.GreenBlue,
        F = (int) NodeColor.BlueMagenta,
        H = (int) NodeColor.MagentaRed;
    #endregion
    int[, ] blueprint = new int[, ] { { 0, 0, 0, 0, 0, 0, 0, G, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, G, G, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, G, G, G, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, G, G, G, G, 0, 0, 0, 0, 0, 0 }, { 0, Y, Y, Y, Y, D, g, g, g, E, B, B, B, B, 0 }, { 0, Y, Y, Y, y, U, U, U, U, b, B, B, B, 0, 0 }, { 0, 0, Y, Y, y, U, U, U, U, U, b, B, B, 0, 0 }, { 0, 0, Y, y, U, U, U, U, U, U, b, B, 0, 0, 0 }, { 0, 0, 0, C, U, U, U, U, U, U, U, F, 0, 0, 0 }, { 0, 0, O, o, U, U, U, U, U, U, m, M, 0, 0, 0 }, { 0, 0, O, O, o, U, U, U, U, U, m, M, M, 0, 0 }, { 0, O, O, O, o, U, U, U, U, m, M, M, M, 0, 0 }, { 0, O, O, O, O, A, r, r, r, H, M, M, M, M, 0 }, { 0, 0, 0, 0, 0, R, R, R, R, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, R, R, R, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, R, R, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, R, 0, 0, 0, 0, 0, 0, 0 },

    };

    public ModeSelect[] modeSelection;
    public NodeObject nodePrefab;
    public PieceObject piecePrefab;

    public List<UserModel> allPlayers;

    public static bool isReady = false;

    public static TestManager ins;

    public TMPro.TMP_Text turnText, winText;
    public TMPro.TMP_Dropdown selection;
    private void Awake () {
        if (ins != null) {
            Destroy (gameObject);
            return;
        }
        ins = this;

        test_OriginalBoard = GridCreator.ChineseCheckers.CreateHexagonGrid (blueprint, nodePrefab);

    }

    bool value;
    public void DebugNodes () {
        value = !value;
        foreach (var item in FindObjectsOfType<NodeObject> ()) {
            item.DebugCoordinates (value);
        }
        
        if(!value){
            NodeObject.ResetInteractions();
        }
    }

    public bool EnableDebug => value;

    public void OnStartEvent () {
        allPlayers = TestGameModel.StartNewGame (modeSelection[selection.value].players, piecePrefab);
        TestGameModel.GetNextTurn (allPlayers, turnText);
    }

    private void Update () {
        //Debug.Log ($"Updating, is {(isReady?"":"not")} ready");
        if (isReady) {
            TurnStarted ();
        }
    }
    public void TurnEnded () {
        // Debug.Log ("Setting ready");
        isReady = true;
    }

    public void TurnStarted () {
        isReady = false;
        TestGameModel.GetNextTurn (allPlayers, turnText);

    }

    public void ResetVictoryText(){
        winText.text = "";
    }

    public void InsertVictoryMessage(string message){
        winText.text = message;
    }
}

[System.Serializable]
public class ModeSelect {

    public string modeName;
    public List<PlayerDefinition> players = new List<PlayerDefinition> ();

}

[System.Serializable]
public class PlayerDefinition {
    public bool computerDriven;
    public Team player;

    public PlayerDefinition (UserModel user) {
        computerDriven = ((user as ComputerPlayer) != null) ? true : false;
        player = user.currentTeam;
    }
}