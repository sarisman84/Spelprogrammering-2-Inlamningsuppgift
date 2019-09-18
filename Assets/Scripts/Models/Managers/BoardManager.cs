using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {

    public class Board {
        public Node[, ] board;
        public float value;
        public Node this [int a, int b] {

            get { return board[a, b]; }
            set { board[a, b] = value; }
        }

        public int GetLength (int i) {
            return board.GetLength (i);
        }
        public int Length => board.Length;

        public Board Copy () {
            Board copy = new Board ();
            copy.board = new Node[board.GetLength (0), board.GetLength (1)];
            for (int y = 0; y < board.GetLength (0); y++) {
                for (int x = 0; x < board.GetLength (1); x++) {
                    copy.board[y, x] = board[y, x];
                }
            }
            return copy;
        }

    }
    public abstract class BoardManager {

        public static Board board;

        public static List<Node> TestPath (Node source) {
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

            return validMoves;
        }

        public static List<Node> Path (Node source, bool highlight) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true);
        }

        public static List<Node> Path (Node source, bool highlight, Node[, ] tempBoard) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true, tempBoard);
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

        private static List<Node> GetPath (Node source, List<Node> validMoves, bool highlight, bool searchAllAvailablePaths, Node[, ] tempBoard) {
            for (int curDir = 0; curDir < 6; curDir++) {
                Node node = GetValidNode (source, validMoves, curDir);
                if (node == null) continue;
                if (node.StoredPiece != null) {
                    Node potentialPath = GetValidNode (node, validMoves, curDir, tempBoard);
                    if (potentialPath != null && potentialPath.StoredPiece == null) {
                        potentialPath.Object.HighlightNode (Color.yellow, highlight);
                        validMoves.Add (potentialPath);
                        List<Node> newPath = GetPath (potentialPath, validMoves, highlight, false, tempBoard);
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

        public static Node GetValidNode (Node source, List<Node> validMoves, int index) {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard (pos, index), newPos = pos + boardDir;
            if (OutOfBounds (newPos)) return null;
            Node potetialNode = GetNodeInBoardPos (newPos);
            if (potetialNode.BelongsTo == Team.Empty || validMoves.Any (p => p == potetialNode)) return null;

            //potetialNode.HighlightNode(Color.cyan, highlight);
            return potetialNode;

        }

        public static Node GetValidNode (Node source, List<Node> validMoves, int index, Node[, ] tempBoard) {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard (pos, index), newPos = pos + boardDir;
            if (OutOfBounds (tempBoard, newPos)) return null;
            Node potetialNode = GetNodeInBoardPos (tempBoard, newPos);
            if (validMoves == null) return null;
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

        private static bool OutOfBounds (Node[, ] tempBoard, Vector2Int newPos) {
            if (tempBoard == null) return false;
            return (newPos.x > tempBoard.GetLength (0) - 1 || newPos.y > tempBoard.GetLength (1) - 1 || newPos.x <= 0 || newPos.y <= 0);
        }

        private static Node GetNodeInBoardPos (Vector2Int pos) {
            return board[pos.x, pos.y];
        }

        private static Node GetNodeInBoardPos (Node[, ] tempBoard, Vector2Int pos) {
            if (tempBoard == null) return null;
            return tempBoard[pos.x, pos.y];
        }

    }
}