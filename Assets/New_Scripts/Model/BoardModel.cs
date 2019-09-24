
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
    Red = 2, Orange, Yellow, Green, Blue, Magenta, Unoccupied = 1
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

        public Board CreateSimulation(){
            Board newBoard = new Board();
            newBoard.pieceArray = this.pieceArray;
            newBoard.boardArray = this.boardArray;
            return newBoard;
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

