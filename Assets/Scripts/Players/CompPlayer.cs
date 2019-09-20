using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ChineseCheckers;
using TMPro;
using UnityEngine;

//Node: This script is not implemented yet. 

/// <summary>
/// The core class for computer based player behaivours.
/// </summary>
public class CompPlayer : MonoBehaviour, IPlayer
{

    #region Variables
    [SerializeField] List<Node> cachedValidNodes;
    [SerializeField] TeamGenerator currentTeam;
    IPlayer opponent;

    float maxValue;

    [SerializeField] Node currentPiece, desiredNode;

    [SerializeField] List<Piece> cachedPieces;
    bool hasJumped;

    #endregion

    #region  Properties

    public IPlayer CurrentOpponent
    {
        get => opponent = opponent ?? UserManager.SetOpponent(this);
        set => opponent = value;
    }

    public Node SelectedPiece
    {
        get
        {
            Node newPiece = null;
            GetBaseNodes();
            int index = UnityEngine.Random.Range(0, cachedPieces.Count);
            if (index >= cachedPieces.Count) return null;
            newPiece = UserManager.AttemptToGetPiece(cachedPieces[index].NodeReference, currentTeam.Team, false, ref cachedValidNodes);
            if (cachedValidNodes.Count == 0)
            {
                return null;
            }
            opponent = opponent ?? UserManager.SetOpponent(this);

            currentPiece = newPiece;
            return currentPiece;
        }

        set => currentPiece = value;
    }


    /// <summary>
    /// A Minimax method that simulates a board with a move and then proceeds to evaluate a score.
    /// </summary>
    /// <param name="depth"> How far should the algorithm evaluate and simulate.</param>
    /// <param name="currentPlayer">The current owner of said method</param>
    /// <param name="currentBoard">The default state of the board in which we will simulate from</param>
    /// <param name="maximizingPlayer">Dictates which player's turn it is.</param>
    /// <returns>A list of boards each with its own evaluated score.</returns>
    public static List<Board> FindAllPossiblePaths(int depth, IPlayer currentPlayer, Board currentBoard, bool maximizingPlayer)
    {
        switch (currentPlayer)
        {
            //If the player is of type CompPlayer, get its BaseNodes.
            case CompPlayer comp:
                comp.GetBaseNodes();
                break;
        }
        List<Board> listOfTempBoards = new List<Board>();
        if (depth == 0) { return listOfTempBoards; }

        if (!maximizingPlayer)
        {
            float minValue = float.MaxValue;
            Board simulatedBoard = BoardManager.board.Copy();
            List<Piece> simulatedOwnedPieces = new List<Piece>();
            foreach (Node node in simulatedBoard.board)
            {
                if (node.StoredPiece != null && node.StoredPiece.BelongsTo == currentPlayer.CurrentTeam.Team)
                    simulatedOwnedPieces.Add(node.StoredPiece);
            }
            foreach (var piece in simulatedOwnedPieces)
            {
                List<Node> path = BoardManager.Path(piece.NodeReference, false, currentBoard.board);
                foreach (var node in path)
                {
                    //Create a new simulated board.
                    Board newBoard = BoardManager.board.Copy();
                    //Simulate a move
                    Piece.SimulatingMovePiece(piece, piece.NodeReference, node);
                    //Evaluate a score on said move based on the current position and the distance to the enemy's base.
                    float eval = Value(piece, currentPlayer.CurrentTeam.TeamBase);
                    minValue = Mathf.Min(eval, minValue);
                    newBoard.value = minValue;
                    listOfTempBoards.Add(newBoard);
                    //Do a recursive call on the method with the oppisite sides turn to simulate.
                    List<Board> board = FindAllPossiblePaths(depth - 1, currentPlayer, newBoard, true);
                    //Add the newly found results from the previous recursive call into a list.
                    listOfTempBoards.AddRange(board);
                }

            }
        }
        else if (maximizingPlayer)
        {
            float maxValue = float.MinValue;
            Board simulatedBoard = BoardManager.board.Copy();
            List<Piece> simulatedOwnedPieces = new List<Piece>();
            foreach (Node node in simulatedBoard.board)
            {
                if (node.StoredPiece != null && node.StoredPiece.BelongsTo == currentPlayer.CurrentOpponent.CurrentTeam.Team)
                    simulatedOwnedPieces.Add(node.StoredPiece);
            }
            foreach (var piece in simulatedOwnedPieces)
            {
                List<Node> path = BoardManager.Path(piece.NodeReference, false, currentBoard.board);
                foreach (var node in path)
                {
                    //Create a new simulated board.
                    Board newBoard = BoardManager.board.Copy();
                    //Simulate a move
                    Piece.SimulatingMovePiece(piece, piece.NodeReference, node);
                    //Evaluate a score on said move based on the current position and the distance to the enemy's base.
                    float eval = Value(piece, currentPlayer.CurrentOpponent.CurrentTeam.TeamBase);
                    maxValue = Mathf.Max(eval, maxValue);
                    newBoard.value = maxValue;

                    listOfTempBoards.Add(newBoard);
                    //Do a recursive call on the method with the oppisite sides turn to simulate.
                    List<Board> board = FindAllPossiblePaths(depth - 1, currentPlayer, newBoard, false);
                    //Add the newly found results from the previous recursive call into a list.F
                    listOfTempBoards.AddRange(board);
                }

            }
        }
        //Return the results.
        return listOfTempBoards;

    }



