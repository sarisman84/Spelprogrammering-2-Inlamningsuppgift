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

    public List<Node> GetPath (Node source, List<Node> savedResults, bool searchForGeneralMoves, bool highlight) {
        for (int curDir = 0; curDir < 6; curDir++) {
            Node potentialNode = GetValidMove (source, savedResults, curDir);
            if (potentialNode == null) continue;
            if (originalBoard.GetPiece (potentialNode.pos) != null) {
                Node newSource = GetValidMove (potentialNode, savedResults, curDir);
                if (newSource == null) continue;
                if (originalBoard.GetPiece (newSource.pos) == null) {
                    savedResults.Add (newSource);
                    if (highlight)
                        originalBoard.boardViewArray[newSource.pos.x, newSource.pos.y].OnInteract ("#ffcc99");
                    List<Node> newResults = GetPath (newSource, savedResults, false, highlight);
                    savedResults.AddRange (newResults);

                }
                continue;
            }
            if (searchForGeneralMoves) {
                savedResults.Add (potentialNode);
                if (highlight)
                    originalBoard.boardViewArray[potentialNode.pos.x, potentialNode.pos.y].OnInteract ("#ffcc99");
            }

        }

        return savedResults;
    }

    Node GetValidMove (Node source, List<Node> savedResults, int index) {
        if (source == null || OutOfBounds (source.pos)) return null;
        Vector2Int boardDir = source.pos + BoardDirection (source.pos, index);
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
                    computer.opponentsBase.Add (node.pos);

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
                human.opponentsBase.Add (node.pos);

        }
        return human;

    }

    public static UserModel GetOpponent (UserModel currentPlayer) {
        foreach (var player in GameManagercs.allPlayers) {
            switch (currentPlayer.currentTeam) {

                case Team.Red:
                    if (player.currentTeam == Team.Green) return player;
                    break;

                case Team.Orange:
                    if (player.currentTeam == Team.Blue) return player;
                    break;

                case Team.Yellow:
                    if (player.currentTeam == Team.Magenta) return player;
                    break;

                case Team.Green:
                    if (player.currentTeam == Team.Red) return player;
                    break;

                case Team.Blue:
                    if (player.currentTeam == Team.Orange) return player;
                    break;

                case Team.Magenta:
                    if (player.currentTeam == Team.Yellow) return player;
                    break;
            }
        }
        return null;
    }

    protected void MovePiece (ref Piece selectedPiece, ref Node selectedNode, ref List<Node> path) {
        if (selectedPiece == null || selectedNode == null) return;

        Piece pieceToMove = selectedPiece;
        Node target = selectedNode;
        this.playerPieces.Remove (selectedPiece);
        PieceObject pieceViewToMove = originalBoard.GetVisualPiece (pieceToMove.pos);
        selectedNode = null;
        selectedPiece = null;

        //Update pieceArray
        originalBoard.RemovePieceAt (pieceToMove.pos);
        pieceToMove.pos = target.pos;
        originalBoard.InsertPieceAt (pieceToMove.pos, pieceToMove);
        this.playerPieces.Add (pieceToMove);

        //Update Visuals
        originalBoard.RemovePieceViewAt (pieceViewToMove.currentBoardPosition);
        pieceViewToMove.currentBoardPosition = target.pos;
        pieceViewToMove.transform.position = target.worldPos;
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
        PieceObject pieceViewToMove = originalBoard.GetVisualPiece (pieceToMove.pos);
        selectedNode = null;
        selectedPiece = null;

        //Update pieceArray
        originalBoard.RemovePieceAt (pieceToMove.pos);
        pieceToMove.pos = target.pos;
        originalBoard.InsertPieceAt (pieceToMove.pos, pieceToMove);
        this.playerPieces.Add (pieceToMove);

        //Update Visuals
        originalBoard.RemovePieceViewAt (pieceViewToMove.currentBoardPosition);
        pieceViewToMove.currentBoardPosition = target.pos;
        pieceViewToMove.transform.position = target.worldPos;
        originalBoard.InsertPieceViewAt (pieceViewToMove.currentBoardPosition, pieceViewToMove);

    }

    public void SimulateMove (ref Piece selectedPiece, ref Node selectedNode, ref Board simulatedBoard) {
        if (selectedPiece == null || selectedNode == null) return;

        //Update pieceArray
        simulatedBoard.RemovePieceAt (selectedPiece.pos);
        simulatedBoard.InsertPieceAt (selectedPiece.pos, selectedPiece);

    }

    protected void ResetPath (ref List<Node> path) {
        if (path != null) {
            foreach (Node nodeObj in path) {
                originalBoard.boardViewArray[nodeObj.pos.x, nodeObj.pos.y].OnInteract ();
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