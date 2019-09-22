using System.Collections.Generic;
using UnityEngine;
using System;
using static BoardModel;
public class HumanPlayer : UserModel
{
    RaycastHit2D hit2D;
    Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
    }
    public HumanPlayer(Team team)
    {
        currentTeam = team;
    }
    public override void OnTurnTaken()
    {



        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        hit2D = Physics2D.Raycast(mousePos, cam.transform.forward);
        if (hit2D.collider == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            NodeObject foundNode = hit2D.collider.GetComponent<NodeObject>();
            if (foundNode != null)
            {
                GetSelectedPiece(foundNode);
                GetTarget(foundNode);
                MovePiece(this.selectedPiece, this.selectedNode);

            }


        }



    }

    private void MovePiece(Piece selectedPiece, Node selectedNode)
    {
        if (this.selectedPiece == null || this.selectedNode == null) return;

        PieceObject pieceToMove = originalBoard.GetVisualPiece(this.selectedPiece.currentPosition);
        //Model move
        originalBoard.RemovePieceAt(this.selectedPiece.currentPosition);
        this.selectedPiece.currentPosition = this.selectedNode.currentPosition;
        originalBoard.InsertPieceAt(this.selectedNode.currentPosition, this.selectedPiece);

        //View move
        pieceToMove.transform.position = this.selectedNode.worldPosition;
        pieceToMove.boardCoordinate = this.selectedNode.currentPosition;
        originalBoard.RemovePieceViewAt(pieceToMove.boardCoordinate);
        originalBoard.InsertPieceViewAt(this.selectedNode.currentPosition, pieceToMove);
        selectedPiece = null;
        this.selectedPiece = null;
        selectedNode = null;
        this.selectedNode = null;
        ResetPath();
    }

    private void GetTarget(NodeObject foundNode)
    {
        if (path == null) return;
        if (FromBoard(selectedPiece) == FromBoard(foundNode)) return;
        Node potentialTarget = FromBoard(foundNode);
        foreach (var node in path)
        {

            if (potentialTarget == node)
            {
                selectedNode = potentialTarget;
                return;
            }
        }
    }

    List<Node> path;
    private void GetSelectedPiece(NodeObject foundNode)
    {
        Node node = FromBoard(foundNode);
        //find a piece within said node that is of the same team and store it.
        //Reset any previous paths recorded.
        ResetPath();
        foreach (Piece piece in this.playerPieces)
        {

            if (piece.currentPosition == node.currentPosition)
            {
                this.selectedPiece = piece;
                break;
            }

        }
        if (this.selectedPiece != null)
        {
            path = GetPath(BoardModel.originalBoard.GetNode(this.selectedPiece.currentPosition), new List<Node>(), true, this);

        }

        if (selectedPiece == null) return;

    }

    private void ResetPath()
    {
        if (path != null)
        {
            foreach (Node nodeObj in path)
            {
                originalBoard.boardViewArray[nodeObj.currentPosition.x, nodeObj.currentPosition.y].OnInteract();
            }
            path.Clear();
        }
    }

    private static Node FromBoard(NodeObject foundNode)
    {
        return originalBoard.GetNode(foundNode.boardCoordinate);
    }

    private static Node FromBoard(Piece foundNode)
    {
        return originalBoard.GetNode(foundNode.currentPosition);
    }


}