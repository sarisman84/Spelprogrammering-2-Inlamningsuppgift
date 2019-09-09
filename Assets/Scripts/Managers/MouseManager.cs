using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using UnityEngine;

//This script handles the player input.
//NOTE: THIS SCRIPT IS MEANT ONLY TO TEST THE REST OF THE PROJECT AND THEREFOR WILL BE REPLACED!!
public class MouseManager : MonoBehaviour {
    #region Common Variables
    public LayerMask mask;
    bool input;

    Vector3 mousePos;

    Camera cam;

    RaycastHit2D go;
    #endregion

    public Node.Team team;
    [SerializeField] Node currentNode, selectedNode;

    [SerializeField] Node[] validMoves;

    private void Awake () {
        cam = Camera.main;
    }

    void Update () {
        #region Gameobject Detection
        //First, get the mouse position from screen coordinates to world coordinates.
        mousePos = cam.ScreenToWorldPoint (Input.mousePosition);

        //Then, set that position as the center of an Physics2D.OverlapCircle with a small radius. 
        //AGAIN: THIS IS TEMPRORARY AND IS MEANT TO BE REPLACED LATER!!!
        go = Physics2D.Raycast (mousePos - new Vector3 (0, 0, 10), cam.transform.forward, mask.value);
        Debug.DrawRay (mousePos, cam.transform.forward * 100f, Color.red);

        //Get an input from the left mouse button when pressed down.
        input = Input.GetMouseButtonDown (0);
        #endregion

        if (go.collider == null) return;
        Node node = go.collider.GetComponent<Node> ();

        UserManager.OnActionTaken (node, ref currentNode, ref selectedNode, team, input, Input.GetKeyDown (KeyCode.Space));

    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (go.point, 0.1f);
        Gizmos.color = Color.red;
        if (currentNode != null)
            foreach (var node in BoardManager.ValidMoves (currentNode, false)) {
                if (node != null)
                    Gizmos.DrawWireSphere (node.transform.position, 0.5f);
            }
        Gizmos.color = Color.blue;
        if (selectedNode != null)
            Gizmos.DrawWireCube (selectedNode.transform.position, new Vector3 (0.5f, 0.5f, 0.5f));
    }

}