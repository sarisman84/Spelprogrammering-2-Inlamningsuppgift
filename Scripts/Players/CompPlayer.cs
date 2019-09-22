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
public class CompPlayer : MonoBehaviour, IPlayer {

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

    public IPlayer CurrentOpponent {
        get => opponent = opponent ?? UserManager.SetOpponent (this);
        set => opponent = value;
    }

    public Node SelectedPiece {
        get {
            Node newPiece = null;
            GetBaseNodes ();
            int index = UnityEngine.Random.Range (0, cachedPieces.Count);
            if (index >= cachedPieces.Count) return null;
            newPiece = UserManager.AttemptToGetPiece (cachedPieces[index].NodeReference, currentTeam.Team, false, ref cachedValidNodes);
            if (cachedValidNodes.Count == 0) {
                return null;
            }
            opponent = opponent ?? UserManager.SetOpponent (this);

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
    public static List<Board> FindAllPossiblePaths (int depth, IPlayer currentPlayer, bool maximizingPlayer, Board currentBoard, List<Board> listOfSims) {

        if (depth == 0) return listOfSims;

        if (!maximizingPlayer) {
            Node[,] testingBoard = currentBoard.Clone ();
            List<Piece> pieceList = new List<Piece> ();
            SimulatePieces (currentPlayer, testingBoard, pieceList);
            SimulateMoves (currentPlayer, currentBoard, listOfSims, pieceList);
            float minEval = float.MaxValue;
            Board bestBoard = null;
            foreach (var sim in listOfSims) {
                if (sim.value < minEval) {
                    bestBoard = sim;
                    minEval = sim.value;
                }
            }
            //listOfSims.Add (bestBoard);
            List<Board> results = FindAllPossiblePaths (depth - 1, currentPlayer, true, bestBoard, listOfSims);
            Debug.Log (results.Count);
            listOfSims.AddRange (results);
        }
        if (maximizingPlayer) {
            if(currentBoard == null) return listOfSims;
            Node[,] testingBoard = currentBoard.Clone ();
            List<Piece> pieceList = new List<Piece> ();
            SimulatePieces (currentPlayer.CurrentOpponent, testingBoard, pieceList);
            SimulateMoves (currentPlayer.CurrentOpponent, currentBoard, listOfSims, pieceList);
            float maxEval = float.MinValue;
            Board bestBoard = null;
            foreach (var sim in listOfSims) {
                if (sim.value < maxEval) {
                    bestBoard = sim;
                    maxEval = sim.value;
                }
            }
            //listOfSims.Add (bestBoard);
            List<Board> results = FindAllPossiblePaths (depth - 1, currentPlayer, true, bestBoard, listOfSims);
            Debug.Log (results.Count);
            listOfSims.AddRange (results);
        }

        //Return the results.
        return listOfSims;

    }

    private static void SimulatePieces (IPlayer currentPlayer, Node[,] testingBoard, List<Piece> pieceList) {
        foreach (Node board in testingBoard) {
            if (board != null && board.StoredPiece != null && board.BelongsTo == currentPlayer.CurrentTeam.Team) {
                pieceList.Add (board.StoredPiece);
            }
        }
    }

    private static void SimulateMoves (IPlayer currentPlayer, Board currentBoard, List<Board> listOfSims, List<Piece> pieceList) {
        foreach (var piece in pieceList) {
            foreach (var node in BoardManager.Path (piece.NodeReference, false)) {
                Board newBoard = new Board();
                Node[,] simulatedBoard = currentBoard.Clone ();
                
                //Make move via coords on simulated board.
                Vector2Int startPos = piece.CurrentPosition;
                Vector2Int endPos = node.CurrentPosition;
                Piece.SimulateMovePiece (currentBoard, startPos, endPos);
                float eval = Value (piece, currentPlayer.CurrentOpponent.CurrentTeam.TeamBase);
                newBoard.value = eval;
                newBoard.board = simulatedBoard;
                listOfSims.Add (newBoard);
            }
        }

    }

    public List<Piece> GetBaseNodes () {

        if (cachedPieces == null || cachedPieces.Count == 0) {
            List<Piece> pieceArray = new List<Piece> ();
            for (int i = 0; i < currentTeam.TeamBase.Length; i++) {
                pieceArray.Add (currentTeam.TeamBase[i].StoredPiece);
            }
            cachedPieces = pieceArray;
            return pieceArray;
        }
        return cachedPieces;

    }

    public Node DesiredTarget {
        get {
            int index = UnityEngine.Random.Range (0, cachedValidNodes.Count - 1);
            if (index >= cachedValidNodes.Count) return desiredNode;
            return desiredNode = (currentPiece != null) ? UserManager.AttemptToGetTarget (cachedValidNodes[index], ref cachedValidNodes) : desiredNode;
        }

        set => desiredNode = value;
    }
    public bool HasDoneFirstMove {
        get => hasJumped;
        set => hasJumped = value;
    }

    public Node DetectedNode =>
        throw new NotImplementedException ();

    public List<Node> CachedValidMoves {
        get => cachedValidNodes;
        set => cachedValidNodes = value;
    }

    public List<Piece> CachedPieces {
        get => cachedPieces ?? GetBaseNodes ();
        set => cachedPieces = value;
    }

    public TeamGenerator CurrentTeam {
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
    public TextMeshPro SetupText {
        get => text;
        set => text = value;
    }
    bool endTurn = false;
    public bool EndTurn {
        get =>
            endTurn;
        set =>
            endTurn = value;
    }

    public static IPlayer CreatePlayer (Team team, TeamGenerator gen) {
        CompPlayer player = new GameObject ($"Player {team} : Computer").AddComponent<CompPlayer> ();
        Canvas parent = FindObjectOfType<Canvas> ();
        player.SetupText = player.SetupText ?? new GameObject ("Max Distance").AddComponent<TextMeshPro> ();
        player.SetupText.transform.parent = parent.transform;
        player.CurrentTeam = gen;
        return player;
    }

    static float Value (Piece piece, Node[] targets) {
        float maxDist = 0;
        if (targets == null) return maxDist;
        foreach (Node target in targets) {
            float dist = Vector2.Distance (piece.CurrentPosition, target.CurrentPosition);
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