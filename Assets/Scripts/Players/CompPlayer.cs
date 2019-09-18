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

    [SerializeField] Piece[] cachedPieces;
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
            int index = UnityEngine.Random.Range (0, cachedPieces.Length);
            if (index >= cachedPieces.Length) return null;
            newPiece = UserManager.AttemptToGetPiece (cachedPieces[index].CurrentlyLiesIn, currentTeam.Team, false, ref cachedValidNodes);
            if (cachedValidNodes.Count == 0) {
                return null;
            }
            opponent = opponent ?? UserManager.SetOpponent (this);

            currentPiece = newPiece;
            return currentPiece;
        }

        set => currentPiece = value;
    }

    public static List<Board> FindAllPossiblePaths (int depth, IPlayer currentPlayer, Board currentBoard) {
        switch (currentPlayer) {

            case CompPlayer comp:
                comp.GetBaseNodes ();
                break;
        }
        List<Board> listOfTempBoards = new List<Board> ();
        if (depth == 0) { return listOfTempBoards; }

        if (currentPlayer != currentPlayer.CurrentOpponent) {
            float minValue = float.MaxValue;
            Board simulatedBoard = BoardManager.board.Copy ();
            List<Piece> simulatedOwnedPieces = new List<Piece> ();
            foreach (Node node in simulatedBoard.board) {
                if (node.StoredPiece != null && node.StoredPiece.BelongsTo == currentPlayer.CurrentTeam.Team)
                    simulatedOwnedPieces.Add (node.StoredPiece);
            }
            foreach (var piece in simulatedOwnedPieces) {
                List<Node> path = BoardManager.Path (piece.CurrentlyLiesIn, false, currentBoard.board);
                foreach (var node in path) {
                    Board newBoard = BoardManager.board.Copy ();
                    Piece.SimulatingMovePiece (piece, piece.CurrentlyLiesIn, node);
                    float eval = Value (piece, currentPlayer.CurrentOpponent.CurrentTeam.TeamBase);
                    minValue = Mathf.Min (eval, minValue);
                    newBoard.value = minValue;
                    listOfTempBoards.Add (newBoard);
                    List<Board> board = FindAllPossiblePaths (depth - 1, currentPlayer.CurrentOpponent, newBoard);
                    listOfTempBoards.AddRange (board);
                }

            }
        } else if (currentPlayer == currentPlayer.CurrentOpponent) {
            float maxValue = float.MinValue;
            Board simulatedBoard = BoardManager.board.Copy ();
            List<Piece> simulatedOwnedPieces = new List<Piece> ();
            foreach (Node node in simulatedBoard.board) {
                if (node.StoredPiece != null && node.StoredPiece.BelongsTo == currentPlayer.CurrentTeam.Team)
                    simulatedOwnedPieces.Add (node.StoredPiece);
            }
            foreach (var piece in simulatedOwnedPieces) {
                List<Node> path = BoardManager.Path (piece.CurrentlyLiesIn, false, currentBoard.board);
                foreach (var node in path) {
                    Board newBoard = BoardManager.board.Copy ();
                    Piece.SimulatingMovePiece (piece, piece.CurrentlyLiesIn, node);
                    float eval = Value (piece, currentPlayer.CurrentOpponent.CurrentTeam.TeamBase);
                    maxValue = Mathf.Max (eval, maxValue);
                    newBoard.value = maxValue;

                    listOfTempBoards.Add (newBoard);

                    List<Board> board = FindAllPossiblePaths (depth - 1, currentPlayer, newBoard);
                    listOfTempBoards.AddRange (board);
                }

            }
        }

        return listOfTempBoards;

    }

    class EvaluatedMove {
        List<Node> tempBoard;
        public float value;
    }

    public void GetBaseNodes () {

        if (cachedPieces == null) {
            Piece[] pieceArray = new Piece[currentTeam.TeamBase.Length];
            for (int i = 0; i < pieceArray.Length; i++) {
                pieceArray[i] = currentTeam.TeamBase[i].StoredPiece;
            }
            cachedPieces = pieceArray;
        }

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

    public Piece[] CachedPieces {
        get => cachedPieces;
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
    bool IPlayer.EndTurn {
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

    private void Start () {
        StartCoroutine (DelayedUpdate ());
    }
    IEnumerator DelayedUpdate () {
        while (true) {

            // UserManager.OnActionTaken (this);
            yield return new WaitForSeconds (0.5f);
        }

    }

    static float Value (Piece piece, Node[] targets) {
        float maxDist = 0;
        foreach (Node target in targets) {
            float dist = Vector2.Distance (piece.CurrentlyLiesIn.CurrentBoardPosition, target.CurrentBoardPosition);
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