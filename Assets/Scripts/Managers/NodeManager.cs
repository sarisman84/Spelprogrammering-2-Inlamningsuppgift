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
        static Node[] validMoves;


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
        public static Node[] ValidMoves(Node selectedNode)
        {
            if (selectedNode == null) return validMoves;
            validMoves = new Node[6];
            Vector2Int currentPos = selectedNode.CurrentBoardPosition;
            for (int i = 0; i < 6; i++)
            {
                Vector2Int pos = (selectedNode.CurrentBoardPosition + Node.DirectionToBoardCoordinate(selectedNode, i));
                if (board[pos.x, pos.y].BelongsTo == Node.Team.Empty 
                || selectedNode.CurrentBoardPosition == pos 
                || board[pos.x, pos.y].StoredPiece != null) continue;
                validMoves[i] = board[pos.x, pos.y];
                validMoves[i].HighlightNode(Color.cyan, true);

            }
            return validMoves;

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