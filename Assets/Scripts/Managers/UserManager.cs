using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using UnityEngine;

namespace ChineseCheckers {
    public static class UserManager {

        static Node GetTargetNode (Node nodeInQuestion, Node source) {

            if (source == null) return null;
            foreach (Node node in BoardManager.ValidMoves (source, false)) {
                if (nodeInQuestion != node || node.StoredPiece != null || nodeInQuestion == source) continue;
                Debug.Log ($" Found {nodeInQuestion} within {node}");
                return nodeInQuestion;
            }
            return null;
        }

        static Node cachedNode;
        static Node GetNodeWithPiece (Node detectedNode, Node.Team currentTeam) {
            if (cachedNode != null) cachedNode.HighlightNode (Color.green, false);
            cachedNode = (detectedNode.StoredPiece != null && detectedNode.StoredPiece.BelongsTo == currentTeam) ? detectedNode : cachedNode;
            if (cachedNode != null) cachedNode.HighlightNode (Color.green, true);
            return cachedNode;
        }

        public static void OnActionTaken (Node detectedNode, ref Node currentNode, ref Node selectedNode, Node.Team currentTeam, bool selectionInput, bool resetInput) {
            if (resetInput) {
                currentNode.HighlightNode (new Color (), false);
                BoardManager.ResetValidNodes ();
                currentNode = null;
            }
            if (!selectionInput) return;

            currentNode = currentNode ?? UserManager.GetNodeWithPiece (detectedNode, currentTeam);
            selectedNode = (currentNode != null) ? UserManager.GetTargetNode (detectedNode, currentNode) : selectedNode;

            if (currentNode == null || selectedNode == null) return;
            Piece.MovePiece (currentNode.StoredPiece, currentNode, selectedNode);
            currentNode.HighlightNode (new Color (), false);
            BoardManager.ResetValidNodes ();
            currentNode = selectedNode;
            currentNode.HighlightNode (Color.green, true);
            selectedNode = null;
        }
    }
}