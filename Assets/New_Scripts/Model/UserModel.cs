using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

using static TestBoardModel;

//Definitions for each color of a node. A big node on the big reds and greens. These are meant for the 2 player mode. 
public enum Team { None, Unoccupied, Red, Orange, Yellow, Green, Blue, Magenta, BigRed, BigGreen = 11 }


//Each user has a opponent variable (used for the AI), 
//some lists and properties to get the nessesary information (such as their opponents base, their pieces, ect) 
//as well as a set of methods used to pathfind from a piece to a node.
public abstract class UserModel : MonoBehaviour
{
    public Team currentTeam;
    public UserModel opponent;
    private List<PieceObject> viewPieces;
    private List<OpponentNode> targetBase;

    public List<BoardPiece> OwnedPieces
    {
        get
        {
            return globalPieceList.FindAll(x => x.belongsTo == currentTeam);
        }
    }

    public class OpponentNode
    {
        public BoardNode node;
        public bool isOccupied
        {
            get
            {
                BoardPiece piece = owner.OwnedPieces.Find(p => p.pos == node.pos);
                if (piece != null)
                {
                    if (TestManager.ins.EnableDebug)
                        globalNodeViewList.Find(z => z.boardCoordinate == node.pos).OnInteract("#cc33ff");
                    return true;
                }
                if (TestManager.ins.EnableDebug)
                    globalNodeViewList.Find(z => z.boardCoordinate == node.pos).OnInteract();

                return false;
            }
        }

        UserModel owner;


        public OpponentNode(BoardNode foundNode, UserModel user)
        {
            node = foundNode;
            owner = user;
        }
    }

    public List<PieceObject> OwnedViewPieces { get => viewPieces; set => viewPieces = value; }

    public List<OpponentNode> TargetBase
    {
        get
        {
            targetBase = targetBase ?? FindBase();
            return targetBase;
        }
    }

    private List<OpponentNode> FindBase()
    {
        List<BoardNode> results = test_OriginalBoard.FindAll(node => node.belongsTo == GetOpponent(this));
        List<OpponentNode> output = new List<OpponentNode>();
        for (int i = 0; i < results.Count; i++)
        {
            output.Add(new OpponentNode(results[i], this));
        }

        return output;

    }

  

    //Used for Minimax (computer)
    public BoardNode DesiredGoal()
    {
        float maxEval = float.MinValue;
        BoardNode target = new BoardNode();

        //Note: This is a small fix to the 2 player mode. 
        //This checks if the amount of occupied spaces is one less than the opponents base (i.e. the opposite side of the board).
        //If it is, get the unoccupied place and save that as a desired goal for the AI.
        if (TestGameModel.amountOfPlayers == 2)
            if (TargetBase.FindAll(p => p.isOccupied == true).Count == OwnedPieces.Count - 1)
            {
                target = TargetBase.Find(p => p.isOccupied == false).node;
                return target;
            }

        //This gets the furthest position in the board in correlation to the player's team and position with the opposite side of the board.
        for (int t = 0; t < TargetBase.Count; t++)
        {
            for (int p = 0; p < OwnedPieces.Count; p++)
            {
                float dist = Vector2.Distance(TargetBase[t].node.worldPos, OwnedPieces[p].worldPos);
                if (maxEval < dist)
                {
                    maxEval = dist;
                    target = TargetBase[t].node;
                }
            }
        }


        return target;
    }

    public bool HasPlayerWon()
    {

        for (int i = 0; i < OwnedPieces.Count; i++)
        {
            if (!targetBase[i].isOccupied) return false;
        }
        return true;
    }


    //Global methods that are used in general in relation to players.
    #region Static Methods

    //Both of start and end turn here are used as temporary templates for the main Human and Computer classes.


    /// <summary>
    /// Used to start a players turn.
    /// </summary>
    public virtual void StartTurn() { }

    /// <summary>
    /// Used to end a players turn.
    /// </summary>
    public virtual void EndTurn() { }

    /// <summary>
    /// Global method: Used as a fake constructor to create a player.
    /// </summary>
    /// <param name="computerDriven">Is the this player meant to be a computer?</param>
    /// <param name="team">What team does this player correspond into?</param>
    /// <returns>A player.</returns>
    public static UserModel CreatePlayer(bool computerDriven, Team team)
    {

        if (computerDriven)
        {
            //Create Object
            ComputerPlayer computerEntity = new GameObject($"(AI) Player {team}").AddComponent<ComputerPlayer>();
            //Set its team
            computerEntity.currentTeam = team;
            //Set its target for minimax.

            computerEntity.targetBase = GetTargetBase(GetOpponent(computerEntity), computerEntity);
            return computerEntity;
        }

        HumanPlayer normalPlayer = new GameObject($"Player {team}").AddComponent<HumanPlayer>();

        normalPlayer.currentTeam = team;


        normalPlayer.targetBase = GetTargetBase(GetOpponent(normalPlayer), normalPlayer);

        return normalPlayer;
    }

