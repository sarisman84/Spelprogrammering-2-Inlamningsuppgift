using System;
using UnityEngine;

namespace ChineseCheckers
{
    public static class UserManager
    {



        /// <summary>
        /// Changes the highlight mode of a selectedNode based on set parameters.
        /// </summary>
        /// <param name="go">The selectedNode in question</param>
        /// <param name="isHighlighted">If said node will be highlighted or not</param>
        static void OnInteract(Node go, bool isHighlighted)
        {
            if (go == null) return;
            go.HighlightNode(Color.green, isHighlighted);
        }


        public static TeamGenerator SetOpponent(IPlayer player)
        {
            if (GameManager.allPlayers == null) return null;
            foreach (IPlayer _player in GameManager.allPlayers)
            {
                Team opponent = _player.CurrentTeam.Team;
                switch (player.CurrentTeam.Team)
                {

                    case Team.Red:
                        if (opponent != Team.Blue) break;
                        return _player.CurrentTeam;

                    case Team.Blue:
                        if (opponent != Team.Red) break;
                        return _player.CurrentTeam;

                    case Team.Magenta:
                        if (opponent != Team.Green) break;
                        return _player.CurrentTeam;

                    case Team.Orange:
                        if (opponent != Team.Yellow) break;
                        return _player.CurrentTeam;

                    case Team.Yellow:
                        if (opponent != Team.Orange) break;
                        return _player.CurrentTeam;

                    case Team.Green:
                        if (opponent != Team.Magenta) break;
                        return _player.CurrentTeam;

                }

            }
            return null;

        }

        /// <summary>
        /// Resets the cached list of valid moves by disabling its colors and then emptying the array.
        /// </summary>
        /// <param name="cachedValidMoves">A reference to the list of valid positions</param>
        public static void ResetValidMoves(ref Node[] cachedValidMoves)
        {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves)
            {
                if (node == null) continue;
                node.HighlightNode(new Color(), false);
            }
            cachedValidMoves = null;
        }

        /// <summary>
        /// Resets the highlight color of the items within the array of valid nodes.false 
        /// </summary>
        /// <param name="cachedValidMoves">A reference to the list of valid positions</param>
        static void ResetHigtlightOnValidMoves(ref Node[] cachedValidMoves)
        {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves)
            {
                node.HighlightNode(new Color(), false);
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
        public static Node AttemptToGetPiece(Node selectedNode, Team team, bool doneFirstMove, bool highlight, ref Node[] cachedValidMoves)
        {
            cachedValidMoves = null;
            Node newNode = null;
            ResetHigtlightOnValidMoves(ref cachedValidMoves);
            // Debug.Log ($" {selectedNode}");
            newNode = (selectedNode.StoredPiece != null && selectedNode.StoredPiece.BelongsTo == team) ? selectedNode : newNode;
            OnInteract(newNode, highlight);
            // Debug.Log(selectedNode);
            if (newNode != null)
                cachedValidMoves = BoardManager.ValidMoves(selectedNode, ref doneFirstMove, highlight);
            doneFirstMove = true;
            return newNode;

        }
        /// <summary>
        /// When called, this method attemps to get a valid target by comparing if the inputed node is equal to one of the valid nodes inside the cachedValidNodes array.
        /// </summary>
        /// <param name="selectedNode">The inputed node to be tested</param>
        /// <param name="cachedValidMoves">A reference to the array of valid nodes</param>
        /// <returns>Either the selectedNode if it does find a valid correlation or null if it doesnt find any.</returns>
        public static Node AttemptToGetTarget(Node selectedNode, ref Node[] cachedValidMoves)
        {
            if (cachedValidMoves == null) throw new NullReferenceException("List of valid moves is empty");
            foreach (Node validNode in cachedValidMoves)
            {
                if (selectedNode == validNode && selectedNode.StoredPiece == null)
                {
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
        public static void OnActionTaken(IPlayer player)
        {
            switch (player)
            {

                case HumanPlayer humanPlayer:
                    OnHumanActionTaken(player);
                    break;
                case CompPlayer computer:
                    OnComputerActionTaken(player);
                    break;
            }

        }

        /// <summary>
        /// Gets called when the player is of type Computer.
        /// </summary>
        /// <param name="player">The type of player that use the IPlayer interface.</param>
        private static void OnComputerActionTaken(IPlayer player)
        {
            Node selectedPiece = player.SelectedPiece, desiredTarget = player.DesiredTarget;
            if (selectedPiece != null && desiredTarget != null)
            {

                Piece.MovePiece(selectedPiece.StoredPiece, selectedPiece, desiredTarget);
                player.SelectedPiece = null;
                //player.HasDoneFirstMove = true;
                player.CachedValidMoves = null;
                player.DesiredTarget = null;
            }
        }
        /// <summary>
        /// Gets called when the player is of type Human.
        /// </summary>
        /// <param name="player">he type of player that use the IPlayer interface.</param>
        private static void OnHumanActionTaken(IPlayer player)
        {

            if (player.SelectedPiece != null && player.DesiredTarget != null)
            {

                Piece.MovePiece(player.SelectedPiece.StoredPiece, player.SelectedPiece, player.DesiredTarget);
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
        public static void WhenTurnEnds(ref bool doneFirstMove, ref Node savedNode, ref Node savedTargetNode, ref Node[] cachedValidNodes)
        {
            //Reset everything
            doneFirstMove = false;
            ResetValidMoves(ref cachedValidNodes);
            OnInteract(savedNode, false);
            if (savedNode != null)
            {
                savedNode.HighlightNode(new Color(), false);
                savedNode = null;
            }
            if (savedTargetNode != null)
            {
                savedTargetNode = null;
            }

        }

    }
}