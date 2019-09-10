using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {
    public abstract class BoardManager {

        public static Node[, ] board;
        //static Node[] validMoves;

        public static Node[] ValidMoves (Node selectedNode, ref bool doneFirstMove) {
            // if (selectedNode == null) return null;
            // List<Node> validMoves = new List<Node> ();
            // Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            // for (int i = 0; i < 6; i++) {
            //     Vector2Int pos = selectedNode.CurrentBoardPosition + Node.DirectionInBoard (selectedNode.CurrentBoardPosition, i);
            //     Node foundNode = board[pos.x, pos.y];
            //     if (!IsValid (selectedNode, pos, foundNode)) continue;
            //     if (foundNode.StoredPiece != null) {
            //         Node newJump = ValidMove (foundNode, Node.DirectionInBoard (foundNode.CurrentBoardPosition, i));
            //         if (newJump == null) continue;
            //         newJump.HighlightNode (Color.yellow, true);
            //         validMoves.Add (newJump);

            //         if (hasJumped)
            //             continue;
            //     }

            //     Node newNode = foundNode;
            //     newNode.HighlightNode (Color.cyan, true);
            //     validMoves.Add (newNode);

            // }
            // return validMoves.ToArray ();

            if (selectedNode == null) {
                throw new NullReferenceException ("Selected node is missing.");
            }
            List<Node> validMoves = new List<Node> ();
            Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            for (int currentDirection = 0; currentDirection < 6; currentDirection++) {
                Vector2Int dir = Node.DirectionInBoard (selectedNode.CurrentBoardPosition, currentDirection);
                Vector2Int newPos = currentPos + dir;
                Node resultedNode = board[newPos.x, newPos.y];
                if (resultedNode == null || resultedNode.BelongsTo == Node.Team.Empty || OutOfBounds (newPos)) continue;
                Node finalNode = ConfirmResults (resultedNode, currentDirection, doneFirstMove);
                if (finalNode == null) continue;
                validMoves.Add (finalNode);
            }
            return validMoves.ToArray ();

        }

        private static Node ConfirmResults (Node resultedNode, int index, bool doneFirstMove) {
            if (resultedNode.StoredPiece != null) {
                Vector2Int currentPos = resultedNode.CurrentBoardPosition;
                Vector2Int newPos = currentPos + Node.DirectionInBoard (currentPos, index);
                Node newResultedNode = board[newPos.x, newPos.y];
                if (newResultedNode == null || newResultedNode.BelongsTo == Node.Team.Empty || OutOfBounds (newPos) || newResultedNode.StoredPiece != null) return null;
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

        public static void InsertPiecesToBoard (Node.Team[] players, Piece prefab) {
            for (int i = 0; i < players.Length; i++) {
                for (int y = 0; y < board.GetLength (0); y++) {
                    for (int x = 0; x < board.GetLength (1); x++) {
                        Node node = board[y, x];
                        if (node.BelongsTo == players[i])
                            node.StoredPiece = Piece.CreatePiece (prefab, Color.black, node, players[i]);

                    }
                }
            }

        }

    }
}