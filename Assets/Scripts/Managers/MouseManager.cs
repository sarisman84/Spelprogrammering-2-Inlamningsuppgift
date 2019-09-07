using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the player input.
//NOTE: THIS SCRIPT IS MEANT ONLY TO TEST THE REST OF THE PROJECT AND THEREFOR WILL BE REPLACED!!
public class MouseManager : MonoBehaviour
{

    public LayerMask mask;
    bool toggle = false;
    NodeManager _manager;
    bool input;

    Vector3 mousePos;

    Camera cam;

    RaycastHit2D go;
    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        //First, get the mouse position from screen coordinates to world coordinates.
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Then, set that position as the center of an Physics2D.OverlapCircle with a small radius. 
        //AGAIN: THIS IS TEMPRORARY AND IS MEANT TO BE REPLACED LATER!!!
        go = Physics2D.Raycast(mousePos - new Vector3(0, 0, 10), cam.transform.forward, mask.value);
        Debug.DrawRay(mousePos, cam.transform.forward * 100f, Color.red);

        //Get an input from the left mouse button when pressed down.
        input = Input.GetMouseButtonDown(0);

        if (go.collider == null) return;
        Node node = go.collider.GetComponent<Node>();
        Debug.Log(node.CurrentBoardPosition);
        //When the player has pressed the Left Mouse Button Once: Get the component of the node then AttemptToMovePiece().
        if (!input) return;

        NodeManager.ValidMoves(node);

    }



    //A debug Gizmo to see if the mouse works.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(go.point, 0.1f);
    }
}