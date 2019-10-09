using System;
using System.Collections.Generic;
using UnityEngine;
using static TestBoardModel;

public class ComputerPlayer : UserModel
{
    #region OldCode

    // [SerializeField] List<Board> savedResults = new List<Board> ();

    // UserModel opponent;

    // public void OnTurnTaken () {

    //     Turn test = Minimax (new Turn (), this, opponent, 1, float.MinValue, float.MaxValue, true);

    //     Piece selectedPiece = originalBoard.GetPiece (test.movedPiece);
    //     Node selectedTarget = originalBoard.GetNode (test.target);
    //     //Debug.Log($"Piece {selectedPiece.belongsTo} at {selectedPiece.pos} moves to {selectedTarget} at {selectedTarget.pos}");
    //     MovePiece (selectedPiece, selectedTarget);
    //     EndTurn();

    // }

    // public override void StartTurn () {
    //     opponent = opponent ?? UserModel.GetOpponent (this);
    //     OnTurnTaken ();
    // }

    // public override void EndTurn () {
    //     GameModel.PlayerDone();
    // }

    // Turn Minimax (Turn t, UserModel player, UserModel otherPlayer, int depth, bool minimizingPlayer) {

    //     if (depth == 0) { return t; }

    //     if (minimizingPlayer) {
    //         List<Turn> mainResults = t.Expand (player, originalBoard);
    //         if (mainResults.Count == 0) return t;
    //         GnomeSort (mainResults);

    //         return Minimax (mainResults[0], player, otherPlayer, depth - 1, false);
    //     }
    //     List<Turn> results = t.Expand (otherPlayer, originalBoard);
    //     if (results.Count == 0) return t;
    //     GnomeSort (results);

    //     return Minimax (results[results.Count - 1], player, otherPlayer, depth - 1, true);

    // }

    // Turn Minimax (Turn t, UserModel player, UserModel otherPlayer, int depth, float alpha, float beta, bool maximizingPlayer) {
    //     if (depth == 0) return t;
    //     List<Turn> results;
    //     Turn nextPontetialTurn = null;
    //     if (maximizingPlayer) {
    //         results = t.Expand (player, originalBoard);
    //         if (results.Count == 0) return t;
    //         float maxEval = float.MinValue;
    //         foreach (Turn turn in results) {
    //             float eval = turn.Value;
    //             maxEval = Mathf.Max (maxEval, eval);
    //             alpha = Mathf.Max (alpha, eval);
    //             if (beta <= alpha) break;
    //         }
    //         //Debug.Log (otherPlayer.currentTeam);
    //         nextPontetialTurn = results.Find (p => p.Value == maxEval);
    //         return Minimax (nextPontetialTurn, player, otherPlayer, depth - 1, alpha, beta, true);
    //     }
    //     results = t.Expand (otherPlayer, originalBoard);
    //     if (results.Count == 0) return t;
    //     float minEval = float.MaxValue;
    //     foreach (Turn turn in results) {
    //         float eval = turn.Value;
    //         minEval = Mathf.Min (minEval, eval);
    //         beta = Mathf.Min (beta, eval);
    //         if (beta <= alpha) break;

    //     }
    //     //Debug.Log (player.currentTeam);
    //     nextPontetialTurn = results.Find (p => p.Value == minEval);
    //     return Minimax (nextPontetialTurn, player, otherPlayer, depth - 1, alpha, beta, false);
    // }

    // //Taken from https://rosettacode.org/wiki/Sorting_algorithms/Gnome_sort#C.23

    // public static void GnomeSort<S> (List<S> aList) where S : IComparable {
    //     int first = 1;
    //     int second = 2;

    //     while (first < aList.Count) {
    //         if (aList[first - 1].CompareTo (aList[first]) <= 0) {
    //             first = second;
    //             second++;
    //         } else {
    //             S temp = aList[first - 1];
    //             aList[first - 1] = aList[first];
    //             aList[first] = temp;
    //             first -= 1;
    //             if (first == 0) {
    //                 first = 1;
    //                 second = 2;
    //             }
    //         }
    //     }
    // }
    #endregion







    void AtTheStartOfTurn()
    {
        int index = GetValidOpponent(TestGameModel.currPlayerIndex + 1);
        Move newMove = Minimax(new Move(), this, TestManager.ins.allPlayers[index], 1, float.MinValue, float.MaxValue, true);

        Vector2Int currentPiece = newMove.currentPiece;
        Vector2Int target = newMove.target;
        Debug.Log($"From {currentTeam}: {newMove.value}");
        MovePiece(currentPiece, target, OwnedViewPieces);

        EndTurn();
    }

    private int GetValidOpponent(int x)
    {

        if (x >= TestGameModel.amountOfPlayers || x < 0) x = 0;
        if (!TestManager.ins.allPlayers[x].HasPlayerWon())
        {
            return x;
        }
        return GetValidOpponent(x + 1);
    }

    public override void StartTurn()
    {
        opponent = opponent ?? (TestManager.ins.allPlayers.Count % 2 == 1) ? TestManager.ins.allPlayers[UnityEngine.Random.Range(1, TestManager.ins.allPlayers.Count)] : TestManager.ins.allPlayers.Find(p => p.currentTeam == GetOpponent(this));
        //Debug.Log("Starting Turn");
        AtTheStartOfTurn();
    }

    public override void EndTurn()
    {
        TestGameModel.PlayerDone();
    }

    Move Minimax(Move t, UserModel player, UserModel otherPlayer, int depth, float alpha, float beta, bool maximizingPlayer)
    {
        if (depth == 0) return t;
        List<Move> results;
        Move nextPontetialTurn = new Move();
        Move potentialTurn;

        if (maximizingPlayer)
        {
            results = t.Expand(player);
            if (results.Count == 0) return t;
            float maxEval = float.MinValue;
            foreach (Move turn in results)
            {

                potentialTurn = Minimax(turn, player, otherPlayer, depth - 1, alpha, beta, false);
                if (potentialTurn != null && potentialTurn.value > maxEval)
                {
                    nextPontetialTurn = turn;
                    maxEval = potentialTurn.value;
                }
                // maxEval = Mathf.Max (maxEval, eval);
                //alpha = Mathf.Max (alpha, eval);
            }
            //Debug.Log (otherPlayer.currentTeam);
            //nextPontetialTurn = results.Find (p => p.value == maxEval);
        }
        else
        {
            results = t.Expand(otherPlayer);
            if (results.Count == 0) return t;
            float minEval = float.MaxValue;
            foreach (Move turn in results)
            {

                potentialTurn = Minimax(turn, player, otherPlayer, depth - 1, alpha, beta, true);
                if (potentialTurn != null && potentialTurn.value < minEval)
                {
                    nextPontetialTurn = turn;
                    minEval = potentialTurn.value;
                }

            }
            //Debug.Log (player.currentTeam);
        }
        return nextPontetialTurn;

    }

    Move Minimax(Move t, UserModel player, UserModel otherPlayer, int depth, bool maximizingPlayer)
    {
        if (depth == 0) return t;
        List<Move> results;
        Move nextPontetialTurn = new Move();
        if (maximizingPlayer)
        {
            results = t.Expand(player);
            if (results.Count == 0) return t;
            GnomeSort(results);
            return Minimax(results[0], player, otherPlayer, depth - 1, false);
        }
        results = t.Expand(otherPlayer);
        if (results.Count == 0) return t;
        GnomeSort(results);
        return Minimax(results[results.Count - 1], player, otherPlayer, depth - 1, true);
    }

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