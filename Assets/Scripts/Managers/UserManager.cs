using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChineseCheckers;

namespace ChineseCheckers
{
    public static class UserManager
    {


        public static Node GetTargetNode(Node nodeInQuestion, Node source)
        {

            if(source == null) return null;
            foreach (Node node in BoardManager.ValidMoves(source))
            {
                if (nodeInQuestion != node || node.StoredPiece != null || nodeInQuestion == source) continue;
                Debug.Log($" Found {nodeInQuestion} within {node}");
                return nodeInQuestion;
            }
            return null;
        }

        static Node cachedNode;
        public static Node GetNodeWithPiece(Node detectedNode, Node.Team currentTeam)
        {
            if (cachedNode != null) cachedNode.HighlightNode(Color.green, false);
            cachedNode = (detectedNode.StoredPiece != null && detectedNode.StoredPiece.BelongsTo == currentTeam) ? detectedNode : cachedNode;
            if (cachedNode != null) cachedNode.HighlightNode(Color.green, true);
            return cachedNode;
        }
    }
}

