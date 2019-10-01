using System;
using System.Collections.Generic;
using UnityEngine;
using static BoardModel;
public class HumanPlayer : UserModel {
    RaycastHit2D hit2D;
    Camera cam;

    bool isTurnEnabled = false;
    private void OnEnable () {
        cam = Camera.main;
    }

    [SerializeField] Piece selectedPiece;
    [SerializeField] Node selectedNode;

    public void OnGameAwake () {
        throw new NotImplementedException ();
    }
    public void OnTurnTaken () {

        Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
        hit2D = Physics2D.Raycast (mousePos, cam.transform.forward);
        if (hit2D.collider == null) return;
        if (Input.GetMouseButtonDown (0)) {
            NodeObject foundNode = hit2D.collider.GetComponent<NodeObject> ();
            if (foundNode != null) {

                GetTarget (foundNode);
                GetSelectedPiece (foundNode);
                MovePiece (ref selectedPiece, ref selectedNode, ref path);

            }

        }
        if (Input.GetKeyDown (KeyCode.Tab)){
            EndTurn();
        }

    }

    private void Update () {
        if (isTurnEnabled)
            OnTurnTaken ();
    }

    public override void StartTurn () {
        isTurnEnabled = true;
    }

    public override void EndTurn () {
        isTurnEnabled = false;
        GameModel.PlayerDone();
    }

    private void GetTarget (NodeObject foundNode) {
        if (path == null) return;
        if (FromBoard (selectedPiece) == FromBoard (foundNode)) return;
        Node potentialTarget = FromBoard (foundNode);
        foreach (var node in path) {

            if (potentialTarget == node) {
                selectedNode = potentialTarget;
                return;
            }
        }
    }

    List<Node> path;
    private void GetSelectedPiece (NodeObject foundNode) {

        //find a piece within said node that is of the same team and store it.
        //Reset any previous paths recorded.
        ResetPath (ref path);

        selectedPiece = (selectedNode != null) ? selectedPiece : originalBoard.GetPiece (foundNode.boardCoordinate);
        if (selectedPiece == null) return;

        path = GetPath (BoardModel.originalBoard.GetNode (this.selectedPiece.pos), new List<Node> (), true, true);

    }

    private static Node FromBoard (NodeObject foundNode) {
        return originalBoard.GetNode (foundNode.boardCoordinate);
    }

    private static Node FromBoard (Piece foundNode) {
        if (foundNode == null) return null;
        return originalBoard.GetNode (foundNode.pos);
    }

}