using System;
using UnityEngine;

namespace ChineseCheckers {
    public static class UserManager {

        static void OnInteract (Node go, bool isHighlighted) {
            if (go == null) return;
            go.HighlightNode (Color.green, isHighlighted);
        }
        public static Node AttemptToGetPiece (Node selectedNode, Team team, bool doneFirstMove, ref Node[] cachedValidMoves) {
            cachedValidMoves = null;
            Node newNode = null;
            ResetHigtlightOnValidMoves (ref cachedValidMoves);
            // Debug.Log ($" {selectedNode}");
            newNode = (selectedNode.StoredPiece != null && selectedNode.StoredPiece.BelongsTo == team) ? selectedNode : newNode;
            OnInteract (newNode, true);
            // Debug.Log(selectedNode);
            if (newNode != null)
                cachedValidMoves = BoardManager.ValidMoves (selectedNode, ref doneFirstMove);
            doneFirstMove = true;
            return newNode;

        }

        //public static Node[] cachedValidMoves;

        public static void ResetValidMoves (ref Node[] cachedValidMoves) {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                if (node == null) continue;
                node.HighlightNode (new Color (), false);
            }
            cachedValidMoves = null;
        }

        static void ResetHigtlightOnValidMoves (ref Node[] cachedValidMoves) {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                node.HighlightNode (new Color (), false);
            }
        }

        public static Node AttemptToGetTarget (Node selectedNode, Node source, ref Node[] cachedValidMoves) {
            if (cachedValidMoves == null) throw new NullReferenceException ("List of valid moves is empty");
            foreach (Node validNode in cachedValidMoves) {
                if (selectedNode == validNode && selectedNode.StoredPiece == null) {
                    return selectedNode;
                }
            }
            return null;

        }
        // public static void OnActionTaken (Node go, Team team, ref bool doneFirstMove, ref Node selectedNode, ref Node targetNode) {
        //     selectedNode = selectedNode ?? AttemptToGetPiece (go, team, doneFirstMove);
        //     targetNode = (selectedNode != null) ? AttemptToGetTarget (go, selectedNode) : targetNode;

        //     if (selectedNode != null && targetNode != null) {
        //         ResetValidMoves ();

        //         Piece.MovePiece (selectedNode.StoredPiece, selectedNode, targetNode);
        //         selectedNode.HighlightNode (new Color (), false);
        //         selectedNode = AttemptToGetPiece (targetNode, team, doneFirstMove);
        //         targetNode = null;

        //     }

        // }

        public static void OnActionTaken (IPlayer player) {
            switch (player) {

                case HumanPlayer humanPlayer:
                    OnHumanActionTaken (player);
                    break;
                case CompPlayer computer:
                    OnComputerActionTaken (player);
                    break;
            }

        }

        private static void OnComputerActionTaken (IPlayer player) {
            throw new NotImplementedException ();
        }

        private static void OnHumanActionTaken (IPlayer player) {

            if (player.SelectedPiece != null && player.DesiredTarget != null) {

                Piece.MovePiece (player.SelectedPiece.StoredPiece, player.SelectedPiece, player.DesiredTarget);
                player.HasDoneFirstMove = true;
                player.SelectedPiece = player.DesiredTarget;
                player.CachedValidMoves = null;
                player.DesiredTarget = null;
            }
        }

        // public static void OnActionTaken (Node[] playerBase, Team team, ref bool doneFirstMove, ref Node selectedNode, ref Node targetNode) {
        //     selectedNode = selectedNode ?? AttemptToGetPiece (go, team, ref doneFirstMove);
        //     targetNode = (selectedNode != null) ? AttemptToGetTarget (go, selectedNode) : targetNode;

        //     if (selectedNode != null && targetNode != null) {
        //         ResetValidMoves ();

        //         Piece.MovePiece (selectedNode.StoredPiece, selectedNode, targetNode);
        //         selectedNode.HighlightNode (new Color (), false);
        //         selectedNode = AttemptToGetPiece (targetNode, team, doneFirstMove);
        //         targetNode = null;

        //     }
        // }

        public static void WhenTurnEnds (ref bool doneFirstMove, ref Node savedNode, ref Node savedTargetNode, ref Node[] cachedValidNodes) {
            //Reset everything
            doneFirstMove = false;
            ResetValidMoves (ref cachedValidNodes);
            OnInteract (savedNode, false);
            if (savedNode != null) {
                savedNode.HighlightNode (new Color (), false);
                savedNode = null;
            }
            if (savedTargetNode != null) {
                savedTargetNode = null;
            }

        }
    }
}