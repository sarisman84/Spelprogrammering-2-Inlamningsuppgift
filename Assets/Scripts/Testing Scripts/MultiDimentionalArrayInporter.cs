using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spyro;
using UnityEngine;

public class MultiDimentionalArrayInporter {

    static MultiDimentionalArrayInporter instance;

    MultiDimentionalArrayInporter () { }
    public static MultiDimentionalArrayInporter Do {
        get {
            return instance = instance ?? new MultiDimentionalArrayInporter ();
        }
    }

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

    public Vector2 SetPosition (Vector2Int boardPos) {
        return new Vector2 (boardPos.x + deltaX * boardPos.y, boardPos.y * deltaY);
    }

    public T[, ] SetUpGrid<T> (int size, T prefab) where T : MonoBehaviour {
        Transform parent = new GameObject ($"{prefab.name}'s list ").transform;
        T[, ] tempGrid = new T[size, size];
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                tempGrid[y, x] = MonoBehaviour.Instantiate (prefab, parent) as T;
                tempGrid[y, x].transform.position = SetPosition (new Vector2Int (y, x));
            }
        }
        return tempGrid;
    }
}