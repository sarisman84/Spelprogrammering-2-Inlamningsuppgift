using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
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

#region OldBoardModel
// [System.Serializable]
// public class BoardModel
// {
//     [System.Serializable]
//     public class Board
//     {

//         public Node[,] boardArray;
//         public Piece[,] pieceArray;

//         int boardSizeX, boardSizeY;

//         public Vector2Int GetLength
//         {
//             get
//             {
//                 if (boardSizeX == 0) boardSizeX = boardArray.GetLength(0);
//                 if (boardSizeY == 0) boardSizeY = boardArray.GetLength(1);

//                 return new Vector2Int(boardSizeX, boardSizeY);
//             }
//         }

//         public Piece[,] GetSimulateABoard()
//         {

//             Piece[,] newPieceArray = new Piece[GetLength.x, GetLength.y];
//             for (int x = 0; x < boardArray.GetLength(0); x++)
//             {
//                 for (int y = 0; y < boardArray.GetLength(1); y++)
//                 {
//                     Piece piece = pieceArray[x, y];
//                     newPieceArray[x, y] = (piece != null) ? new Piece(piece.pos, piece.belongsTo) : null;
//                 }
//             }

//             return newPieceArray;
//         }

//         public void ResetPieceArray()
//         {
//             pieceArray = new Piece[boardArray.GetLength(0), boardArray.GetLength(1)];
//         }
//         public void ResetPieceViewArray()
//         {
//             pieceViewArray = new PieceObject[boardArray.GetLength(0), boardArray.GetLength(1)];
//         }
//         public NodeObject[,] boardViewArray;
//         public PieceObject[,] pieceViewArray;

//         public Node GetNode(Vector2Int pos) => boardArray[pos.x, pos.y];
//         public Piece GetPiece(Vector2Int pos) => pieceArray[pos.x, pos.y];
//         public PieceObject GetVisualPiece(Vector2Int pos) => pieceViewArray[pos.x, pos.y];

//         public NodeObject GetVisualNode(Vector2Int pos) => boardViewArray[pos.x, pos.y];

//         public void RemovePieceAt(Vector2Int pos)
//         {
//             pieceArray[pos.x, pos.y] = null;
//         }
//         public void InsertPieceAt(Vector2Int pos, Piece newPiece)
//         {
//             pieceArray[pos.x, pos.y] = newPiece;
//         }

//         public void RemovePieceViewAt(Vector2Int pos)
//         {
//             pieceViewArray[pos.x, pos.y] = null;
//         }
//         public void InsertPieceViewAt(Vector2Int pos, PieceObject obj)
//         {
//             pieceViewArray[pos.x, pos.y] = obj;
//         }

//         public void SetupLoadedBoard(SaveData data, List<UserModel> players, PieceObject prefab)
//         {
//             pieceArray = new Piece[boardArray.GetLength(0), boardArray.GetLength(1)];
//             foreach (var obj in pieceViewArray)
//             {
//                 if (obj != null)
//                     MonoBehaviour.Destroy(obj.gameObject);
//             }
//             pieceViewArray = new PieceObject[boardArray.GetLength(0), boardArray.GetLength(1)];
//             foreach (var user in players)
//             {
//                 foreach (var pieceData in data.savedBoard)
//                 {
//                     if (pieceData == null) continue;
//                     if (user.currentTeam != pieceData.belongsTo) continue;
//                     Debug.Log(user.currentTeam);
//                     Piece newPiece = new Piece(pieceData);
//                     user.playerPieces.Add(newPiece);
//                     pieceViewArray[newPiece.x, newPiece.y] = PieceObject.CreatePieceObject(prefab, newPiece.worldPos, (PieceColor)newPiece.belongsTo, user.transform, newPiece.pos);
//                     pieceArray[newPiece.x, newPiece.y] = newPiece;
//                 }
//             }

//         }

//     }

//     public class Turn : IComparable
//     {

//         public Vector2Int movedPiece;
//         public Vector2Int target;
//         float value;
//         public float Value => value;

//         public Turn() { }

//         public Turn(Vector2Int _piece, Vector2Int _node, float _value)
//         {
//             movedPiece = _piece;
//             target = _node;
//             value = _value;

//         }

//         public int CompareTo(object obj)
//         {
//             return (value > (obj as Turn).value) ? 1 : (value < (obj as Turn).value) ? -1 : 0;
//         }

