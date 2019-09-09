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
        //static Node[] validMoves;

        public static void ResetValidNodes()
        {
            if (validMoves[0] != null)
            {
                for (int i = 0; i < validMoves.Length; i++)
                {
                    Node node = validMoves[i];
                    if (node == null) continue;
                    node.HighlightNode(new Color(), false);
                    node = null;
                }
            }
        }

        static Node[] validMoves;
        public static Node[] ValidMoves(Node selectedNode, bool onlyExtraMoves)
        {
            if (selectedNode == null) return validMoves;
            //ResetValidNodes();
            validMoves = validMoves ?? new Node[6];
            if(validMoves != null){
                for (int i = 0; i < validMoves.Length; i++)
                {
                    Node node = validMoves[i];
                    if (node == null) continue;
                    node.HighlightNode(new Color(), false);
                    node = null;
                }
            }
            Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            for (int i = 0; i < 6; i++)
            {
                Vector2Int pos = selectedNode.CurrentBoardPosition + Node.DirectionInBoard(selectedNode.CurrentBoardPosition, i);
                Node foundNode = board[pos.x, pos.y];
                if (!IsValid(selectedNode, pos, foundNode)) continue;
                if (foundNode.StoredPiece != null)
                {
                    Vector2Int newPos = foundNode.CurrentBoardPosition + Node.DirectionInBoard(foundNode.CurrentBoardPosition, i);
                    Node newFoundNode = board[newPos.x, newPos.y];
                    if (!IsValid(foundNode, newPos, newFoundNode) || newFoundNode.StoredPiece != null) continue;
                    validMoves[i] = newFoundNode;
                    validMoves[i].HighlightNode(Color.cyan, true);
                    continue;
                }
                if (!onlyExtraMoves)
                {
                    validMoves[i] = foundNode;
                    validMoves[i].HighlightNode(Color.cyan, true);
                }
            }
            return validMoves;

        }





        private static bool IsValid(Node selectedNode, Vector2Int pos, Node foundNode)
        {
            return foundNode.BelongsTo != Node.Team.Empty && selectedNode.CurrentBoardPosition != pos && pos.x <= board.GetLength(0) - 1 && pos.y <= board.GetLength(1) - 1;
        }

        public static void InsertPiecesToBoard(Node.Team[] players, Piece prefab)
        {
            for (int i = 0; i < players.Length; i++)
            {
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        Node node = board[y, x];
                        if (node.BelongsTo == players[i])
                            node.StoredPiece = Piece.CreatePiece(prefab, Color.black, node, players[i]);

                    }
                }
            }

        }

    }
}