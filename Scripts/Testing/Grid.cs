using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Grid
{


    public static TestingVariable[,] CreateGrid(int sizeX, int sizeY)
    {
        Transform parent = new GameObject($"TestingVariable's grid ").transform;
        TestingVariable[,] tempGrid = new TestingVariable[sizeX, sizeY];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {

                tempGrid[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<TestingVariable>();
                tempGrid[x, y].transform.position = new Vector2(x, y) - new Vector2(sizeX/2, sizeY/2);
                tempGrid[x, y].transform.parent = parent;
                tempGrid[x, y].BoardPosition = new Vector2Int(x, y);
                tempGrid[x, y].name = $"Node:{x},{y}";

                //Need to make a text function that displays a coordinate.

            }

        }
        return tempGrid;
    }
}
