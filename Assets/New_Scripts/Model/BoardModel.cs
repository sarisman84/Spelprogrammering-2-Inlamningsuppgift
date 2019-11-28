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



#region NewBoardModel
[System.Serializable]
public class BoardNode
{

    public Vector2Int pos;
    public Vector2 worldPos;
    public Team belongsTo;



    public BoardNode(Vector2Int boardPos, Vector2 transformPos, Team currentTeam)
    {
        pos = boardPos;
        worldPos = transformPos;
        belongsTo = currentTeam;
    }

    public BoardNode(BoardNode node)
    {
        pos = node.pos;
        worldPos = node.worldPos;
        belongsTo = node.belongsTo;
    }

    public BoardNode() { }
}

[System.Serializable]
public class BoardPiece
{

    public Vector2Int pos;
    public Vector2 worldPos;

    public Team belongsTo;

    public BoardPiece(BoardNode node)
    {
        pos = node.pos;
        worldPos = node.worldPos;
        belongsTo = node.belongsTo;
    }

    public BoardPiece(BoardPiece piece)
    {
        pos = piece.pos;
        worldPos = piece.worldPos;
        belongsTo = piece.belongsTo;
    }

    public float offsetValue = 0;

    public BoardPiece()
    {

    }

}

public static class TestBoardModel
{

    public class Board
    {
        public BoardNode[,] boardArr;
        public BoardNode this[int a, int b]
        {
            get => boardArr[a, b];
            set => boardArr[a, b] = value;
        }

        public Board(int sizeX, int sizeY)
        {
            boardArr = new BoardNode[sizeX, sizeY];
        }

        #region Properties // These are used to get a direct reference to the boardArray.
        public BoardNode[,] Grid { set => boardArr = value; }


        public int GetLength(int a) => boardArr.GetLength(a);
        public BoardNode FindReference(Vector2Int value) => (value.x >= boardArr.GetLength(0) || value.y >= boardArr.GetLength(1) || value.x < 0 || value.y < 0) ? new BoardNode() : boardArr[value.x, value.y];

        public BoardNode FindReference(BoardPiece value) => (boardArr[value.pos.x, value.pos.y]);

        public IEnumerator GetEnumerator() => boardArr.GetEnumerator();

        /// <summary>
        /// Creates a literal clone of the globalPieceList. (Since Clone() only returns a reference point to the original list.)
        /// </summary>
        /// <returns>A literal clone.</returns>
        public List<BoardPiece> Clone()
        {
            List<BoardPiece> result = new List<BoardPiece>();
            for (int i = 0; i < globalPieceList.Count; i++)
            {
                result.Add(new BoardPiece(TestBoardModel.globalPieceList[i]));
            }
            return result;
        }

        /// <summary>
        /// Creates a literal clone of the boardArr. (Since Clone() only returns a reference point to the original array.)
        /// </summary>
        /// <returns>A literal clone.</returns>
        public BoardNode[,] SaveBoard()
        {
            BoardNode[,] output = new BoardNode[boardArr.GetLength(0), boardArr.GetLength(1)];
            for (int h = 0; h < output.GetLength(0); h++)
            {
                for (int i = 0; i < output.GetLength(1); i++)
                {
                    output[h, i] = new BoardNode(boardArr[h, i]);
                }
            }
            return output;
        }


        /// <summary>
        /// A replication of the Linq.FindAll() method.
        /// </summary>
        /// <param name="del">A predicate delegate that mimics the behaivour of the original method.</param>
        /// <returns>A list of elements from the desired predicate.</returns>
        public List<BoardNode> FindAll(Predicate<BoardNode> del)
        {
            List<BoardNode> result = new List<BoardNode>();
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

        /// <summary>
        /// A replication of the Linq.Find() method.
        /// </summary>
        /// <param name="del">A predicate delegate that mimics the behaivour of the original method.</param>
        /// <returns>A list of elements from the desired predicate.</returns>
        public BoardNode Find(Predicate<BoardNode> del)
        {
            List<BoardNode> result = new List<BoardNode>();
            for (int x = 0; x < boardArr.GetLength(0); x++)
            {
                for (int y = 0; y < boardArr.GetLength(1); y++)
                {
                    result.Add(boardArr[x, y]);
                }

            }
            BoardNode node = result.Find(del);
            return node;
        }

        
        public int Length => boardArr.Length;


    }
    #endregion

