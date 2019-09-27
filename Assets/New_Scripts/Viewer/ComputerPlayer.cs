using System;
using System.Collections.Generic;
using UnityEngine;
using static BoardModel;
public class ComputerPlayer : UserModel
{

    [SerializeField] List<Board> savedResults = new List<Board>();
    public override void OnTurnTaken(ref int index)
    {
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

        Turn test = Minimax(new Turn(), this, UserModel.GetOpponent(this), 2, true);



        Piece selectedPiece = originalBoard.GetPiece(test.movedPiece);
        Node selectedTarget = originalBoard.GetNode(test.target);
        MovePiece(selectedPiece, selectedTarget);
        index++;
    }

    // private void Highlight (Board resultedBoard) {
    //     foreach (Piece node in resultedBoard.pieceArray) {
    //         if (originalBoard.boardViewArray == null || node == null) continue;
    //         originalBoard.boardViewArray[node.currentPosition.x, node.currentPosition.y].OnInteract ("#00ffff");
    //     }
    // }

    Turn Minimax(Turn t, UserModel player, UserModel otherPlayer, int depth, bool minimizingPlayer)
    {


        if (depth == 0) { return t; }

        if (minimizingPlayer)
        {
            List<Turn> mainResults = t.Expand(player, originalBoard);
            if (mainResults.Count == 0) return t;
            GnomeSort(mainResults);

            return Minimax(mainResults[0], player, otherPlayer, depth - 1, false);
        }
        List<Turn> results = t.Expand(otherPlayer, originalBoard);
        if (results.Count == 0) return t;
        GnomeSort(results);

        return Minimax(results[results.Count - 1], player, otherPlayer, depth - 1, true);

    }



    Turn Minimax(Turn t, UserModel player, UserModel otherPlayer, int depth, float alpha, float beta, bool minimizingPlayer)
    {
        if (depth == 0) return t;
        List<Turn> results;
        Turn nextPontetialTurn = null;
        if (minimizingPlayer)
        {
            results = t.Expand(player, originalBoard);
            if (results.Count == 0) return t;
            float minEval = float.MaxValue;
            foreach (Turn turn in results)
            {
                float eval = turn.Value;
                nextPontetialTurn = (minEval > eval) ? turn : nextPontetialTurn;
                minEval = (minEval > eval) ? eval : minEval;
                beta = (beta > eval) ? eval : beta;
                if (beta <= alpha) break;

            }
            return Minimax(nextPontetialTurn, player, otherPlayer, depth - 1, alpha, beta, false);
        }
        results = t.Expand(otherPlayer, originalBoard);
        if (results.Count == 0) return t;
        float maxEval = float.MinValue;
        foreach (Turn turn in results)
        {
            float eval = turn.Value;
            nextPontetialTurn = (maxEval < eval) ? turn : nextPontetialTurn;
            maxEval = (maxEval < eval) ? eval : maxEval;
            alpha = (alpha < eval) ? eval : alpha;
            if (beta <= alpha) break;
        }
        return Minimax(nextPontetialTurn, player, otherPlayer, depth - 1, alpha, beta, true);
    }

    //Taken from https://rosettacode.org/wiki/Sorting_algorithms/Gnome_sort#C.23

    public static void GnomeSort<S>(List<S> aList) where S : IComparable
    {
        int first = 1;
        int second = 2;

        while (first < aList.Count)
        {
            if (aList[first - 1].CompareTo(aList[first]) <= 0)
            {
                first = second;
                second++;
            }
            else
            {
                S temp = aList[first - 1];
                aList[first - 1] = aList[first];
                aList[first] = temp;
                first -= 1;
                if (first == 0)
                {
                    first = 1;
                    second = 2;
                }
            }
        }
    }

}