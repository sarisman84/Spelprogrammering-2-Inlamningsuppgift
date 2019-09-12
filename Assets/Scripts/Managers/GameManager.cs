using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using static ChineseCheckers.HexagonGrid;
using UnityEngine;

public enum GameModes { TwoPlayer, ThreePlayer, FourPlayer, SixPlayer, Debug }
public class GameManager : MonoBehaviour
{
    #region  Blueprint for Board
    private const int V = (int)Team.Empty,
      O = (int)Team.Unoccupied;

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

    int[,] blueprint = new int[,] { { V, V, V, V, V, V, V, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, V, V, V, V, V, V, V }, { V, V, V, V, V, V, 2, 2, 2, V, V, V, V, V, V }, { V, V, V, V, V, 2, 2, 2, 2, V, V, V, V, V, V }, { V, 6, 6, 6, 6, O, O, O, O, O, 5, 5, 5, 5, V }, { V, 6, 6, 6, O, O, O, O, O, O, 5, 5, 5, V, V }, { V, V, 6, 6, O, O, O, O, O, O, O, 5, 5, V, V }, { V, V, 6, O, O, O, O, O, O, O, O, 5, V, V, V }, { V, V, V, O, O, O, O, O, O, O, O, O, V, V, V }, { V, V, 7, O, O, O, O, O, O, O, O, 3, V, V, V }, { V, V, 7, 7, O, O, O, O, O, O, O, 3, 3, V, V }, { V, 7, 7, 7, O, O, O, O, O, O, 3, 3, 3, V, V }, { V, 7, 7, 7, 7, O, O, O, O, O, 3, 3, 3, 3, V }, { V, V, V, V, V, 4, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, 4, V, V, V, V, V, V }, { V, V, V, V, V, V, 4, 4, V, V, V, V, V, V, V }, { V, V, V, V, V, V, V, 4, V, V, V, V, V, V, V },

  };

    #endregion

    /// <summary>
    /// This region defines a variety of different classes that hold certain information, this is made this way so that Unity can serialize it into the editor
    /// </summary>
    #region Game Setup
    public Node nodePrefab;
    public Piece piecePrefab;
    [System.Serializable]
    public class Player
    {
        public bool isComputer;
        public Team team;
    }

    [System.Serializable]
    public class Matchups
    {
        public Player firstPlayer;
        public Player secondPlayer;

    }

    [System.Serializable]
    public class GameMode
    {
        public Matchups[] matchup;
        public bool oddAmountOfPlayers;

    }
    #endregion
    public List<GameMode> modes;

    /// <summary>
    /// A mode selector for the list above.
    /// </summary>
    public GameModes selectedMode;
    public TMPro.TMP_Dropdown dropdown;


    //A list of all players.
    IPlayer[] allPlayers;

    //public int amountOfPlayers;



    #region UI Button Logic
    public void Add()
    {
        modes.Add(new GameMode());
    }

    public void Remove()
    {
        if (modes.Count <= 0) return;
        modes.RemoveAt(modes.Count - 1);
    }

    public void SelectMode(int mode)
    {
        selectedMode = (GameModes)mode;
    }
    public void StartGame()
    {

        IPlayer[][] players = PlayerMatchup.StartNewGame(modes[(int)selectedMode].matchup, modes[(int)selectedMode].oddAmountOfPlayers, piecePrefab, selectedMode);
        allPlayers = new IPlayer[players[0].Length + players[1].Length];
        if (players[0] != null)
        {
            players[0].CopyTo(allPlayers, 0);

        }

        if (players[1] != null)
        {
            players[1].CopyTo(allPlayers, players[0].Length);

        }

    }
    #endregion

    private void Awake()
    {
        //Create a board at the start of the game
        BoardManager.board = BoardManager.board ?? CreateGrid(blueprint, nodePrefab);
    }





    private void Update()
    {
        if (dropdown.enabled)
            selectedMode = (GameModes)dropdown.value;

        if (allPlayers == null) return;
        foreach (var item in allPlayers)
        {
            if (item == null) continue;
            if (PlayerMatchup.HasPlayerWon(item)) { }
        }

    }


}