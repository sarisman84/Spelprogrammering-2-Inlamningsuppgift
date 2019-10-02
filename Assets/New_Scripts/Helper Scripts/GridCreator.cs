using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static TestBoardModel;
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
        public static TestBoardModel.Board CreateHexagonGrid(int[,] blueprint, NodeObject prefab)
        {
            Transform parent = CreateCanvas($"{prefab.name}'s list.");
            Board newBoard = new TestBoardModel.Board();
            globalNodeViewList = new List<NodeObject>();
            newBoard.Grid = new TestNode[blueprint.GetLength(0), blueprint.GetLength(1)];
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
                    Vector2Int boardPos = new Vector2Int(x, y);
                    Vector2 objectPos = SetPosition(new Vector2Int(x, xPos) - new Vector2(newBoard.GetLength(0) / centerPointX, newBoard.GetLength(1) / centerPointY));
                    newBoard[x, y] = new TestNode(boardPos, objectPos, currentTeam);
                    globalNodeViewList.Add(NodeObject.CreateNodeObject(prefab, objectPos, (NodeColor)blueprint[x, y], parent, boardPos));
                }
            }

            return newBoard;
        }

        private static Transform CreateCanvas(string v)
        {
            GameObject parent = new GameObject(v);
            Canvas canvas = parent.AddComponent<Canvas>();
            CanvasScaler scaler = parent.GetComponent<RectTransform>().gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            return parent.transform;
        }
    }


}
