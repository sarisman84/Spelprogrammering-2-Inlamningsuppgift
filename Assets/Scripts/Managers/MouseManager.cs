using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the player input.
//NOTE: THIS SCRIPT IS MEANT ONLY TO TEST THE REST OF THE PROJECT AND THEREFOR WILL BE REPLACED!!
public class MouseManager : MonoBehaviour {

    public LayerMask mask;
    bool toggle = false;
    NodeManager _manager;
    bool input;

    Vector2 mousePos;
    private void Awake () {

        _manager = new NodeManager (mask);
    }
    void Update () {
        //First, get the mouse position from screen coordinates to world coordinates.
        mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

        //Then, set that position as the center of an Physics2D.OverlapCircle with a small radius. 
        //AGAIN: THIS IS TEMPRORARY AND IS MEANT TO BE REPLACED LATER!!!
        Collider2D go = Physics2D.OverlapCircle (mousePos, 0.1f, mask.value);

        //Get an input from the left mouse button when pressed down.
        input = Input.GetMouseButtonDown (0);

        if (go == null) return;

        //When the player has pressed the Left Mouse Button Once: Get the component of the node then AttemptToMovePiece().
        if (!input) return;
        Node node = go.GetComponent<Node> ();
        AttemptToMovePiece (node);

    }

    //This function calls the 3 methods from the NodeManager. 
    //AGAIN: THIS IS TEMPORARY AND IS MEANT TO BE REPLACED LATER!!!
    private void AttemptToMovePiece (Node node) {
        _manager.GetPiece (node);
        _manager.GetTargetPosition (node);
        _manager.MovePiece ();
    }

    //A debug Gizmo to see if the mouse works.
    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (mousePos, 0.1f);
    }
}