//         public List<Turn> Expand(UserModel model, Board currentBoard)
//         {
//             List<Turn> output = new List<Turn>();
//             for (int i = 0; i < model.playerPieces.Count; i++)
//             {
//                 Piece piece = model.playerPieces[i];
//                 List<Node> list = model.GetPath(currentBoard.GetNode(piece.pos), new List<Node>(), true, false);
//                 for (int i1 = 0; i1 < list.Count; i1++)
//                 {
//                     Node node = list[i1];
//                     Piece[,] simulatedBoard = currentBoard.GetSimulateABoard();
//                     Piece simPiece = simulatedBoard[piece.x, piece.y];
//                     Node simNode = originalBoard
//                         .GetNode(node.pos);
//                     Vector2Int oldPos = simPiece.pos;
//                     Board newBoard = new Board();
//                     newBoard.pieceArray = simulatedBoard;
//                     model.SimulateMove(simPiece, simNode, newBoard);
//                     float eval = EvaluateState(model, newBoard);

//                     Turn turn = new Turn(oldPos, simNode.pos, eval);
//                     output.Add(turn);
//                 }
//             }
//             return output;
//         }

//         float EvaluateState(UserModel model, Board customBoard)
//         {
//             float dist = 0;
//             foreach (Vector2Int pos in UserModel.GetPlayerPositions(model.currentTeam, customBoard))
//             {
//                 dist -= Vector2Int.Distance(pos, model.opponentGoal);

//             }
//             return dist;
//         }
//     }

//     public static Board originalBoard;

// }

// [System.Serializable]
// public class Node
// {
//     public Node(Vector2Int pos, UnityEngine.Vector2 worldPos, Team currentTeam)
//     {
//         this.pos = pos;
//         belongsTo = currentTeam;
//         this.worldPos = worldPos;
//     }

//     public Vector2Int pos;
//     public UnityEngine.Vector2 worldPos;
//     public Team belongsTo;
// }

// [System.Serializable]
// public class Piece
// {
//     public Piece(Vector2Int _pos, Team currentTeam)
//     {
//         pos = _pos;
//         belongsTo = currentTeam;
//     }

//     public Piece(PieceData data)
//     {
//         pos = new Vector2Int(data.boardPos.x, data.boardPos.y);
//         worldPos = new UnityEngine.Vector2(data.worldPos.x, data.worldPos.y);
//         belongsTo = data.belongsTo;
//     }

//     // public Vector2Int pos;
//     public Vector2Int pos
//     {
//         get
//         {
//             return new Vector2Int(x, y);
//         }
//         set
//         {
//             x = value.x;
//             y = value.y;
//         }
//     }

//     public int x, y;
//     public Team belongsTo;
//     public UnityEngine.Vector2 worldPos
//     {
//         get
//         {
//             return new UnityEngine.Vector2(wX, wY);
//         }
//         set
//         {
//             wX = value.x;
//             wY = value.y;
//         }
//     }
//     public float wX, wY;
// }

#endregion

#region NewBoardModel
[System.Serializable]
public class TestNode
{

    public Vector2Int pos;
    public Vector2 worldPos;
    public Team belongsTo;

    public bool countAsBase;

    public TestNode(Vector2Int boardPos, Vector2 transformPos, Team currentTeam)
    {
        pos = boardPos;
        worldPos = transformPos;
        belongsTo = currentTeam;
        if (currentTeam != Team.Unoccupied && currentTeam != Team.None && currentTeam != Team.BigGreen && currentTeam != Team.BigRed)
        {
            countAsBase = false;
        }
        countAsBase = true;
    }

    public TestNode() { }
}

[System.Serializable]
public class TestPiece
{

    public Vector2Int pos;
    public Vector2 worldPos;

    public Team belongsTo;

    public TestPiece(TestNode node)
    {
        pos = node.pos;
        worldPos = node.worldPos;
        belongsTo = node.belongsTo;
    }

    public TestPiece(TestPiece piece)
    {
        pos = piece.pos;
        worldPos = piece.worldPos;
        belongsTo = piece.belongsTo;
    }

    public float offsetValue = 0;

    public TestPiece()
    {

    }

}

public static class TestBoardModel
{

    public class Board
    {
        public TestNode[,] boardArr;
        public TestNode this[int a, int b]
        {
            get => boardArr[a, b];
            set => boardArr[a, b] = value;
        }

        #region Properties
        public TestNode[,] Grid { set => boardArr = value; }

        public int GetLength(int a) => boardArr.GetLength(a);
        public TestNode FindReference(Vector2Int value) => (value.x >= boardArr.GetLength(0) || value.y >= boardArr.GetLength(1) || value.x < 0 || value.y < 0) ? new TestNode() : boardArr[value.x, value.y];

        public TestNode FindReference(TestPiece value) => (boardArr[value.pos.x, value.pos.y]);

        public IEnumerator GetEnumerator() => boardArr.GetEnumerator();

        public List<TestPiece> Clone()
        {
            List<TestPiece> result = new List<TestPiece>();
            for (int i = 0; i < globalPieceList.Count; i++)
            {
                result.Add(new TestPiece(TestBoardModel.globalPieceList[i]));
            }
            return result;
        }