    public void GetBaseNodes()
    {

        if (cachedPieces == null || cachedPieces.Count == 0)
        {
            List<Piece> pieceArray = new List<Piece>();
            for (int i = 0; i < currentTeam.TeamBase.Length; i++)
            {
                pieceArray.Add(currentTeam.TeamBase[i].StoredPiece);
            }
            cachedPieces = pieceArray;
        }

    }

    public Node DesiredTarget
    {
        get
        {
            int index = UnityEngine.Random.Range(0, cachedValidNodes.Count - 1);
            if (index >= cachedValidNodes.Count) return desiredNode;
            return desiredNode = (currentPiece != null) ? UserManager.AttemptToGetTarget(cachedValidNodes[index], ref cachedValidNodes) : desiredNode;
        }

        set => desiredNode = value;
    }
    public bool HasDoneFirstMove
    {
        get => hasJumped;
        set => hasJumped = value;
    }

    public Node DetectedNode =>
        throw new NotImplementedException();

    public List<Node> CachedValidMoves
    {
        get => cachedValidNodes;
        set => cachedValidNodes = value;
    }

    public List<Piece> CachedPieces
    {
        get => cachedPieces;
        set => cachedPieces = value;
    }

    public TeamGenerator CurrentTeam
    {
        get =>
            currentTeam;
        set =>
            currentTeam = value;
    }

    #endregion

    //current value / max value

    /// <summary>
    /// Create a player using the Interface IPlayer. Current method returns a ComputerPlayer.
    /// </summary>
    /// <param name="currentTeam"> What team this player will be in.</param>
    /// <param name="opponent">What opponent will this player face.</param>
    /// <returns>A newly created player. </returns>
    static TextMeshPro text;
    public TextMeshPro SetupText
    {
        get => text;
        set => text = value;
    }
    bool endTurn = false;
    public bool EndTurn
    {
        get =>
            endTurn;
        set =>
            endTurn = value;
    }

    public static IPlayer CreatePlayer(Team team, TeamGenerator gen)
    {
        CompPlayer player = new GameObject($"Player {team} : Computer").AddComponent<CompPlayer>();
        Canvas parent = FindObjectOfType<Canvas>();
        player.SetupText = player.SetupText ?? new GameObject("Max Distance").AddComponent<TextMeshPro>();
        player.SetupText.transform.parent = parent.transform;
        player.CurrentTeam = gen;
        return player;
    }



    static float Value(Piece piece, Node[] targets)
    {
        float maxDist = 0;
        if (targets == null) return maxDist;
        foreach (Node target in targets)
        {
            float dist = Vector2.Distance(piece.CurrentPosition, target.CurrentBoardPosition);
            maxDist += dist;
        }

        return maxDist;
    }

    // int i;
    // float Minimax(Node[] knownValidPositions, int depth, bool maximizingPos)
    // {

    //     if (depth == 0)
    //     {
    //         return Value(knownValidPositions[i], opponent.TeamBase);
    //     }

    //     if(maximizingPos){
    //         float maxEval = Mathf.NegativeInfinity;
    //         foreach (var validPos in knownValidPositions)
    //         {
    //             float eval = 
    //         }
    //     }
    // }

    /*
        First, read all of the available pieces that you have that can move and store them somewhere.

        Second, pick a piece that is the best move possible.
        To do so, i need to define the following:
            1.What "position" is in the minimax method?
            2.What are the maximizingEval and minimizingEval?


        Third, apply same move methods as the human player.
        
        
     */
}