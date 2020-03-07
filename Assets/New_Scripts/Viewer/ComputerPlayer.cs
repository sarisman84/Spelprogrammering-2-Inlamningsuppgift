using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TestBoardModel;

public class ComputerPlayer : UserModel {

    void ExecuteComputersTurn () {

        Move newMove = Minimax (new Move (), test_OriginalBoard.Clone (), TestManager.ins.allPlayers, 1, this,
            TestManager.ins.allPlayers.FindIndex (0, TestManager.ins.allPlayers.Count, p => p == this),
            float.MinValue, float.MaxValue, true);
        if (newMove == null) { EndTurn (); return; }
        Vector2Int currentPiece = newMove.currentPiece;
        Vector2Int target = newMove.target;

        MovePiece (currentPiece, target, OwnedViewPieces);

        EndTurn ();
    }

    public override void StartTurn () {

        ExecuteComputersTurn ();
    }

    public override void EndTurn () {
        TestGameModel.PlayerDone ();
    }

    /// <summary>
    /// Minimax algorithm. The method goes through all players recursivly. Maximizes the owner of this method and minimizes the other players.
    /// </summary>
    /// <param name="t">A parameter that is used recursivly. Is saved for further iterations.</param>
    /// <param name="simulatedBoard">A parameter that is used recursivly. Is saved for further iterations.</param>
    /// <param name="playerList">A list of all players currently in the game.</param>
    /// <param name="depth">How many itterations will the method recurve.</param>
    /// <param name="startingPlayer">The current starting player. Is used to identify who is the owner of this method in further itterations.</param>
    /// <param name="nextPlayer">A variable that increments by one for every itteration. Is used to loop through every player.</param>
    /// <param name="alpha">Alpha/Beta pruning.</param>
    /// <param name="beta">Alpha/Beta pruning.</param>
    /// <param name="maximizingPlayer">A parameter that is used recursivly. Is saved for further iterations.</param>
    /// <returns>The best posible move for the owner of this method.</returns>
    Move Minimax (Move t, List<BoardPiece> simulatedBoard, List<UserModel> playerList, int depth,
        UserModel startingPlayer, int nextPlayer, float alpha, float beta, bool maximizingPlayer) {

        if (depth == 0) return t;

        List<Move> results;
        Move nextPontetialTurn = new Move ();
        Move potentialTurn;

        nextPlayer = (playerList.Count <= nextPlayer) ? 0 : nextPlayer;

        if (maximizingPlayer) {
            UserModel player = playerList[nextPlayer];
            results = t.Expand (player, ref simulatedBoard);
            if (results.Count == 0) return t;
            float maxEval = float.MinValue;
            foreach (Move turn in results) {
                potentialTurn = Minimax (turn, simulatedBoard, playerList, depth - 1, startingPlayer, nextPlayer + 1, alpha, beta, playerList[playerList.FindIndex (p => p == startingPlayer)]);
                if (potentialTurn != null && potentialTurn.value > maxEval) {
                    nextPontetialTurn = turn;
                    maxEval = potentialTurn.value;
                }
                if (potentialTurn != null && potentialTurn.value > alpha) {
                    alpha = potentialTurn.value;
                }
                if (beta <= alpha) {
                    break;
                }
            }
        } else {

            UserModel otherPlayer = playerList[nextPlayer];
            results = t.Expand (otherPlayer, ref simulatedBoard);
            if (results.Count == 0) return t;
            float minEval = float.MaxValue;
            foreach (Move turn in results) {

                potentialTurn = Minimax (turn, simulatedBoard, playerList, depth - 1, startingPlayer, nextPlayer + 1, alpha, beta, playerList[playerList.FindIndex (p => p == startingPlayer)]);
                if (potentialTurn != null && potentialTurn.value < minEval) {
                    nextPontetialTurn = turn;
                    minEval = potentialTurn.value;
                }
                if (potentialTurn != null && potentialTurn.value < beta) {
                    beta = potentialTurn.value;
                }

                if (beta <= alpha) {
                    break;
                }

            }

        }
        return nextPontetialTurn;
    }

}