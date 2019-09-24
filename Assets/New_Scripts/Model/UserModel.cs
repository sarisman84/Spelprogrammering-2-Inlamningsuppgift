using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BoardModel;

public enum Team { None, Unoccupied, Red, Orange, Yellow, Green, Blue, Magenta, BigRed, BigGreen = 11 }
public abstract class UserModel : MonoBehaviour {

    public Team currentTeam;
    public List<Piece> playerPieces;

    public List<Vector2Int> opponentsBase;

   

    public abstract void OnTurnTaken ();

    public bool HasWon => opponentsBase.All (pos => originalBoard.pieceArray[pos.x, pos.y].belongsTo == currentTeam);

    protected List<Node> GetPath (Node source, List<Node> savedResults, bool searchForGeneralMoves) {
        for (int curDir = 0; curDir < 6; curDir++) {
            Node potentialNode = GetValidMove (source, savedResults, curDir);
            if (potentialNode == null) continue;
            if (originalBoard.GetPiece (potentialNode.currentPosition) != null) {
                Node newSource = GetValidMove (potentialNode, savedResults, curDir);
                if (newSource == null) continue;
                if (originalBoard.GetPiece (newSource.currentPosition) == null) {
                    savedResults.Add (newSource);
                    originalBoard.boardViewArray[newSource.currentPosition.x, newSource.currentPosition.y].OnInteract ("#ffcc99");
                    List<Node> newResults = GetPath (newSource, savedResults, false);
                    savedResults.AddRange (newResults);

                }
                continue;
            }
            if (searchForGeneralMoves) {
                savedResults.Add (potentialNode);
                originalBoard.boardViewArray[potentialNode.currentPosition.x, potentialNode.currentPosition.y].OnInteract ("#ffcc99");
            }

        }

        return savedResults;
    }

    Node GetValidMove (Node source, List<Node> savedResults, int index) {
        if (source == null || OutOfBounds (source.currentPosition)) return null;
        Vector2Int boardDir = source.currentPosition + BoardDirection (source.currentPosition, index);
        if (OutOfBounds (boardDir)) return null;
        Node foundNode = BoardModel.originalBoard.GetNode (boardDir);
        if (foundNode == source || foundNode.belongsTo == Team.None || savedResults.Any (p => p == foundNode)) return null;
        return foundNode;
    }

    private bool OutOfBounds (Vector2Int source) {
        return (source.x >= BoardModel.originalBoard.boardArray.GetLength (0) || source.x < 0 || source.y >= BoardModel.originalBoard.boardArray.GetLength (1) || source.y < 0);
    }

    Vector2Int BoardDirection (Vector2Int source, int index) {

        switch (index) {

            case 0:
                return new Vector2Int (0, -1); // Left

            case 1:
                return new Vector2Int (0, 1); // Right

            case 2:
                if (source.x % 2 == 0)
                    return new Vector2Int (-1, 0);
                return new Vector2Int (-1, 1); //Top Right
            case 3:
                if (source.x % 2 == 0)
                    return new Vector2Int (-1, -1);
                return new Vector2Int (-1, 0); //Top left

            case 4:
                if (source.x % 2 == 0)
                    return new Vector2Int (1, 0);
                return new Vector2Int (1, 1); //Bottom Right
            case 5:
                if (source.x % 2 == 0)
                    return new Vector2Int (1, -1);
                return new Vector2Int (1, 0); //Bottom Left
        }
        return Vector2Int.zero;
    }

    public static UserModel CreatePlayer (bool isComputer, Team team) {

        if (isComputer) {
            ComputerPlayer computer = new GameObject ($"Player {team}").AddComponent<ComputerPlayer> ();
            computer.currentTeam = team;
            Team compOpponent = GetOpponent (team);
            computer.opponentsBase = new List<Vector2Int> ();
            computer.playerPieces = new List<Piece> ();
            foreach (Node node in originalBoard.boardArray) {
                if (node.belongsTo == compOpponent)
                    computer.opponentsBase.Add (node.currentPosition);

            }
            Debug.Log (computer.opponentsBase.Count);
            return computer;
        }

        HumanPlayer human = new GameObject ($"Player {team}").AddComponent<HumanPlayer> ();
        human.currentTeam = team;
        Team humanOpponent = GetOpponent (team);
        human.opponentsBase = new List<Vector2Int> ();
        human.playerPieces = new List<Piece> ();
        foreach (Node node in originalBoard.boardArray) {
            if (node.belongsTo == humanOpponent)
                human.opponentsBase.Add (node.currentPosition);

        }
        return human;

    }

    protected void MovePiece (ref Piece selectedPiece, ref Node selectedNode, ref List<Node> path) {
        if (selectedPiece == null || selectedNode == null) return;

        Piece pieceToMove = selectedPiece;
        Node target = selectedNode;
        this.playerPieces.Remove (selectedPiece);
        PieceObject pieceViewToMove = originalBoard.GetVisualPiece (pieceToMove.currentPosition);
        selectedNode = null;
        selectedPiece = null;

        //Update pieceArray
        originalBoard.RemovePieceAt (pieceToMove.currentPosition);
        pieceToMove.currentPosition = target.currentPosition;
        originalBoard.InsertPieceAt (pieceToMove.currentPosition, pieceToMove);
        this.playerPieces.Add (pieceToMove);

        //Update Visuals
        originalBoard.RemovePieceViewAt (pieceViewToMove.currentBoardPosition);
        pieceViewToMove.currentBoardPosition = target.currentPosition;
        pieceViewToMove.transform.position = target.worldPosition;
        originalBoard.InsertPieceViewAt (pieceViewToMove.currentBoardPosition, pieceViewToMove);

        ResetPath (ref path);
    }
    protected void MovePiece (Piece selectedPiece, Node selectedNode) {
        Debug.Log (selectedNode);
        Debug.Log (selectedPiece);
        if (selectedPiece == null || selectedNode == null) return;

        Piece pieceToMove = selectedPiece;
        Node target = selectedNode;
        this.playerPieces.Remove (selectedPiece);
        PieceObject pieceViewToMove = originalBoard.GetVisualPiece (pieceToMove.currentPosition);
        selectedNode = null;
        selectedPiece = null;

        //Update pieceArray
        originalBoard.RemovePieceAt (pieceToMove.currentPosition);
        pieceToMove.currentPosition = target.currentPosition;
        originalBoard.InsertPieceAt (pieceToMove.currentPosition, pieceToMove);
        this.playerPieces.Add (pieceToMove);

        //Update Visuals
        originalBoard.RemovePieceViewAt (pieceViewToMove.currentBoardPosition);
        pieceViewToMove.currentBoardPosition = target.currentPosition;
        pieceViewToMove.transform.position = target.worldPosition;
        originalBoard.InsertPieceViewAt (pieceViewToMove.currentBoardPosition, pieceViewToMove);

    }

    protected void ResetPath (ref List<Node> path) {
        if (path != null) {
            foreach (Node nodeObj in path) {
                originalBoard.boardViewArray[nodeObj.currentPosition.x, nodeObj.currentPosition.y].OnInteract ();
            }
            path.Clear ();
        }
    }

    protected static Team GetOpponent (Team currentTeam) {
        switch (currentTeam) {

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
        return currentTeam;
    }
}