    /// <summary>
    /// Helper method: Sets the goal for the player
    /// </summary>
    /// <param name="desiredTeam">Which team do you want to set your goal into?</param>
    /// <param name="user">The current user who called this method</param>
    /// <returns>A list of nodes(positions) that corresponds the player's opposite side of the board. </returns>
    private static List<OpponentNode> GetTargetBase(Team desiredTeam, UserModel user)
    {
        List<OpponentNode> results = new List<OpponentNode>();
        foreach (var node in test_OriginalBoard.boardArr)
        {
            if (node.belongsTo == desiredTeam)
            {
                results.Add(new OpponentNode(node, user));
            }
        }
        return results;
    }
    
    /// <summary>
    /// Global method: Gets the opponent of a player.
    /// </summary>
    /// <param name="user">The current user we want to check from</param>
    /// <returns>The user's opposite side of the board.</returns>
    public static Team GetOpponent(UserModel user)
    {
        switch (user.currentTeam)
        {

            case Team.Red:
                return Team.Green;

            case Team.Orange:
                return Team.Blue;

            case Team.Yellow:
                return Team.Magenta;

            case Team.Green:
                return Team.Red;

            case Team.Blue:
                return Team.Orange;

            case Team.Magenta:
                return Team.Yellow;
        }
        return Team.None;
    }
    
    /// <summary>
    /// Helper method: Checks for any neighbours near the source.
    /// </summary>
    /// <param name="source">The center of the check.</param>
    /// <param name="user">A reference to the current user in order to access some mainline methods</param>
    /// <returns>A list of valid neighbours.</returns>
    public static List<BoardPiece> CheckForNeighbours(BoardPiece source, UserModel user)
    {
        List<BoardPiece> output = new List<BoardPiece>();
        for (int i = 0; i < 6; i++)
        {
            BoardPiece neighbour = GetNeighbour(source, i, user);
            if (neighbour == null) continue;
            output.Add(neighbour);
        }
        return output;
    }

    /// <summary>
    /// Helper method: Returns a valid neighbour.
    /// </summary>
    /// <param name="source">The source of the method's check.</param>
    /// <param name="i">The current direction we are checking from.</param>
    /// <param name="user">A reference to the current user in order to access some mainline methods.</param>
    /// <returns>A valid neighbour.</returns>
    private static BoardPiece GetNeighbour(BoardPiece source, int i, UserModel user)
    {
        Vector2Int result = source.pos + user.BoardDirection(source.pos, i);
        if (user.OutOfBounds(result) || test_OriginalBoard[result.x, result.y].belongsTo == Team.None) return null;
        return globalPieceList.Find(p => p.pos == result);
    }
    #endregion

    //This is a recursive method that finds a list of available paths per source in a position.
    //I am using Vector2int here as a general variable incase i want to use other forms of 2D arrays.
    public List<Vector2Int> PathOfMoves(Vector2Int source, List<Vector2Int> savedResults, bool isFirstSearch)
    {
        for (int curDir = 0; curDir < 6; curDir++)
        {
            Vector2Int potentialPath = AttemptToGetPath(source, curDir, savedResults);
            if (potentialPath == -Vector2Int.one) continue;
            if (globalPieceList.Any(p => p.pos == potentialPath))
            {
                Vector2Int branch = AttemptToGetPath(potentialPath, curDir, savedResults);
                if (branch == -Vector2Int.one) continue;
                if (!globalPieceList.Any(p => p.pos == branch))
                {
                    savedResults.Add(branch);
                    savedResults = PathOfMoves(branch, savedResults, false);

                }
            }
            if (!globalPieceList.Any(p => p.pos == potentialPath))
            {
                if (isFirstSearch)
                {
                    savedResults.Add(potentialPath);
                }
            }

        }
        return savedResults;
    }



    /// <summary>
    /// Helper method: Attemps to get a valid path depending on where it checks from, the direction it checks from as well as if the result in question is already within a saved list.
    /// </summary>
    /// <param name="source">The center of said check</param>
    /// <param name="curDir">The direction of said check</param>
    /// <param name="savedResults">A list to compare if the result does not exist within</param>
    /// <returns>A valid position. Returns null if the result already exists in the list</returns>
    private Vector2Int AttemptToGetPath(Vector2Int source, int curDir, List<Vector2Int> savedResults)
    {
        Vector2Int result = source + BoardDirection(source, curDir);
        if (OutOfBounds(result) || test_OriginalBoard[result.x, result.y].belongsTo == Team.None || savedResults.Any(p => p == result)) return -Vector2Int.one;
        return result;

    }

    /// <summary>
    /// Helper method: Checks if the attemped check is out of bounds from the 2D array (the board of the game)
    /// </summary>
    /// <param name="result">The result in question to be tested</param>
    /// <returns>true if it is indeed out of bounds, else false</returns>
    private bool OutOfBounds(Vector2Int result) => (result.x >= test_OriginalBoard.GetLength(0) || result.y >= test_OriginalBoard.GetLength(1) || result.x < 0 || result.y < 0);

