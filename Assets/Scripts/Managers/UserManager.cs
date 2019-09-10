using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ChineseCheckers;
using UnityEngine;

namespace ChineseCheckers {
    public static class UserManager {

        static void OnInteract (Node go, bool isHighlighted) {
            if (go == null) return;
            go.HighlightNode (Color.green, isHighlighted);
        }
        static Node AttemptToGetPiece (Node selectedNode, Node.Team team, ref bool doneFirstMove) {
            cachedValidMoves = null;
            Node newNode = null;
            ResetHigtlightOnValidMoves ();
            newNode = (selectedNode.StoredPiece != null && selectedNode.StoredPiece.BelongsTo == team) ? selectedNode : newNode;
            OnInteract (newNode, true);
            Debug.Log (selectedNode);
            if (selectedNode != null)
                cachedValidMoves = BoardManager.ValidMoves (selectedNode, ref doneFirstMove);
            doneFirstMove = true;
            return newNode;

        }

        public static Node[] cachedValidMoves;

        //public static Node[] GetValidMoves => cachedValidMoves;

        static void ResetValidMoves () {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                if (node == null) continue;
                node.HighlightNode (new Color (), false);
            }
            cachedValidMoves = null;
        }

        static void ResetHigtlightOnValidMoves () {
            if (cachedValidMoves == null) return;
            foreach (var node in cachedValidMoves) {
                node.HighlightNode (new Color (), false);
            }
        }

        static Node AttemptToGetTarget (Node selectedNode, Node source) {
            if (cachedValidMoves == null) throw new NullReferenceException ("List of valid moves is empty");
            foreach (Node validNode in cachedValidMoves) {
                if (selectedNode == validNode && selectedNode.StoredPiece == null) {
                    return selectedNode;
                }
            }
            return null;

        }
        public static void OnActionTaken (Node go, Node.Team team, ref bool doneFirstMove, ref Node selectedNode, ref Node targetNode) {

            // if (resetInput) {
            //     currentNode.HighlightNode (new Color (), false);
            //     BoardManager.ResetValidNodes ();
            //     currentNode = null;
            // }
            // if (!selectionInput) return;

            // currentNode = currentNode ?? UserManager.GetNodeWithPiece (detectedNode, currentTeam);
            // selectedNode = (currentNode != null) ? UserManager.GetTargetNode (detectedNode, currentNode) : selectedNode;

            // if (currentNode == null || selectedNode == null) return;
            // Piece.MovePiece (currentNode.StoredPiece, currentNode, selectedNode);
            // currentNode.HighlightNode (new Color (), false);
            // BoardManager.ResetValidNodes ();
            // currentNode = selectedNode;
            // currentNode.HighlightNode (Color.green, true);
            // selectedNode = null;

            selectedNode = selectedNode ?? AttemptToGetPiece (go, team, ref doneFirstMove);
            targetNode = (selectedNode != null) ? AttemptToGetTarget (go, selectedNode) : targetNode;

            if (selectedNode != null && targetNode != null) {
                ResetValidMoves ();

                Piece.MovePiece (selectedNode.StoredPiece, selectedNode, targetNode);
                selectedNode.HighlightNode (new Color (), false);
                selectedNode = AttemptToGetPiece (targetNode, team, ref doneFirstMove);
                targetNode = null;

            }

        }

        public static void WhenTurnEnds (ref bool doneFirstMove, ref Node savedNode, ref Node savedTargetNode) {
            //Reset everything
            doneFirstMove = false;
            ResetValidMoves ();
            OnInteract (savedNode, false);
            savedNode.HighlightNode (new Color (), false);
            savedNode = null;
            savedTargetNode = null;

        }
    }
}