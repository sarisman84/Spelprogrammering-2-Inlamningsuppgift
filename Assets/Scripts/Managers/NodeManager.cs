using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {
    public abstract class BoardManager {

        public static Node[, ] board;

        public static Node[] ValidMoves (Node selectedNode, ref bool doneFirstMove) {
            if (selectedNode == null) {
                throw new NullReferenceException ("Selected node is missing.");
            }
            List<Node> validMoves = new List<Node> ();
            Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            for (int currentDirection = 0; currentDirection < 6; currentDirection++) {
                Vector2Int dir = Node.DirectionInBoard (selectedNode.CurrentBoardPosition, currentDirection);
                Vector2Int newPos = currentPos + dir;
                if (OutOfBounds (newPos)) continue;
                Node resultedNode = board[newPos.x, newPos.y];
                if (resultedNode == null || resultedNode.BelongsTo == Team.Empty) continue;
                Node finalNode = ConfirmResults (resultedNode, currentDirection, doneFirstMove);
                if (finalNode == null) continue;
                validMoves.Add (finalNode);
            }
            return validMoves.ToArray ();

        }

        public static Node[] ValidMoves (Node[] playerBase, ref bool doneFirstMove) {
            if (playerBase == null) {
                throw new NullReferenceException ("Player Base doesnt exist. (Array is empty)");
            }
            List<Node> validMoves = new List<Node> ();
            foreach (Node selectedNode in playerBase) {
                Vector2Int currentPos = selectedNode.CurrentBoardPosition;
                for (int currentDirection = 0; currentDirection < 6; currentDirection++) {
                    Vector2Int dir = Node.DirectionInBoard (selectedNode.CurrentBoardPosition, currentDirection);
                    Vector2Int newPos = currentPos + dir;
                    if (OutOfBounds (newPos)) continue;
                    Node resultedNode = board[newPos.x, newPos.y];
                    if (resultedNode == null || resultedNode.BelongsTo == Team.Empty) continue;
                    Node finalNode = ConfirmResults (resultedNode, currentDirection, doneFirstMove);
                    if (finalNode == null) continue;
                    validMoves.Add (finalNode);
                }
            }

            return validMoves.ToArray ();
        }

        private static Node ConfirmResults (Node resultedNode, int index, bool doneFirstMove) {
            if (resultedNode.StoredPiece != null) {
                Vector2Int currentPos = resultedNode.CurrentBoardPosition;
                Vector2Int newPos = currentPos + Node.DirectionInBoard (currentPos, index);
                if (OutOfBounds (newPos)) return null;
                Node newResultedNode = board[newPos.x, newPos.y];
                if (newResultedNode == null || newResultedNode.BelongsTo == Team.Empty || newResultedNode.StoredPiece != null) return null;
                newResultedNode.HighlightNode (Color.yellow, true);
                return newResultedNode;
            }
            if (!doneFirstMove) {
                resultedNode.HighlightNode (Color.cyan, true);
                return resultedNode;
            }
            return null;

        }

        private static bool OutOfBounds (Vector2Int newPos) {
            return (newPos.x > board.GetLength (0) - 1 || newPos.y > board.GetLength (1) - 1 || newPos.x <= 0 || newPos.y <= 0);
        }

    }
}