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
        static Node[] validMoves = new Node[6];


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

            Vector2Int currentPos = selectedNode.CurrentBoardPosition;


            //x - y / 2;
            for (int i = 0; i < 6; i++)
            {
                // for (int j = 0; j < board.Length; j++) {
                //     if (selectedNode.CurrentBoardPosition + direction[i] == board[i,j].CurrentBoardPosition) {
                //         validNodes.Add (gameBoard.nodes[j]);
                //         gameBoard.nodes[j].GetComponent<Renderer> ().material.color = Color.black;
                //     }
                // }
                Vector2Int pos = (selectedNode.CurrentBoardPosition + Node.DirectionToBoardCoordinate(selectedNode, i));
                if (board[pos.x, pos.y].BelongsTo == Node.Team.Empty || selectedNode.CurrentBoardPosition == pos || board[pos.x, pos.y].StoredPiece != null) continue;
                validMoves[i] = board[pos.x, pos.y];
                validMoves[i].HighlightNode(Color.cyan, true);

            }
            return validMoves;

        }


        public static void InsertPiecesToBoard(int amountOfPlayers, Piece prefab)
        {
            for (int i = 0; i < amountOfPlayers; i++)
            {
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        Node node = board[y, x];
                        if (node.BelongsTo == (Node.Team)(i + 2))
                            node.StoredPiece = Piece.CreatePiece(prefab, Color.black, node, (Node.Team)(i + 2));

                    }
                }
            }

        }




    }
}