    public static Board test_OriginalBoard;


    //I have seperated the BoardPiece list from the Board class since we only want to interact with this list and not the static board.
    public static List<BoardPiece> globalPieceList;

    //I have also seperated the list for the viewNodes for the same reason. Then again, i could always add this back into the board class.
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

        //Impementation of IComparable for the Gnome sort algorithm.
        public int CompareTo(object obj)
        {
            return ((obj as Move).value > value) ? 1 : ((obj as Move).value < value) ? -1 : 0;
        }

        //Note: A move (this class) is deficed by the selected piece, the selected destination for said piece and a value for the move.
        /// <summary>
        /// Mainline Method: Gets the child moves of this current one.
        /// </summary>
        /// <param name="owner">Which user are we used from?</param>
        /// <returns>A list of moves</returns>
        public List<Move> Expand(UserModel owner)
        {
            List<Move> output = new List<Move>();
            for (int i = 0; i < owner.OwnedPieces.Count; i++)
            {
                BoardPiece piece = owner.OwnedPieces[i];
                List<Vector2Int> path = owner.PathOfMoves(piece.pos, new List<Vector2Int>(), true);
                for (int a = 0; a < path.Count; a++)
                {
                    //Simulate a board
                    List<BoardPiece> simulatedBoard = test_OriginalBoard.Clone();
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
                    BoardNode goal = owner.DesiredGoal();
                    float value = EvaluateMove(simulatedBoard, owner, goal);
                    Move move = new Move(currentPiece, target, value);
                    output.Add(move);
                }

                //Return the list.
            }
            return output;
        }

        /// <summary>
        /// Helper method: rates the move depending on distance.
        /// </summary>
        /// <param name="board">The current simulated board we are working in.</param>
        /// <param name="user">The current user we are working with.</param>
        /// <param name="goal">The current goal we are trying to go to.</param>
        /// <returns>A score for the current move.</returns>
        float EvaluateMove(List<BoardPiece> board, UserModel user, BoardNode goal)
        {
            float value = 0;

            foreach (BoardPiece ownedPiece in UserModel.GetPlayerPositions(user.currentTeam, board))
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

                if (UserModel.CheckForNeighbours(ownedPiece, user).Any(p => p.belongsTo != user.currentTeam) && DoesPieceExistIn(ownedPiece, user.currentTeam))
                {
                    value -= 400;
                    rayColor = Color.red;
                }
                if (DoesPieceExistIn(ownedPiece, UserModel.GetOpponent(user)) && GetBelongsTo(goal) == UserModel.GetOpponent(user))
                {
                    value -= 400;
                    rayColor = Color.gray;
                }
                if (TestManager.ins.EnableDebug)
                    Debug.DrawLine(ownedPiece.worldPos, goal.worldPos, rayColor, 0.5f);
            }
            return -value;
        }

        //The following are used as conditions for setting different score values on different scenarios.

        private bool PieceLiesInAnUninterestingArea(BoardPiece ownedPiece, UserModel user)
        {
            return TestManager.ins.allPlayers.Any(x => user != x && DoesPieceExistIn(ownedPiece, x.currentTeam) && x.currentTeam != UserModel.GetOpponent(user));
        }

        private static bool DoesPieceExistIn(BoardPiece ownedPiece, Team desiredTeam)
        {
            return GetBelongsTo(ownedPiece) == desiredTeam;
        }

        private static Team GetBelongsTo(BoardNode value)
        {
            return test_OriginalBoard[value.pos.x, value.pos.y].belongsTo;
        }

        private static Team GetBelongsTo(BoardPiece value)
        {
            return test_OriginalBoard.FindReference(value).belongsTo;
        }
    }

}
#endregion