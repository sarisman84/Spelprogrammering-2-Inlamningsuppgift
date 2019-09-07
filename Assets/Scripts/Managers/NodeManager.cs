using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This script handles how a user can find a piece, a destination and the proceed to move them using the PieceManager
//This is a temporary script as this will be massively reworked at a later date.
public sealed class NodeManager
{

    public static Node[,] board;
    static Node[] validMoves = new Node[6];
    public static Node[] ValidMoves(Node selectedNode)
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
            if (board[pos.x, pos.y].BelongsTo == Node.Team.Empty || selectedNode.CurrentBoardPosition == pos) continue;
            validMoves[i] = board[pos.x, pos.y];
            validMoves[i].HighlightNode(Color.cyan, true);

        }
        return validMoves;

    }
}