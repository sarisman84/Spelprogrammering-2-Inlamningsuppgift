using System.Collections.Generic;
using UnityEngine;
public class GameManagercs : MonoBehaviour
{
    #region Constant Tags
    private const int
    U = (int)NodeColor.Unoccupied,
        R = (int)NodeColor.Red,
        O = (int)NodeColor.Orange,
        Y = (int)NodeColor.Yellow,
        G = (int)NodeColor.Green,
        B = (int)NodeColor.Blue,
        M = (int)NodeColor.Magenta,
        r = (int)NodeColor.RedFade,
        o = (int)NodeColor.OrangeFade,
        y = (int)NodeColor.YellowFade,
        g = (int)NodeColor.GreenFade,
        b = (int)NodeColor.BlueFade,
        m = (int)NodeColor.MagentaFade,
        A = (int)NodeColor.RedOrange,
        C = (int)NodeColor.OrangeYellow,
        D = (int)NodeColor.YellowGreen,
        E = (int)NodeColor.GreenBlue,
        F = (int)NodeColor.BlueMagenta,
        H = (int)NodeColor.MagentaRed;
    #endregion
    int[,] blueprint = new int[,] { { 0, 0, 0, 0, 0, 0, 0, G, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, G, G, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, G, G, G, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, G, G, G, G, 0, 0, 0, 0, 0, 0 }, { 0, Y, Y, Y, Y, D, g, g, g, E, B, B, B, B, 0 }, { 0, Y, Y, Y, y, U, U, U, U, b, B, B, B, 0, 0 }, { 0, 0, Y, Y, y, U, U, U, U, U, b, B, B, 0, 0 }, { 0, 0, Y, y, U, U, U, U, U, U, b, B, 0, 0, 0 }, { 0, 0, 0, C, U, U, U, U, U, U, U, F, 0, 0, 0 }, { 0, 0, O, o, U, U, U, U, U, U, m, M, 0, 0, 0 }, { 0, 0, O, O, o, U, U, U, U, U, m, M, M, 0, 0 }, { 0, O, O, O, o, U, U, U, U, m, M, M, M, 0, 0 }, { 0, O, O, O, O, A, r, r, r, H, M, M, M, M, 0 }, { 0, 0, 0, 0, 0, R, R, R, R, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, R, R, R, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, R, R, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, R, 0, 0, 0, 0, 0, 0, 0 },

    };

    public NodeObject nodePrefab;
    public PieceObject piecePrefab;



    public List<GameMode> mode;

    public TMPro.TMP_Dropdown dropdown;

    public static int selectedMode;
    public static List<UserModel> allPlayers;

   

    public void StartGame()
    {
        allPlayers = GameModel.StartNewGame(mode[dropdown.value].ammOfPlayers, piecePrefab);
        selectedMode = dropdown.value;
        StartCoroutine(GameModel.GameRuntime(allPlayers));
    }

    private void Awake()
    {
        BoardModel.originalBoard = GridCreator.ChineseCheckers.CreateHexagonGrid(blueprint, nodePrefab);
    }
}

[System.Serializable]
public class GameMode
{
    public string modeName;
    public List<Player> ammOfPlayers = new List<Player>();

}

[System.Serializable]

public class Player
{
    public Team player;
    public bool isComputer;
}