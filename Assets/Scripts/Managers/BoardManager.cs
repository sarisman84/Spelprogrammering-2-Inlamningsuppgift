using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This is a temporary script as this will be massively reworked at a later date.
namespace ChineseCheckers
{
    public abstract class BoardManager
    {

        public static Node[,] board;

        /// <summary>
        /// When called, it checks for any available node that has no pieces stored in it in a cardinal direction on the board.
        /// </summary>
        /// <param name="selectedNode">The source in which it will check those avaliable nodes from</param>
        /// <param name="doneFirstMove">If the user who called this method has already done his first move</param>
        /// <returns>An array of avaliable nodes</returns>
        public static Node[] ValidMoves(Node selectedNode, ref bool doneFirstMove, bool highlight)
        {
            if (selectedNode == null)
            {
                throw new NullReferenceException("Selected node is missing.");
            }
            List<Node> validMoves = new List<Node>();
            Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            for (int currentDirection = 0; currentDirection < 6; currentDirection++)
            {
                Vector2Int dir = Node.DirectionInBoard(selectedNode.CurrentBoardPosition, currentDirection);
                Vector2Int newPos = currentPos + dir;
                if (OutOfBounds(newPos)) continue;
                Node resultedNode = board[newPos.x, newPos.y];
                if (resultedNode == null || resultedNode.BelongsTo == Team.Empty) continue;

                Node finalNode = ConfirmResults(resultedNode, currentDirection, doneFirstMove, highlight);

                if (finalNode == null) continue;
                validMoves.Add(finalNode);
            }
            return validMoves.ToArray();

        }

        /// <summary>
        /// Helper method that confirms if the resulted node has a stored piece or not.
        /// </summary>
        /// <param name="resultedNode">The node that is being tested</param>
        /// <param name="index">The current index when this particular method was called in</param>
        /// <param name="doneFirstMove">If the player has already done his first move.</param>
        /// <returns>Either a result that has no StoredPiece in the node or a new node that was found by checking in the same cardinal direction, using the same logic.</returns>
        private static Node ConfirmResults(Node resultedNode, int currentDirection, bool doneFirstMove, bool highlight)
        {
            if (resultedNode.StoredPiece != null)
            {
                Vector2Int currentPos = resultedNode.CurrentBoardPosition;
                Vector2Int newPos = currentPos + Node.DirectionInBoard(currentPos, currentDirection);
                if (OutOfBounds(newPos))
                {
                    return null;
                }
                Node newResultedNode = board[newPos.x, newPos.y];
                if (newResultedNode == null || newResultedNode.BelongsTo == Team.Empty || newResultedNode.StoredPiece != null) return null;
                newResultedNode.HighlightNode(Color.yellow, highlight);
            }
            if (!doneFirstMove)
            {

                resultedNode.HighlightNode(Color.cyan, highlight);

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


        public static Node[] Path(Node source, bool highlight)
        {

            List<Node> validNodes = new List<Node>();
            int lastDir = -1;
            int i = 0, length = 1;

            return GetPath(i, length, source, highlight, validNodes, lastDir).ToArray();
        }


        private static List<Node> GetPath(int dir, int length, Node source, bool highlight, List<Node> validNodes, int lastDir)
        {

            if (dir >= 6)
                return validNodes;
            // foreach (var item in potentialPath)
            // {
            //     List<Node> newPath = GetPath(item, highlight, ref hasFoundFirstPath, ref potentialPath, validNodes);
            //     foreach (var node in newPath)
            //     {
            //         validNodes.Add(node);
            //     }
            // }

            // for (int i = 0; i < 6; i++)
            // {

            //     Node foundNode = GetValidNode(source, i, hasFoundFirstPath, highlight);
            //     if (foundNode == null) continue;
            //     if (foundNode.StoredPiece != null)
            //     {
            //         potentialPath.Add(GetValidNode(foundNode, i, hasFoundFirstPath, highlight));
            //         continue;
            //     }
            //     validNodes.Add(foundNode);

            // }


            // hasFoundFirstPath = true;


            Node node = GetValidNode(source, dir, highlight);
            if (node == null) return GetPath(dir + 1, length, source, highlight, validNodes, lastDir);
            Node newNode = GetValidNode(node, dir, highlight);
            if (newNode != null && newNode.StoredPiece != null)
            {
                node.HighlightNode(Color.cyan, highlight);
                validNodes.Add(node);
            }

            return GetPath(dir + 1, length, source, highlight, validNodes, lastDir);


        }


        static Node GetValidNode(Node source, int index, bool highlight)
        {
            if (source == null) return null;
            Vector2Int pos = source.CurrentBoardPosition, boardDir = Node.DirectionInBoard(pos, index), newPos = pos + boardDir;
            if (OutOfBounds(newPos)) return null;
            Node potetialNode = GetNodeInBoardPos(newPos);
            if (potetialNode.BelongsTo == Team.Empty) return null;

            //potetialNode.HighlightNode(Color.cyan, highlight);
            return potetialNode;



        }




        /// <summary>
        /// Helper method that checks if the inputed value is out of bounds in the boards array
        /// </summary>
        /// <param name="newPos">The inputed value in question</param>
        /// <returns>True if the value is higher than the Array Length or lower than zero</returns>
        private static bool OutOfBounds(Vector2Int newPos)
        {
            return (newPos.x > board.GetLength(0) - 1 || newPos.y > board.GetLength(1) - 1 || newPos.x <= 0 || newPos.y <= 0);
        }

        private static Node GetNodeInBoardPos(Vector2Int pos)
        {
            return board[pos.x, pos.y];
        }

    }
}