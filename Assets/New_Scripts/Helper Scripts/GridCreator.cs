using UnityEngine;
using static BoardModel;
namespace GridCreator
{

    public static class HexagonGrid
    {
        const float deltaX = 0.5f;
        const float deltaY = 0.8660254f;
        static Vector2 SetPosition(Vector2Int boardPos)
        {
            return new Vector2(boardPos.x + deltaX * boardPos.y, boardPos.y * deltaY);
        }
        public static T[,] CreateHexagonGrid<T, P>(int[,] blueprint, P prefab)
        where P : MonoBehaviour
        where T : new()
        {
            Transform parent = new GameObject($"{prefab.name}'s list.").transform;
            T[,] board = new T[blueprint.GetLength(0), blueprint.GetLength(1)];
            int xPos;
            for (int x = 0; x < blueprint.GetLength(0); x++)
            {
                for (int y = 0; y < blueprint.GetLength(1); y++)
                {
                    xPos = y - x / 2;
                    board[x, y] = new T();
                    MonoBehaviour.Instantiate(prefab, SetPosition(new Vector2Int(xPos, y)), Quaternion.identity, parent);
                }
            }
            return board;
        }
    }



    public static class ChineseCheckers
    {
        const float deltaX = 0.5f;
        const float deltaY = 0.8660254f;

        public const float centerPointX = 2.125f;
        public const float centerPointY = 5f;
        public static Vector2 SetPosition(Vector2 boardPos)
        {
            return new Vector2(boardPos.x + deltaX * boardPos.y, boardPos.y * deltaY);
        }
        public static Board CreateHexagonGrid(int[,] blueprint, NodeObject prefab)
        {
            Transform parent = new GameObject($"{prefab.name}'s list.").transform;
            Board newBoard = new Board();
            newBoard.boardArray = new Node[blueprint.GetLength(0), blueprint.GetLength(1)];
            newBoard.pieceArray = new Piece[blueprint.GetLength(0), blueprint.GetLength(1)];
            newBoard.boardViewArray = new NodeObject[blueprint.GetLength(0), blueprint.GetLength(1)];
            newBoard.pieceViewArray = new PieceObject[blueprint.GetLength(0), blueprint.GetLength(1)];
            int xPos;
            for (int x = 0; x < blueprint.GetLength(0); x++)
            {
                for (int y = 0; y < blueprint.GetLength(1); y++)
                {
                    xPos = y - x / 2;
                    Team currentTeam =
                    (blueprint[x, y] > 8 && blueprint[x, y] != 11) ?
                    (blueprint[x, y] == 14 || blueprint[x, y] == 19) ?
                        Team.BigRed : (blueprint[x, y] == 16 || blueprint[x, y] == 17) ?
                            Team.BigGreen : Team.Unoccupied :
                        (Team)blueprint[x, y];
                    Vector2 objectPos =
                    SetPosition(new Vector2Int(x, xPos) -
                    new Vector2(newBoard.boardArray.GetLength(0) / centerPointX, newBoard.boardArray.GetLength(1) / centerPointY));
                    newBoard.boardArray[x, y] = new Node(new Vector2Int(x, y), objectPos, currentTeam);
                    newBoard.boardViewArray[x, y] = NodeObject.CreateNodeObject(prefab, objectPos, (NodeColor)blueprint[x, y], parent, newBoard.boardArray[x, y].pos);
                }
            }

            return newBoard;
        }
    }


}
