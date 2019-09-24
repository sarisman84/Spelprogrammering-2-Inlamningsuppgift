using System;
using System.Collections.Generic;
using UnityEngine;
using static BoardModel;
public class ComputerPlayer : UserModel {

    [SerializeField] List<Board> savedResults = new List<Board> ();
    public override void OnTurnTaken () {
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
    }

    // private void Highlight (Board resultedBoard) {
    //     foreach (Piece node in resultedBoard.pieceArray) {
    //         if (originalBoard.boardViewArray == null || node == null) continue;
    //         originalBoard.boardViewArray[node.currentPosition.x, node.currentPosition.y].OnInteract ("#00ffff");
    //     }
    // }

    Board Minimax (Board currentBoard, List<Board> savedResults, int depth, List<Piece> ownedPieces, bool minimizingPlayer) {

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
        return null;
    }

    //Taken from https://rosettacode.org/wiki/Sorting_algorithms/Gnome_sort#C.23
    private void GnomeSort (List<Board> savedResults) {
        int first = 1;
        int second = 2;

        while (first < savedResults.Count) {

            if (savedResults[first - 1].value <= savedResults[first].value) {
                first = second;
                second++;
            } else {
                Board temp = savedResults[first - 1];
                savedResults[first - 1] = savedResults[first];
                savedResults[first] = temp;
                first -= 1;
                if (first == 0) {
                    first = 1;
                    second = 2;
                }
            }
        }
    }

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

    float Evaluate (Vector2Int startPos, List<Vector2Int> endPos) {
        float maxDis = float.MinValue;
        foreach (Vector2Int pos in endPos) {
            if (originalBoard.pieceArray[pos.x, pos.y] != null && originalBoard.pieceArray[pos.x, pos.y].belongsTo == currentTeam) continue;
            float dis = Vector2Int.Distance (startPos, pos);
            if (dis > maxDis)
                maxDis = dis;
        }
        return maxDis;
    }

}