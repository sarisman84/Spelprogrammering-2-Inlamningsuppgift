using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using static ChineseCheckers.HexagonGrid;
using UnityEngine;

public enum GameModes { TwoPlayer, ThreePlayer, FourPlayer, SixPlayer, Debug }
public class GameManager : MonoBehaviour
{
    #region  Blueprint for Board
    private const int
    V = (int)Team.Empty,
        O = (int)Team.Unoccupied,
        B = (int)Team.BigBlue,
        R = (int)Team.BigRed,
        Y = (int)Team.BigYellow,
        yO = (int)Team.BigOrange,
        G = (int)Team.BigGreen,
        M = (int)Team.BigMagenta,
        RO = (int)Team.BigRedToOrange,
        OG = (int)Team.BigOrangeToGreen,
        GB = (int)Team.BigGreenToBlue,
        BY = (int)Team.BigBlueToYellow,
        YM = (int)Team.BigYellowToMagenta,
        MR = (int)Team.BigMagentaToRed,
        D = (int)Team.Debug;

    /*
    2 = Yellow
    3 = Green
    4 = Red
    5 = Blue
    6 = Orange
    7 = Magenta
     */
    //Blueprint for the int[,] that is used to shape the grid

    /*

      Star Shape
      { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }, 
      { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, 
      { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 
       { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
      { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
       { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }, 
      { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }, 
       { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }, 
      { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
       { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
      { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 
       { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, 
      { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }, 
      { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },


      Star Shade

      { 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 0, 4, 4, 0, 0, 0, 0, 0, 0 }, 
      { 0, 0, 0, 0, 0, 4, 4, 4, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 4, 4, 4, 4, 0, 0, 0, 0, 0 }, 
      { 6, 6, 6, 6, 1, 1, 1, 1, 1, 5, 5, 5, 5 }, 
       { 6, 6, 6, 1, 1, 1, 1, 1, 1, 5, 5, 5, 0 }, 
      { 0, 6, 6, 1, 1, 1, 1, 1, 1, 1, 5, 5, 0 }, 
       { 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 5, 0, 0 }, 
      { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 }, 
       { 0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0, 0 }, 
      { 0, 7, 7, 1, 1, 1, 1, 1, 1, 1, 3, 3, 0 }, 
       { 7, 7, 7, 1, 1, 1, 1, 1, 1, 3, 3, 3, 0 }, 
      { 7, 7, 7, 7, 1, 1, 1, 1, 1, 3, 3, 3, 3 }, 
       { 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0 }, 
      { 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0 }, 
       { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0 },

       */

    int[,] blueprint = new int[,] { { V, V, V, V, V, V, V, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, 2, V, V, V, V, V, V }, { V, V, V, V, V, 2, 2, 2, 2, V, V, V, V, V, V }, { V, 6, 6, 6, 6, RO, R, R, R, MR, 5, 5, 5, 5, V }, { V, 6, 6, 6, yO, O, O, O, O, M, 5, 5, 5, V, V }, { V, V, 6, 6, yO, O, O, O, O, O, M, 5, 5, V, V }, { V, V, 6, yO, O, O, O, O, O, O, M, 5, V, V, V }, { V, V, V, OG, O, O, O, O, O, O, O, YM, V, V, V }, { V, V, 7, G, O, O, O, O, O, O, Y, 3, V, V, V }, { V, V, 7, 7, G, O, O, O, O, O, Y, 3, 3, V, V }, { V, 7, 7, 7, G, O, O, O, O, Y, 3, 3, 3, V, V }, { V, 7, 7, 7, 7, GB, B, B, B, BY, 3, 3, 3, 3, V }, { V, V, V, V, V, 4, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, V, V, V, V, V, V, V }, { V, V, V, V, V, V, V, 4, V, V, V, V, V, V, V },

    };

    int[,] debugBlueprint = new int[,] { { V, V, V, V, V, V, V, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, 2, V, V, V, V, V, V }, { V, V, V, V, V, 2, 2, 2, 2, V, V, V, V, V, V }, { V, 6, 6, 6, 6, RO, R, R, R, MR, 5, 5, 5, 5, V }, { V, 6, 6, 6, yO, O, O, O, O, M, 5, 5, 5, V, V }, { V, V, 6, 6, yO, O, O, O, O, O, M, 5, 5, V, V }, { V, V, 6, yO, O, D, O, D, O, D, M, 5, V, V, V }, { V, V, V, OG, D, O, D, O, D, O, D, YM, V, V, V }, { V, V, 7, G, O, D, O, D, O, D, Y, 3, V, V, V }, { V, V, 7, 7, G, O, O, O, O, O, Y, 3, 3, V, V }, { V, 7, 7, 7, G, O, O, O, O, Y, 3, 3, 3, V, V }, { V, 7, 7, 7, 7, GB, B, B, B, BY, 3, 3, 3, 3, V }, { V, V, V, V, V, 4, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, V, V, V, V, V, V, V }, { V, V, V, V, V, V, V, 4, V, V, V, V, V, V, V },

    };

    #endregion

    /// <summary>
    /// This region defines a variety of different classes that hold certain information, this is made this way so that Unity can serialize it into the editor
    /// </summary>
    #region Game Setup
    public NodeObject nodePrefab;
    public PieceObject piecePrefab;

    [System.Serializable]
    public class GameMode
    {
        [System.Serializable]
        public class Match
        {
            [System.Serializable]
            public class Player
            {

                public Team playerTeam;
                public bool isComputer;
            }
            public Player player;

        }
        public Match[] matches;
    }

    #endregion

    public List<GameMode> modes;

    /// <summary>
    /// A mode selector for the list above.
    /// </summary>
    public GameModes selectedMode;
    public TMPro.TMP_Dropdown dropdown;

    //A list of all players.
    public static IPlayer[] allPlayers;

    //public int amountOfPlayers;

}
// public void Add () {
//     modes.Add (new GameMode ());
// }

// public void Remove () {
//     if (modes.Count <= 0) return;
//     modes.RemoveAt (modes.Count - 1);
// }

// public void SelectMode (int mode) {
//     selectedMode = (GameModes) mode;
// }
// public void StartGame () {

//     allPlayers = PlayerMatchup.StartNewGame (modes[(int) selectedMode], piecePrefab);
//     StartCoroutine (PlayerMatchup.TurnSystem ());

// }
// #endregion

// private void Awake () {
//     //Create a board at the start of the game
//     BoardManager.originalBoard = BoardManager.originalBoard ?? CreateGridArray (blueprint, nodePrefab);
// }

// private void Update () {
//     if (dropdown.enabled)
//         selectedMode = (GameModes) dropdown.value;

// }

