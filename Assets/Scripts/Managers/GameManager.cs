using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using static ChineseCheckers.HexagonGrid;
using UnityEngine;


public enum GameModes { TwoPlayer, ThreePlayer, FourPlayer, SixPlayer, Debug }
//Temporary script to test the rest of this project.
public class GameManager : MonoBehaviour
{
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

    public List<GameMode> modes;
    public GameModes selectedMode;

    [SerializeField] HumanPlayer[] allPlayers;


    //public int amountOfPlayers;

    int[,] blueprint = new int[,] {
        { V, V, V, V, V, V, V, 2, V, V, V, V, V, V, V },
          { V, V, V, V, V, V, 2, 2, V, V, V, V, V, V, V },
        { V, V, V, V, V, V, 2, 2, 2, V, V, V, V, V, V },
          { V, V, V, V, V, 2, 2, 2, 2, V, V, V, V, V, V },
        { V, 6, 6, 6, 6, O, O, O, O, O, 5, 5, 5, 5, V },
          { V, 6, 6, 6, O, O, O, O, O, O, 5, 5, 5, V, V },
        { V, V, 6, 6, O, O, O, O, O, O, O, 5, 5, V, V },
          { V, V, 6, O, O, O, O, O, O, O, O, 5, V, V, V },
        { V, V, V, O, O, O, O, O, O, O, O, O, V, V, V },
          { V, V, 7, O, O, O, O, O, O, O, O, 3, V, V, V },
        { V, V, 7, 7, O, O, O, O, O, O, O, 3, 3, V, V },
          { V, 7, 7, 7, O, O, O, O, O, O, 3, 3, 3, V, V },
        { V, 7, 7, 7, 7, O, O, O, O, O, 3, 3, 3, 3, V },
          { V, V, V, V, V, 4, 4, 4, 4, V, V, V, V, V, V },
        { V, V, V, V, V, V, 4, 4, 4, V, V, V, V, V, V },
          { V, V, V, V, V, V, 4, 4, V, V, V, V, V, V, V },
        { V, V, V, V, V, V, V, 4, V, V, V, V, V, V, V },

    };



    /*
    At the start of the game, create a board using an 2 Dimentional Array that holds a number.
    This number tells the nodes what state they should be!
    0 = Empty. (Collisions and Textures are turned off)
    1-7 = Board peaces (Have a dedicated color per number as well as sets the team to each of the nodes corresponding the color).
     */
    public void Add()
    {
        modes.Add(new GameMode());
    }

    public void Remove()
    {
        if (modes.Count <= 0) return;
        modes.RemoveAt(modes.Count - 1);
    }
    private void Awake()
    {

        BoardManager.board = BoardManager.board ?? CreateGrid(blueprint, nodePrefab);
        IPlayer[][] players = PlayerMatchup.StartNewGame(modes[(int)selectedMode].matchup, modes[(int)selectedMode].oddAmountOfPlayers, piecePrefab, selectedMode);
        allPlayers = new HumanPlayer[players[0].Length + players[1].Length];
        players[0].CopyTo(allPlayers, 0);
        players[1].CopyTo(allPlayers, players[0].Length);
    }


    private void Update()
    {

        foreach (var player in allPlayers)
        {
            if(player == null) continue;
            PlayerMatchup.HasPlayerWon(player.GetComponent<IPlayer>());
            Debug.Log(player);
        }

    }


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
}