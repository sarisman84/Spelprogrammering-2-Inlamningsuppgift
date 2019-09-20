using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {

    /// <summary>
    /// A custom class that holds information about the current board.
    /// This can also be used to simulate a board.
    /// </summary>
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

   
        /// <summary>
        /// Searches for any available nodes in the default board.
        /// </summary>
        /// <param name="source">Where it will search from.</param>
        /// <param name="highlight">If the founded results will be highlighted or not.</param>
        /// <returns>A list of valid moves.</returns>
        public static List<Node> Path (Node source, bool highlight) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true);
        }
        /// <summary>
        /// Searches for any avaliable nodes in a simulated board.
        /// </summary>
        /// <param name="source"> Where it will search from.</param>
        /// <param name="highlight">If the founded results will be highlighted or not.</param>
        /// <param name="tempBoard">A simulated board to do the search on.</param>
        /// <returns>A list of valid moves.</returns>
        public static List<Node> Path (Node source, bool highlight, Node[, ] tempBoard) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true, tempBoard);
        }

        private static List<Node> GetPath (Node source, List<Node> validMoves, bool highlight, bool searchAllAvailablePaths) {
            //Search through each of the six cardinal directions.
            for (int curDir = 0; curDir < 6; curDir++) {
                //Attempt to get a valid node.
                Node node = GetValidNode (source, validMoves, curDir);
                if (node == null) continue;
                //If said node already has a piece, search beyond it for any possible valid nodes.
                if (node.StoredPiece != null) {
                    Node potentialPath = GetValidNode (node, validMoves, curDir);
                    //If the potentialPath is not null and it doesnt have a piece in it, do a recursive call to repeat the search.
                    if (potentialPath != null && potentialPath.StoredPiece == null) {
                        potentialPath.Object.HighlightNode (Color.yellow, highlight);
                        //Add the found node to the list of validMoves.
                        validMoves.Add (potentialPath);
                        //Once you do the recursive call, add the found result back to the original list.
                        List<Node> newPath = GetPath (potentialPath, validMoves, highlight, false);
                        validMoves.AddRange(newPath);
                    }
                    continue;

                }
                //If the searchAllAvaliablePaths bool is true, allow it to search for any nodes with no piece in them in the said directons
                if (searchAllAvailablePaths) {
                    node.Object.HighlightNode (Color.cyan, highlight);
                    validMoves.Add (node);
                }

            }
            //Return the list of valid moves.
            return validMoves;

        }
       
        //The method below practically does the same, just that it uses a simulated board to do its search.
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
        /// <summary>
        /// Searches for a valid node.
        /// </summary>
        /// <param name="source">Where it will search from</param>
        /// <param name="validMoves">The list of the previous valid nodes.</param>
        /// <param name="index">Which direction are we currently looking at.</param>
        /// <returns>A valid node.</returns>
        public static Node GetValidNode (Node source, List<Node> validMoves, int index) {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard (pos, index), newPos = pos + boardDir;
            if (OutOfBounds (newPos)) return null;
            Node potetialNode = GetNodeInBoardPos (newPos);
            if (potetialNode.BelongsTo == Team.Empty || validMoves.Any (p => p == potetialNode)) return null;

            //potetialNode.HighlightNode(Color.cyan, highlight);
            return potetialNode;

        }
        /// <summary>
        /// Searches for a valid node in a simulated board.
        /// </summary>
        /// <param name="source">Where it will search from</param>
        /// <param name="validMoves">The list of the previous valid nodes</param>
        /// <param name="index">What direction are we currently looking at.</param>
        /// <param name="tempBoard">A simulated board reference</param>
        /// <returns>A valid node.</returns>
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


        /// <summary>
        /// Helper method that checks if he inputed value in the simulated board is out of bounds.
        /// </summary>
        /// <param name="tempBoard">A refernece to that simulated board</param>
        /// <param name="newPos">The inputed value in question</param>
        /// <returns>True if the value is higher than the Array Length or lower than zero</returns>
        private static bool OutOfBounds (Node[, ] tempBoard, Vector2Int newPos) {
            if (tempBoard == null) return false;
            return (newPos.x > tempBoard.GetLength (0) - 1 || newPos.y > tempBoard.GetLength (1) - 1 || newPos.x <= 0 || newPos.y <= 0);
        }

        /// <summary>
        /// A shortcut to get a node in the default board position.
        /// </summary>
        /// <param name="pos">Inputed position</param>
        /// <returns>A node</returns>
        private static Node GetNodeInBoardPos (Vector2Int pos) {
            return board[pos.x, pos.y];
        }
        
        /// <summary>
        /// A shortcut to get a node in the simulated board position.
        /// </summary>
        /// <param name="tempBoard">A reference to the simulated board.</param>
        /// <param name="pos">Inputed positon</param>
        /// <returns>A node</returns>
        private static Node GetNodeInBoardPos (Node[, ] tempBoard, Vector2Int pos) {
            if (tempBoard == null) return null;
            return tempBoard[pos.x, pos.y];
        }

    }
}