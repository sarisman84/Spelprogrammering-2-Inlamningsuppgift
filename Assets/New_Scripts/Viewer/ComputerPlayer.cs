using System;
using System.Collections.Generic;
using UnityEngine;
using static TestBoardModel;

public class ComputerPlayer : UserModel {

    void AtTheStartOfTurn () {
        int index = GetValidOpponent (TestGameModel.currPlayerIndex + 1);
        Move newMove = //Minimax (new Move (), this, TestManager.ins.allPlayers[index], 1, float.MinValue, float.MaxValue, true); 
            Minimax (new Move (), this, TestManager.ins.allPlayers.FindAll (u => u != this && u.HasPlayerWon () == false), 1, float.MinValue, float.MaxValue, true);

        Vector2Int currentPiece = newMove.currentPiece;
        Vector2Int target = newMove.target;
        //Debug.Log($"From {currentTeam}: {newMove.value}");
        MovePiece (currentPiece, target, OwnedViewPieces);

        EndTurn ();
    }

    /// <summary>
    /// Checks if the next opponent has already won, if it has, check the next one.
    /// Repeat until you find one that has not won.
    /// </summary>
    /// <param name="x">The current player that we are checking.</param>
    /// <returns></returns>
    private int GetValidOpponent (int x) {

        if (x >= TestGameModel.amountOfPlayers || x < 0) x = 0;
        if (!TestManager.ins.allPlayers[x].HasPlayerWon ()) {
            return x;
        }
        return GetValidOpponent (x + 1);
    }

    public override void StartTurn () {
        opponent = opponent ?? (TestManager.ins.allPlayers.Count % 2 == 1) ? TestManager.ins.allPlayers[UnityEngine.Random.Range (1, TestManager.ins.allPlayers.Count)] : TestManager.ins.allPlayers.Find (p => p.currentTeam == GetOpponent (this));
        //Debug.Log("Starting Turn");
        AtTheStartOfTurn ();
    }

    public override void EndTurn () {
        TestGameModel.PlayerDone ();
    }

    /// <summary>
    /// Mainline method: A search algoritm that tries to find the best possible move for this particular instance.
    /// </summary>
    /// <param name="t">Used for recursion.</param>
    /// <param name="player">A reference to this current player.</param>
    /// <param name="otherPlayers">A reference to the another player.</param>
    /// <param name="depth">How many layers of recursion are we doing?</param>
    /// <param name="alpha"> Pruning used to lessen searching.</param>
    /// <param name="beta">Pruning used to lessen searching.</param>
    /// <param name="maximizingPlayer"> Which player are we going to search first?</param>
    /// <returns>The best possible move.</returns>
    Move Minimax (Move t, UserModel player, List<UserModel> otherPlayers, int depth, float alpha, float beta, bool maximizingPlayer) {
        if (depth == 0) return t;
        List<Move> results;
        Move nextPontetialTurn = new Move ();
        Move potentialTurn;

        if (maximizingPlayer) {
            results = t.Expand (player);
            if (results.Count == 0) return t;
            float maxEval = float.MinValue;
            foreach (Move turn in results) {
                potentialTurn = Minimax (turn, player, otherPlayers, depth - 1, alpha, beta, false);
                if (potentialTurn != null && potentialTurn.value > maxEval) {
                    nextPontetialTurn = turn;
                    maxEval = potentialTurn.value;
                }
                if (potentialTurn.value > alpha) {
                    alpha = potentialTurn.value;
                }
                if (beta <= alpha) {
                    break;
                }
            }
        } else {
            for (int i = 0; i < otherPlayers.Count; i++) {
                UserModel otherPlayer = otherPlayers[i];
                results = t.Expand (otherPlayer);
                if (results.Count == 0) return t;
                float minEval = float.MaxValue;
                foreach (Move turn in results) {

                    potentialTurn = Minimax (turn, player, otherPlayers, depth - 1, alpha, beta, true);
                    if (potentialTurn != null && potentialTurn.value < minEval) {
                        nextPontetialTurn = turn;
                        minEval = potentialTurn.value;
                    }
                    if (potentialTurn.value < beta) {
                        beta = potentialTurn.value;
                    }

                    if (beta <= alpha) {
                        break;
                    }

                }
            }

        }
        return nextPontetialTurn;

    }

    Move Minimax (Move t, UserModel player, UserModel otherPlayer, int depth, float alpha, float beta, bool maximizingPlayer) {
        if (depth == 0) return t;
        List<Move> results;
        Move nextPontetialTurn = new Move ();
        Move potentialTurn;

        if (maximizingPlayer) {
            results = t.Expand (player);
            if (results.Count == 0) return t;
            float maxEval = float.MinValue;
            foreach (Move turn in results) {
                potentialTurn = Minimax (turn, player, otherPlayer, depth - 1, alpha, beta, false);
                if (potentialTurn != null && potentialTurn.value > maxEval) {
                    nextPontetialTurn = turn;
                    maxEval = potentialTurn.value;
                }
                if (potentialTurn.value > alpha) {
                    alpha = potentialTurn.value;
                }
                if (beta <= alpha) {
                    break;
                }
            }
        } else {
            results = t.Expand (otherPlayer);
            if (results.Count == 0) return t;
            float minEval = float.MaxValue;
            foreach (Move turn in results) {

                potentialTurn = Minimax (turn, player, otherPlayer, depth - 1, alpha, beta, true);
                if (potentialTurn != null && potentialTurn.value < minEval) {
                    nextPontetialTurn = turn;
                    minEval = potentialTurn.value;
                }
                if (potentialTurn.value < beta) {
                    beta = potentialTurn.value;
                }

                if (beta <= alpha) {
                    break;
                }

            }
        }
        return nextPontetialTurn;

    }

    //Another variation of minimax, using a sorting algoritm as its base. I am no longer using it but am keeping it for reference sake.
    Move Minimax (Move t, UserModel player, UserModel otherPlayer, int depth, bool maximizingPlayer) {
        if (depth == 0) return t;
        List<Move> results;
        Move nextPontetialTurn = new Move ();
        if (maximizingPlayer) {
            results = t.Expand (player);
            if (results.Count == 0) return t;
            GnomeSort (results);
            return Minimax (results[0], player, otherPlayer, depth - 1, false);
        }
        results = t.Expand (otherPlayer);
        if (results.Count == 0) return t;
        GnomeSort (results);
        return Minimax (results[results.Count - 1], player, otherPlayer, depth - 1, true);
    }

    /// <summary>
    /// A sorting algorithm meant to sort from best to worst result depending on variables within the list.
    /// </summary>
    /// <param name="aList">The list to sort</param>
    /// <typeparam name="S">The type of said list</typeparam>
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