
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public enum NodeColor
{
    None, Unoccupied, Red, Orange, Yellow, Green, Blue, Magenta,
    RedFade, OrangeFade, YellowFade, GreenFade, BlueFade, MagentaFade,
    RedOrange, OrangeYellow, YellowGreen, GreenBlue, BlueMagenta, MagentaRed
}

public enum PieceColor
{
    Red = 2, Orange, Yellow, Green, Blue, Magenta
}


public delegate void WhileSearchingInList(Node foundNode);
[System.Serializable]
public class BoardModel
{
    [System.Serializable]
    public class Board
    {

        public Node[,] boardArray;
        public Piece[,] pieceArray;

        public NodeObject[,] boardViewArray;
        public PieceObject[,] pieceViewArray;

        public float value;

        public Board Clone()
        {
            Board clone = new Board();
            clone.boardArray = new Node[boardArray.GetLength(0), boardArray.GetLength(1)];
            for (int x = 0; x < boardArray.GetLength(0); x++)
            {
                for (int y = 0; y < boardArray.GetLength(1); y++)
                {
                    Node newNode = new Node(boardArray[x, y].currentPosition, boardArray[x, y].worldPosition, boardArray[x, y].belongsTo);
                    clone.boardArray[x, y] = newNode;
                }
            }
            clone.pieceArray = new Piece[pieceArray.GetLength(0), pieceArray.GetLength(1)];
            for (int x = 0; x < pieceArray.GetLength(0); x++)
            {
                for (int y = 0; y < pieceArray.GetLength(1); y++)
                {
                    Piece newPiece = new Piece(pieceArray[x, y].currentPosition, pieceArray[x, y].belongsTo);
                    clone.pieceArray[x, y] = newPiece;
                }
            }
            return clone;
        }

        public Node GetNode(Vector2Int pos) => boardArray[pos.x, pos.y];
        public Piece GetPiece(Vector2Int pos) => pieceArray[pos.x, pos.y];
        public PieceObject GetVisualPiece(Vector2Int pos) => pieceViewArray[pos.x, pos.y];
        public void RemovePieceAt(Vector2Int pos)
        {
            pieceArray[pos.x, pos.y] = null;
        }
        public void InsertPieceAt(Vector2Int pos, Piece newPiece)
        {
            pieceArray[pos.x, pos.y] = newPiece;
        }

        public void RemovePieceViewAt(Vector2Int pos)
        {
            pieceViewArray[pos.x, pos.y] = null;
        }

        public void InsertPieceViewAt(Vector2Int pos, PieceObject obj)
        {
            pieceViewArray[pos.x, pos.y] = obj;
        }

    }





    public static Board originalBoard;

}

[System.Serializable]
public class Node
{
    public Node(Vector2Int pos, Vector2 worldPos, Team currentTeam)
    {
        currentPosition = pos;
        belongsTo = currentTeam;
        worldPosition = worldPos;
    }
    [SerializeField] public Vector2Int currentPosition = Vector2Int.zero;
    [SerializeField] public Vector2 worldPosition = Vector2.zero;
    [SerializeField] public Team belongsTo;
}


[System.Serializable]
public class Piece
{
    public Piece(Vector2Int pos, Team currentTeam)
    {
        currentPosition = pos;
        belongsTo = currentTeam;
    }
    [SerializeField] public Vector2Int currentPosition;
    [SerializeField] public Team belongsTo;
}