        public List<TestNode> FindAll(Predicate<TestNode> del)
        {
            List<TestNode> result = new List<TestNode>();
            for (int x = 0; x < boardArr.GetLength(0); x++)
            {
                for (int y = 0; y < boardArr.GetLength(1); y++)
                {
                    result.Add(boardArr[x, y]);
                }

            }
            result = result.FindAll(del);
            return result;
        }

        public TestNode Find(Predicate<TestNode> del)
        {
            List<TestNode> result = new List<TestNode>();
            for (int x = 0; x < boardArr.GetLength(0); x++)
            {
                for (int y = 0; y < boardArr.GetLength(1); y++)
                {
                    result.Add(boardArr[x, y]);
                }

            }
            TestNode node = result.Find(del);
            return node;
        }


    }
    #endregion

    public static Board test_OriginalBoard;
    public static List<TestPiece> globalPieceList;

    public static List<NodeObject> globalNodeViewList;

    public class Move : IComparable
    {

        public float value;

        public Vector2Int currentPiece, target;

        public Move(Vector2Int _currentPiece, Vector2Int _target, float _value)
        {
            value = _value;
            currentPiece = _currentPiece;
            target = _target;
        }

        public Move() { }

        public int CompareTo(object obj)
        {
            return ((obj as Move).value > value) ? 1 : ((obj as Move).value < value) ? -1 : 0;
        }

        public List<Move> Expand(UserModel owner)
        {
            List<Move> output = new List<Move>();
            for (int i = 0; i < owner.OwnedPieces.Count; i++)
            {
                TestPiece piece = owner.OwnedPieces[i];
                List<Vector2Int> path = owner.PathOfMoves(piece.pos, new List<Vector2Int>(), true);
                for (int a = 0; a < path.Count; a++)
                {
                    //Simulate a board
                    List<TestPiece> simulatedBoard = test_OriginalBoard.Clone();
                    int newPiece = 0;
                    for (int p = 0; p < simulatedBoard.Count; p++)
                    {

                        if (simulatedBoard[p].pos == piece.pos)
                        {
                            newPiece = p;
                            break;
                        }
                    }
                    Vector2Int currentPiece = simulatedBoard[newPiece].pos;
                    Vector2Int target = path[a];


                    owner.Simulate_MovePiece(newPiece, target, simulatedBoard);
                    TestNode goal = owner.DesiredGoal();
                    float value = EvaluateMove(simulatedBoard, owner, goal);
                    Move move = new Move(currentPiece, target, value);
                    output.Add(move);
                }

                //Return the list.
            }
            return output;
        }


        float EvaluateMove(List<TestPiece> board, UserModel user, TestNode goal)
        {
            float value = 0;

            foreach (TestPiece ownedPiece in UserModel.GetPlayerPositions(user.currentTeam, board))
            {
                Color rayColor = Color.cyan;
                value += Vector2.Distance(ownedPiece.pos, goal.pos);
                if (DoesPieceExistIn(ownedPiece, UserModel.GetOpponent(user)))
                {
                    value -= 40;
                    rayColor = Color.green;
                }

                if (DoesPieceExistIn(ownedPiece, user.currentTeam) || PieceLiesInAnUninterestingArea(ownedPiece, user))
                {
                    value += 1000;
                    rayColor = Color.yellow;
                }

                if(UserModel.CheckForNeighbours(ownedPiece, user).Any(p => p.belongsTo != user.currentTeam) && DoesPieceExistIn(ownedPiece, user.currentTeam)){
                    value -= 40;
                    rayColor = Color.red;
                }
                Debug.DrawLine(ownedPiece.worldPos, goal.worldPos, rayColor, 0.5f);
            }
            return -value;
        }

        private bool PieceLiesInAnUninterestingArea(TestPiece ownedPiece, UserModel user)
        {
            return TestManager.ins.allPlayers.Any(x => user != x && DoesPieceExistIn(ownedPiece, x.currentTeam) && x.currentTeam != UserModel.GetOpponent(user));
        }

        private static bool DoesPieceExistIn(TestPiece ownedPiece, Team desiredTeam)
        {
            return GetBelongsTo(ownedPiece) == desiredTeam;
        }

        private static Team GetBelongsTo(TestNode value)
        {
            return test_OriginalBoard[value.pos.x, value.pos.y].belongsTo;
        }

        private static Team GetBelongsTo(TestPiece value)
        {
            return test_OriginalBoard.FindReference(value).belongsTo;
        }

        
        //float EvaluateState(UserModel model, Board customBoard)
        //         {
        //             float dist = 0;
        //             foreach (Vector2Int pos in UserModel.GetPlayerPositions(model.currentTeam, customBoard))
        //             {
        //                 dist -= Vector2Int.Distance(pos, model.opponentGoal);

        //             }
        //             return dist;
        //         }
    }

}
#endregion