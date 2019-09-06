using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spyro;
using UnityEngine;

public class GOManager : MonoBehaviour {

    public int gridSize;
    Node[, ] grid;

    public Node test;
    private void Awake () {
        //Create the objects
        grid = MultiDimentionalArrayInporter.Do.SetUpGrid<Node> (gridSize, test);
    }

}