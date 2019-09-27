using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum NodeColor
{
    None,
    Unoccupied,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Magenta,
    RedFade,
    OrangeFade,
    YellowFade,
    GreenFade,
    BlueFade,
    MagentaFade,
    RedOrange,
    OrangeYellow,
    YellowGreen,
    GreenBlue,
    BlueMagenta,
    MagentaRed
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

        public Board SimulateABoard
        {
            get
            {
                Board newBoard = new Board();
                newBoard.boardArray = new Node[boardArray.GetLength(0), boardArray.GetLength(1)];
                newBoard.pieceArray = new Piece[pieceArray.GetLength(0), pieceArray.GetLength(1)];
                for (int x = 0; x < boardArray.GetLength(0); x++)
                {
                    for (int y = 0; y < boardArray.GetLength(1); y++)
                    {
                        newBoard.boardArray[x, y] = new Node(boardArray[x, y].pos, boardArray[x, y].worldPos, boardArray[x, y].belongsTo);
                        newBoard.pieceArray[x, y] = (pieceArray[x, y] != null) ? new Piece(pieceArray[x, y].pos, pieceArray[x, y].belongsTo) : null;
                    }
                }

                return newBoard;
            }
        }

        public NodeObject[,] boardViewArray;
        public PieceObject[,] pieceViewArray;

        public Node GetNode(Vector2Int pos) => boardArray[pos.x, pos.y];
        public Piece GetPiece(Vector2Int pos) => pieceArray[pos.x, pos.y];
        public PieceObject GetVisualPiece(Vector2Int pos) => pieceViewArray[pos.x, pos.y];

        public NodeObject GetVisualNode(Vector2Int pos) => boardViewArray[pos.x, pos.y];

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

    public class Turn : IComparable
    {

        public Vector2Int movedPiece;
        public Vector2Int target;
        float value;
        public float Value => value;

        public Turn() { }

        public Turn(Vector2Int _piece, Vector2Int _node, float _value)
        {
            movedPiece = _piece;
            target = _node;
            value = _value;

        }

        public int CompareTo(object obj)
        {
            return (value > (obj as Turn).value) ? 1 : (value < (obj as Turn).value) ? -1 : 0;
        }

        public List<Turn> Expand(UserModel model, Board currentBoard)
        {
            List<Turn> output = new List<Turn>();
            foreach (Piece piece in model.playerPieces)
            {
                foreach (Node node in model.GetPath(currentBoard.GetNode(piece.pos), new List<Node>(), true, false))
                {
                    Board simulatedBoard = currentBoard.SimulateABoard;
                    Piece simPiece = simulatedBoard.GetPiece(piece.pos);
                    Node simNode = simulatedBoard
                        .GetNode(node.pos);
                    Vector2Int oldPos = simPiece.pos;
                    model.SimulateMove(ref simPiece, ref simNode, ref simulatedBoard);
                    float eval = EvaluateState(model, simulatedBoard);

                    Turn turn = new Turn(oldPos, simNode.pos, eval);
                    output.Add(turn);
                }
            }
            return output;
        }

        float EvaluateState(UserModel model, Board customBoard)
        {
            float dist = 0;
            foreach (Vector2Int pos in UserModel.GetPlayerPositions(model.currentTeam, customBoard))
            {
                dist -= Vector2Int.Distance(pos, model.opponentGoal);

            }
            return dist;
        }
    }

    public static Board originalBoard;

}

[System.Serializable]
public class Node
{
    public Node(Vector2Int pos, UnityEngine.Vector2 worldPos, Team currentTeam)
    {
        this.pos = pos;
        belongsTo = currentTeam;
        this.worldPos = worldPos;
    }

    [SerializeField] public Vector2Int pos = Vector2Int.zero;
    [SerializeField] public UnityEngine.Vector2 worldPos = UnityEngine.Vector2.zero;
    [SerializeField] public Team belongsTo;
}

[System.Serializable]
public class Piece
{
    public Piece(Vector2Int pos, Team currentTeam)
    {
        this.pos = pos;
        belongsTo = currentTeam;
    }

    public Vector2Int this[int a, int b] => pos;

    [SerializeField] public Vector2Int pos;
    [SerializeField] public Team belongsTo;
}