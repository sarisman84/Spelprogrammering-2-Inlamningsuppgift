using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {
    public abstract class BoardManager {

        public static Node[, ] board;

        public static bool TestPath (Node source) {
            List<Node> validMoves = new List<Node> ();
            for (int i = 0; i < 6; i++) {
                Node node = GetValidNode (source, validMoves, i);
                if (node == null) continue;
                if (node.StoredPiece != null) {
                    Node potentialPath = GetValidNode (node, validMoves, i);
                    if (potentialPath == null) continue;
                    if (potentialPath.StoredPiece == null) {
                        validMoves.Add (potentialPath);
                    }
                    continue;
                }
                validMoves.Add (node);

            }

            if (validMoves != null) return true;
            return false;
        }

        public static Node[] Path (Node source, bool highlight) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true).ToArray ();
        }

        private static List<Node> GetPath (Node source, List<Node> validMoves, bool highlight, bool searchAllAvailablePaths) {
            for (int curDir = 0; curDir < 6; curDir++) {
                Node node = GetValidNode (source, validMoves, curDir);
                if (node == null) continue;
                if (node.StoredPiece != null) {
                    Node potentialPath = GetValidNode (node, validMoves, curDir);
                    if (potentialPath != null && potentialPath.StoredPiece == null) {
                        potentialPath.Object.HighlightNode (Color.yellow, highlight);
                        validMoves.Add (potentialPath);
                        List<Node> newPath = GetPath (potentialPath, validMoves, highlight, false);
                        foreach (var path in newPath) {
                            if (validMoves.Any (p => p == path)) continue;
                            validMoves.Add (path);
                        }
                    }
                    continue;

                }
                if (searchAllAvailablePaths) {
                    node.Object.HighlightNode (Color.cyan, highlight);
                    validMoves.Add (node);
                }

            }
            return validMoves;

        }

        static Node GetValidNode (Node source, List<Node> validMoves, int index) {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard (pos, index), newPos = pos + boardDir;
            if (OutOfBounds (newPos)) return null;
            Node potetialNode = GetNodeInBoardPos (newPos);
            if (potetialNode.BelongsTo == Team.Empty || validMoves.Any (p => p == potetialNode)) return null;

            //potetialNode.HighlightNode(Color.cyan, highlight);
            return potetialNode;

        }

        /// <summary>
        /// Helper method that checks if the inputed value is out of bounds in the boards array
        /// </summary>
        /// <param name="newPos">The inputed value in question</param>
        /// <returns>True if the value is higher than the Array Length or lower than zero</returns>
        private static bool OutOfBounds (Vector2Int newPos) {
            return (newPos.x > board.GetLength (0) - 1 || newPos.y > board.GetLength (1) - 1 || newPos.x <= 0 || newPos.y <= 0);
        }

        private static Node GetNodeInBoardPos (Vector2Int pos) {
            return board[pos.x, pos.y];
        }

    }
}