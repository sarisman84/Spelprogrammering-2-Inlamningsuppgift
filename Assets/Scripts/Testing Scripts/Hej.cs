using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ChineseCheckers {

    static class HexagonGrid {

        //I need to do the following:

        /*
        A:
            1. Import all gameObjects in the scene to a multiDimentionalArray and store them there in the correct order.
            2. Be able to access the correct gameObject using a coordinate system on the array.
        B:
            1.Create a box of correctly placed gameObjects in the scene then import then to the array.
            These are used to convert a position to coordinates (see further BoardView).
            private const float deltaX = 0.5f;
            private const float deltaY = 0.8660254f; // √3/2

            (return (new Vector2(pos.x + deltaX * pos.y, pos.y * deltaY))
            2.Use a "blueprint" of some sort (either an enum or a multiDimentionalArray of numbers) to create a shape.
            3.Be able to access the correct gameObject using a coordinate system on the array.
         */

        const float deltaX = 0.5f;
        const float deltaY = 0.8660254f;

        static Vector2 SetPosition (Vector2Int boardPos) {
            return new Vector2 (boardPos.x + deltaX * boardPos.y, boardPos.y * deltaY);
        }

        /// <summary>
        /// Creates a Square Grid in a Hexagonical Pattern.
        /// </summary>
        /// <param name="blueprint"> This is used as a way to "shape" the board. Create an int[,] to set the boards size as well as how it will be shaped.</param>
        /// <param name="prefab">The prefab in question that is going to be used as a board piece. </param>
        /// <returns>A list of board pieces that shape a board. </returns>
        public static Node[, ] CreateGrid (int[, ] blueprint, Node prefab) {
            Transform parent = new GameObject ($"{prefab.name}'s list ").transform;
            Node[, ] tempGrid = new Node[blueprint.GetLength (0), blueprint.GetLength (1)];
            Canvas newCanvas = new GameObject ("Text").AddComponent<Canvas> ();
            int xPos;
            for (int y = 0; y < blueprint.GetLength (0); y++) {
                for (int x = 0; x < blueprint.GetLength (1); x++) {

                    tempGrid[y, x] = Node.CreateNode (prefab, blueprint[y, x], parent);
                    xPos = x - y / 2;
                    tempGrid[y, x].transform.position = SetPosition (new Vector2Int (x, y));
                    tempGrid[y, x].CurrentBoardPosition = new Vector2Int (y, x);

                    //Need to make a text function that displays a coordinate.

                }

            }
            return tempGrid;
        }

    }

}