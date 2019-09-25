using System;
using System.Collections.Generic;
using UnityEngine;
using static BoardModel;
public class ComputerPlayer : UserModel {

    [SerializeField] List<Board> savedResults = new List<Board> ();
    public override void OnTurnTaken () {
        #region  OldCode
        // Board resultedBoard = Minimax (originalBoard, savedResults, 2, playerPieces, true);
        // Highlight (resultedBoard);
        // Vector2Int target = Vector2Int.zero;
        // Vector2Int piecePos = Vector2Int.zero;
        // foreach (Piece piece in resultedBoard.pieceArray) {
        //     if (piece != null && originalBoard.pieceArray[piece.currentPosition.x, piece.currentPosition.y] != resultedBoard.pieceArray[piece.currentPosition.x, piece.currentPosition.y]) target = piece.currentPosition;
        // }
        // foreach (Piece piece in originalBoard.pieceArray) {
        //     if (piece != null && originalBoard.pieceArray[piece.currentPosition.x, piece.currentPosition.y] != resultedBoard.pieceArray[piece.currentPosition.x, piece.currentPosition.y]) piecePos = piece.currentPosition;
        // }

        // MovePiece (originalBoard.GetPiece (piecePos), originalBoard.GetNode (target));
        // Debug.Log (resultedBoard.value);
        #endregion

        Turn test = Minimax (new Turn (), this, UserModel.GetOpponent (this), 2, false);
     
        originalBoard.GetVisualNode (test.movedPiece.pos).OnInteract ("#ff00ff");
        originalBoard.GetVisualNode (test.target.pos).OnInteract ("#00ccff");

        Piece selectedPiece = originalBoard.GetPiece(test.movedPiece.pos);
        Node selectedTarget = originalBoard.GetNode(test.target.pos);
        MovePiece(selectedPiece, selectedTarget);
    }

    // private void Highlight (Board resultedBoard) {
    //     foreach (Piece node in resultedBoard.pieceArray) {
    //         if (originalBoard.boardViewArray == null || node == null) continue;
    //         originalBoard.boardViewArray[node.currentPosition.x, node.currentPosition.y].OnInteract ("#00ffff");
    //     }
    // }

    Turn Minimax (Turn t, UserModel player, UserModel otherPlayer, int depth, bool minimizingPlayer) {
        #region OldCode

        // if (depth == 0) return savedResults[0];

        // if (minimizingPlayer) {

        //     foreach (Piece piece in ownedPieces) {
        //         foreach (Node validNode in GetPath (currentBoard.GetNode (piece.currentPosition), new List<Node> (), true)) {
        //             //Create a simulation of a board.
        //             Board simulatedBoard = currentBoard.CreateSimulation ();
        //             //Get the proper references for both piece and node from the simulated board.
        //             Piece simulatedPiece = simulatedBoard.GetPiece (piece.currentPosition);
        //             Node simulatedNode = simulatedBoard.GetNode (validNode.currentPosition);

        //             //Do a movement logic on the simulated board.
        //             simulatedBoard.RemovePieceAt (simulatedPiece.currentPosition);
        //             simulatedPiece.currentPosition = simulatedNode.currentPosition;
        //             simulatedBoard.InsertPieceAt (simulatedPiece.currentPosition, simulatedPiece);

        //             //Evaluate the current position of the piece.
        //             float eval = Evaluate (simulatedPiece.currentPosition, this.opponentsBase);
        //             simulatedBoard.value = eval;

        //             //Add the simulated board to a list of boards.
        //             savedResults.Add (simulatedBoard);
        //         }
        //     }
        //     //Sort list.
        //     GnomeSort (savedResults);
        //     savedResults.Add (Minimax (savedResults[savedResults.Count - 1], savedResults, depth - 1, ownedPieces, false));

        // }

        // if (!minimizingPlayer) {
        //     foreach (Piece piece in ownedPieces) {
        //         foreach (Node validNode in GetPath (currentBoard.GetNode (piece.currentPosition), new List<Node> (), true)) {
        //             //Create a simulation of a board.
        //             Board simulatedBoard = currentBoard.CreateSimulation ();
        //             //Get the proper references for both piece and node from the simulated board.
        //             Piece simulatedPiece = simulatedBoard.GetPiece (piece.currentPosition);
        //             Node simulatedNode = simulatedBoard.GetNode (validNode.currentPosition);

        //             //Do a movement logic on the simulated board.
        //             simulatedBoard.RemovePieceAt (simulatedPiece.currentPosition);
        //             simulatedPiece.currentPosition = simulatedNode.currentPosition;
        //             simulatedBoard.InsertPieceAt (simulatedPiece.currentPosition, simulatedPiece);

        //             //Evaluate the current position of the piece.
        //             float eval = Evaluate (simulatedPiece.currentPosition, this.opponentsBase);
        //             simulatedBoard.value = eval;

        //             //Add the simulated board to a list of boards.
        //             savedResults.Add (simulatedBoard);
        //         }
        //     }
        //     //Sort list.
        //     GnomeSort (savedResults);
        //     savedResults.Add (Minimax (savedResults[0], savedResults, depth - 1, ownedPieces, true));
        // }
        // GnomeSort (savedResults);
        // return savedResults[0];
        #endregion

        if (depth == 0) { return t; }

        if (minimizingPlayer) {
            List<Turn> mainResults = t.Expand (player, originalBoard);
            if (mainResults.Count == 0) return t;
            GnomeSort (mainResults);
            return Minimax (mainResults[0], player, otherPlayer, depth - 1, false);
        }
        List<Turn> results = t.Expand (otherPlayer, originalBoard);
        if (results.Count == 0) return t;
        GnomeSort (results);
        return Minimax (results[results.Count - 1], player, otherPlayer, depth - 1, true);

    }

    //Taken from https://rosettacode.org/wiki/Sorting_algorithms/Gnome_sort#C.23

    public static void GnomeSort<S> (List<S> aList) where S : IComparable {
        int first = 1;
        int second = 2;

        while (first < aList.Count) {
            if (aList[first - 1].CompareTo (aList[first]) <= 0) {
                first = second;
                second++;
            } else {
                S temp = aList[first - 1];
                aList[first - 1] = aList[first];
                aList[first] = temp;
                first -= 1;
                if (first == 0) {
                    first = 1;
                    second = 2;
                }
            }
        }
    }

}