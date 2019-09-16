using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers {
    public abstract class BoardManager {

        public static Node[, ] board;

        /// <summary>
        /// When called, it checks for any available node that has no pieces stored in it in a cardinal direction on the board.
        /// </summary>
        /// <param name="selectedNode">The source in which it will check those avaliable nodes from</param>
        /// <param name="doneFirstMove">If the user who called this method has already done his first move</param>
        /// <returns>An array of avaliable nodes</returns>
        public static Node[] ValidMoves (Node selectedNode, ref bool doneFirstMove, bool highlight) {
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

                Node finalNode = ConfirmResults (resultedNode, currentDirection, doneFirstMove, highlight);

                if (finalNode == null) continue;
                validMoves.Add (finalNode);
            }
            return validMoves.ToArray ();

        }

        /// <summary>
        /// Helper method that confirms if the resulted node has a stored piece or not.
        /// </summary>
        /// <param name="resultedNode">The node that is being tested</param>
        /// <param name="index">The current index when this particular method was called in</param>
        /// <param name="doneFirstMove">If the player has already done his first move.</param>
        /// <returns>Either a result that has no StoredPiece in the node or a new node that was found by checking in the same cardinal direction, using the same logic.</returns>
        private static Node ConfirmResults (Node resultedNode, int currentDirection, bool doneFirstMove, bool highlight) {
            if (resultedNode.StoredPiece != null) {
                Vector2Int currentPos = resultedNode.CurrentBoardPosition;
                Vector2Int newPos = currentPos + Node.DirectionInBoard (currentPos, currentDirection);
                if (OutOfBounds (newPos)) {
                    return null;
                }
                Node newResultedNode = board[newPos.x, newPos.y];
                if (newResultedNode == null || newResultedNode.BelongsTo == Team.Empty || newResultedNode.StoredPiece != null) return null;
                newResultedNode.HighlightNode (Color.yellow, highlight);
                return newResultedNode;
            }
            if (!doneFirstMove) {

                resultedNode.HighlightNode (Color.cyan, highlight);

                return resultedNode;
            }

            return null;

        }

        // public static Node[] MoveTree(Node source, bool highlight, ref bool searchedMainMoves)
        // {
        //     if (source == null) throw new NullReferenceException("Cant find available paths without a source! (Source variable is null)");
        //     List<Node> validMoves = new List<Node>();
        //     Vector2Int currentPos = source.CurrentBoardPosition;
        //     Node potentialPath = null;
        //     for (int currentDirection = 0; currentDirection < 6; currentDirection++)
        //     {
        //         Vector2Int dir = Node.DirectionInBoard(source.CurrentBoardPosition, currentDirection);
        //         Vector2Int newPos = currentPos + dir;
        //         if (OutOfBounds(newPos)) continue;
        //         potentialPath = board[newPos.x, newPos.y];
        //         if (potentialPath.BelongsTo == Team.Empty) continue;
        //         if (potentialPath.StoredPiece == null && !searchedMainMoves)
        //         {
        //             potentialPath.HighlightNode(Color.cyan, highlight);
        //             validMoves.Add(potentialPath);
        //         }
        //         if (potentialPath.StoredPiece != null)
        //         {
        //             Vector2Int pos = potentialPath.CurrentBoardPosition;
        //             Vector2Int _newPos = pos + Node.DirectionInBoard(pos, currentDirection);
        //             potentialPath = board[_newPos.x, _newPos.y];
        //             if (potentialPath.BelongsTo == Team.Empty) continue;
        //             if (potentialPath.StoredPiece == null)
        //             {
        //                 validMoves.Add(potentialPath);
        //                 potentialPath.HighlightNode(Color.yellow, highlight);
        //                 MoveTree(potentialPath, highlight, currentDirection);
        //             }
        //         }
        //     }

        //     return validMoves.ToArray();
        // }

        public static Node[] Path (Node source, bool highlight) {
            List<Node> validMoves = new List<Node> ();
            return GetPath (source, validMoves, highlight, true).ToArray ();
        }

        private static List<Node> GetPath (Node source, List<Node> validMoves, bool highlight, bool searchAllAvailablePaths) {
            for (int curDir = 0; curDir < 6; curDir++) {
                Node node = GetValidNode (source, validMoves, curDir, highlight);
                if (node == null) continue;
                if (node.StoredPiece != null) {
                    Node potentialPath = GetValidNode (node, validMoves, curDir, highlight);
                    if (potentialPath != null && potentialPath.StoredPiece == null) {
                        potentialPath.HighlightNode (Color.yellow, highlight);
                        validMoves.Add (potentialPath);
                        List<Node> newPath = GetPath (potentialPath, validMoves, highlight, false);
                        foreach (var path in newPath) {
                            if (validMoves.Find (p => p == path)) continue;
                            validMoves.Add (path);
                        }
                    }
                    continue;

                }
                if (searchAllAvailablePaths) {
                    node.HighlightNode (Color.cyan, highlight);
                    validMoves.Add (node);
                }

            }
            return validMoves;

        }

        static Node GetValidNode (Node source, List<Node> validMoves, int index, bool highlight) {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard (pos, index), newPos = pos + boardDir;
            if (OutOfBounds (newPos)) return null;
            Node potetialNode = GetNodeInBoardPos (newPos);
            if (potetialNode.BelongsTo == Team.Empty || validMoves.Find (p => p == potetialNode)) return null;

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