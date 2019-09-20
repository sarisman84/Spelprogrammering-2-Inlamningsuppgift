using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChineseCheckers {
    public static class UserManager {

        /// <summary>
        /// Changes the highlight mode of a selectedNode based on set parameters.
        /// </summary>
        /// <param name="go">The selectedNode in question</param>
        /// <param name="isHighlighted">If said node will be highlighted or not</param>
        static void OnInteract (NodeObject go, bool isHighlighted) {
            if (go == null) return;
            go.HighlightNode (Color.green, false);
        }

        public static IPlayer SetOpponent (IPlayer player) {
            if (GameManager.allPlayers == null) return null;
            foreach (IPlayer _player in GameManager.allPlayers) {
                Team opponent = _player.CurrentTeam.Team;
                switch (player.CurrentTeam.Team) {

                    case Team.Red:
                        if (opponent != Team.Blue) break;
                        return _player;

                    case Team.Blue:
                        if (opponent != Team.Red) break;
                        return _player;

                    case Team.Magenta:
                        if (opponent != Team.Green) break;
                        return _player;

                    case Team.Orange:
                        if (opponent != Team.Yellow) break;
                        return _player;

                    case Team.Yellow:
                        if (opponent != Team.Orange) break;
                        return _player;

                    case Team.Green:
                        if (opponent != Team.Magenta) break;
                        return _player;

                }

            }
            return null;

        }

        /// <summary>
        /// Resets the cached list of valid moves by disabling its colors and then emptying the array.
        /// </summary>
        /// <param name="cachedValidMoves">A reference to the list of valid positions</param>
        public static void ResetValidMoves (ref List<Node> cachedValidMoves) {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                if (node == null) continue;
                node.Object.ResetColor ();
            }
            cachedValidMoves = null;
        }

        /// <summary>
        /// Resets the highlight color of the items within the array of valid nodes.false 
        /// </summary>
        /// <param name="cachedValidMoves">A reference to the list of valid positions</param>
        static void ResetHigtlightOnValidMoves (ref List<Node> cachedValidMoves) {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                node.Object.ResetColor ();
            }
        }

        #region AttempingToGetVariables

        /// <summary>
        /// When called, this method attemps to get a valid node as well as a list of valid moves on that valid node based on set parameters.
        /// </summary>
        /// <param name="selectedNode">The node in question that is being tested.</param>
        /// <param name="team">What team is this method being called from. </param>
        /// <param name="doneFirstMove">If the user who called this method has already moved.</param>
        /// <param name="cachedValidMoves">A reference to an array of cachedValidMoves. </param>
        /// <returns>A valid node that holds a piece of the same team. </returns>

        public static Node AttemptToGetPiece (Node selectedNode, Team team, bool highlight, ref List<Node> cachedValidMoves) {

            cachedValidMoves = null;
            Node newNode = null;
            ResetHigtlightOnValidMoves (ref cachedValidMoves);
            // Debug.Log ($" {selectedNode}");
            if (selectedNode == null) return null;
            newNode = (selectedNode.StoredPiece != null && selectedNode.StoredPiece.BelongsTo == team) ? selectedNode : newNode;
            if (newNode.Object == null) return null;
            OnInteract (newNode.Object, highlight);
            // Debug.Log(selectedNode);
            if (newNode != null)
                cachedValidMoves = BoardManager.Path (selectedNode, highlight);
            //doneFirstMove = true;
            return newNode;

        }

        /// <summary>
        /// When called, this method attemps to get a valid target by comparing if the inputed node is equal to one of the valid nodes inside the cachedValidNodes array.
        /// </summary>
        /// <param name="selectedNode">The inputed node to be tested</param>
        /// <param name="cachedValidMoves">A reference to the array of valid nodes</param>
        /// <returns>Either the selectedNode if it does find a valid correlation or null if it doesnt find any.</returns>
        public static Node AttemptToGetTarget (Node selectedNode, ref List<Node> cachedValidMoves) {
            if (cachedValidMoves == null) throw new NullReferenceException ("List of valid moves is empty");
            foreach (Node validNode in cachedValidMoves) {
                if (selectedNode == validNode && selectedNode.StoredPiece == null) {
                    return selectedNode;
                }
            }
            return null;

        }
        #endregion

        #region  OnPlayerAction
        /// <summary>
        /// Interacts with the board, a selected piece and a desired position based on what type of player calls it.
        /// </summary>
        /// <param name="player">The type of player that use the IPlayer interface.</param>
        public static void OnActionTaken (IPlayer player) {
            switch (player) {

                case HumanPlayer humanPlayer:
                    OnHumanActionTaken (player);
                    break;
                case CompPlayer computer:
                    OnComputerActionTaken (computer);
                    break;
            }

        }

        /// <summary>
        /// Gets called when the player is of type Computer.
        /// </summary>
        /// <param name="player">The type of player that use the IPlayer interface.</param>
        private static void OnComputerActionTaken (CompPlayer player) {
            Node target;
            Piece selectedPiece;
            FindOptimalPlay (player, out target, out selectedPiece);
            if (target != null && selectedPiece != null) {
                Piece.MovePiece (selectedPiece, selectedPiece.NodeReference, target);
                player.EndTurn = true;
            }

        }

        /// <summary>
        /// Attemps to find an optimal play for the computer.
        /// </summary>
        /// <param name="player"> Who the current owner is.</param>
        /// <param name="target"> Returns the destinaton of said optimal play</param>
        /// <param name="selectedPiece">Returns a chosen piece to be moved in the optimal play</param>
        static void FindOptimalPlay (IPlayer player, out Node target, out Piece selectedPiece) {
            Piece _selectedPiece = null;
            Node _target = null;
            float minValue = float.MaxValue;
            selectedPiece = _selectedPiece;
            target = _target;
            Board bestBoard = new Board ();
            List<Board> simulation = new List<Board> ();
            List<Board> listOfPaths = CompPlayer.FindAllPossiblePaths (2, player, false, BoardManager.board, simulation);

            if (listOfPaths.Count <= 0) return;
            bestBoard = listOfPaths[0];
            //Debug.Log ($"The list of all available moves is {listOfPaths.Count} long and the current best board is {bestBoard} at {bestBoard.value}.");
            List<Piece> bestBoardPieces = new List<Piece> ();
            List<Piece> boardPieces = new List<Piece> ();
            switch (player) {

                case CompPlayer computer:
                    boardPieces = computer.CachedPieces;
                    break;
                case HumanPlayer human:
                    boardPieces = human.CachedPieces;
                    break;
            }
            if (bestBoard == null) return;
            foreach (Node node in bestBoard.board) {
                if (node == null) continue;
                if (node.StoredPiece != null && node.StoredPiece.BelongsTo == player.CurrentTeam.Team)
                    bestBoardPieces.Add (node.StoredPiece);
            }

            //Search through both lists of bestBoardPieces and boardPieces and see if it can find any differences between. 
            //If it does, log the differences and use them for later.
            //Currently, this doesnt work. Will have to work on this later.

            Debug.Log ($"Check start! Selected Piece: {_selectedPiece}/ Target: {_target}");

            foreach (var piece in bestBoardPieces) {
                Vector2Int pos = piece.CurrentPosition;
                Debug.Log ($"Checking piece (best) {piece}");
                if (BoardManager.board[pos.x, pos.y] != null)
                    BoardManager.board[pos.x, pos.y].Object.HighlightNode (Color.gray, true);
                if (BoardManager.board[pos.x, pos.y] != null && BoardManager.board[pos.x, pos.y].StoredPiece == null) {
                    Debug.Log (BoardManager.board[pos.x, pos.y]);
                    _target = BoardManager.board[pos.x, pos.y];

                }
            }
            foreach (var piece in boardPieces) {
                Debug.Log ($"Checking piece (default) {piece}");
                if (piece == null) continue;
                Vector2Int pos = piece.CurrentPosition;
                if (bestBoard[pos.x, pos.y].StoredPiece == null) {
                    _selectedPiece = BoardManager.board[pos.x, pos.y].StoredPiece;

                }

            }
            Debug.Log ($"Has found optimal play! Selected Piece: {_selectedPiece}/ Target: {_target}");

        }
        /// <summary>
        /// Gets called when the player is of type Human.
        /// </summary>
        /// <param name="player">he type of player that use the IPlayer interface.</param>
        private static void OnHumanActionTaken (IPlayer player) {

            if (player.SelectedPiece != null && player.DesiredTarget != null) {

                Piece.MovePiece (player.SelectedPiece.StoredPiece, player.SelectedPiece, player.DesiredTarget);
                player.HasDoneFirstMove = true;
                player.SelectedPiece = player.DesiredTarget;
                player.CachedValidMoves = null;
                player.DesiredTarget = null;

            }
        }

        #endregion

        /// <summary>
        /// A seperate method that ends the current player's turn by resetting all used variables referenced.
        /// </summary>
        /// <param name="doneFirstMove">A reference to a player's first move </param>
        /// <param name="savedNode">A reference to a stored piece node. </param>
        /// <param name="savedTargetNode">A reference to a target node. </param>
        /// <param name="cachedValidNodes">A reference to a list of all valid moves.</param>
        public static void WhenTurnEnds (ref bool doneFirstMove, ref Node savedNode, ref Node savedTargetNode, ref List<Node> cachedValidNodes) {
            //Reset everything
            doneFirstMove = false;
            ResetValidMoves (ref cachedValidNodes);
            OnInteract (savedNode.Object, false);
            if (savedNode != null) {
                savedNode.Object.HighlightNode (new Color (), false);
                savedNode = null;
            }
            if (savedTargetNode != null) {
                savedTargetNode = null;
            }

        }

    }
}