    /// <summary>
    /// Helper method: Gets an offest of a direction depending on the current index inputed.
    /// </summary>
    /// <param name="source">The center of the check</param>
    /// <param name="index">The index that decides the direction taken</param>
    /// <returns>An offset on that direction</returns>
    Vector2Int BoardDirection(Vector2Int source, int index)
    {

        switch (index)
        {

            case 0:
                return new Vector2Int(0, -1); // Left

            case 1:
                return new Vector2Int(0, 1); // Right

            case 2:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, 0);
                return new Vector2Int(-1, 1); //Top Right
            case 3:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, -1);
                return new Vector2Int(-1, 0); //Top left

            case 4:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, 0);
                return new Vector2Int(1, 1); //Bottom Right
            case 5:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, -1);
                return new Vector2Int(1, 0); //Bottom Left
        }
        return Vector2Int.zero;
    }

    /// <summary>
    /// Mainline method used in HumanPlayer: Moves a piece.
    /// </summary>
    /// <param name="currentPiece">The piece that will move (saved in position coordinates)</param>
    /// <param name="target">The target that the selected piece will move to (saved in position coordinates)</param>
    /// <param name="visualOwnedPieces">A list of all visualOwnedPieces (the ones that the player can see)</param>
    /// <param name="hasntDoneFirstMove">A reference of a check in the HumanPlayer class that is set to false once this is called</param>
    protected void MovePiece(Vector2Int currentPiece, Vector2Int target, List<PieceObject> visualOwnedPieces, ref bool hasntDoneFirstMove)
    {
        int globalI = globalPieceList.FindIndex(0, p => p.pos == currentPiece);
        BoardPiece piece = new BoardPiece();

        int i = visualOwnedPieces.FindIndex(0, p => p.boardCoordinate == currentPiece);

        piece.pos = target;
        piece.worldPos = TestBoardModel.test_OriginalBoard[target.x, target.y].worldPos;
        piece.belongsTo = currentTeam;

        visualOwnedPieces[i].transform.position = piece.worldPos;
        visualOwnedPieces[i].boardCoordinate = piece.pos;

        globalPieceList[globalI] = piece;

        currentPiece = target;
        hasntDoneFirstMove = false;

    }

    /// <summary>
    /// Mainline method used in ComputerPlayer: Moves a piece.
    /// </summary>
    /// <param name="currentPiece">The piece that will move (saved in position coordinates)</param>
    /// <param name="target">The target that the selected piece will move to (saved in position coordinates)</param>
    /// <param name="visualOwnedPieces">A list of all visualOwnedPieces (the ones that the player can see)</param>
    protected void MovePiece(Vector2Int currentPiece, Vector2Int target, List<PieceObject> visualOwnedPieces)
    {
        int globalI = globalPieceList.FindIndex(0, p => p.pos == currentPiece);
        BoardPiece piece = new BoardPiece();

        int i = visualOwnedPieces.FindIndex(0, p => p.boardCoordinate == currentPiece);
        if (i >= visualOwnedPieces.Count || i < 0) return;
        piece.pos = target;
        piece.worldPos = TestBoardModel.test_OriginalBoard[target.x, target.y].worldPos;
        piece.belongsTo = currentTeam;

        visualOwnedPieces[i].transform.position = piece.worldPos;
        visualOwnedPieces[i].boardCoordinate = piece.pos;

        globalPieceList[globalI] = piece;

    }

    /// <summary>
    /// Helper method: Emulates a MovePiece event in a temporary board.
    /// </summary>
    /// <param name="index">The current selection of a piece within the list (board)</param>
    /// <param name="target">The target that the selected piece will move to (saved in position coordinates)</param>
    /// <param name="board">A simulated board that is seperated from the mainline board.</param>
    public void Simulate_MovePiece(int index, Vector2Int target, List<BoardPiece> board)
    {

        BoardPiece piece = new BoardPiece();
        piece.pos = target;
        piece.worldPos = test_OriginalBoard.FindReference(board[index].pos).worldPos;
        piece.belongsTo = currentTeam;
        board[index] = piece;

    }
    
    /// <summary>
    /// Helper method: Gets all pieces of the same team within a simulated board.
    /// </summary>
    /// <param name="desiredTeam">This dictates what pieces the method will select.</param>
    /// <param name="customBoard">A custom board for the method to search in.</param>
    /// <returns>A list of pieces that match the corresponding desiredTeam.</returns>
    public static List<BoardPiece> GetPlayerPositions(Team desiredTeam, List<BoardPiece> customBoard)
    {
        List<BoardPiece> team = new List<BoardPiece>();
        foreach (BoardPiece piece in customBoard)
        {
            if (piece != null && piece.belongsTo == desiredTeam)
            {
                team.Add(piece);

            }
        }
        return team;
    }
}