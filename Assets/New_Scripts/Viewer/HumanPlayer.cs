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

        Team opponent = GetOpponent(currentTeam);
        opponentsBase = new List<Vector2Int>();
        foreach (Node node in originalBoard.boardArray)
        {
            if (node.belongsTo == opponent)
                opponentsBase.Add(node.currentPosition);

        }

    }

    



    [SerializeField] Piece selectedPiece;
    [SerializeField] Node selectedNode;
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

                GetTarget(foundNode);
                GetSelectedPiece(foundNode);
                MovePiece();



            }


        }



    }

    private void MovePiece()
    {
        if (selectedPiece == null || selectedNode == null) return;

        Piece pieceToMove = selectedPiece;
        Node target = selectedNode;
        this.playerPieces.Remove(selectedPiece);
        PieceObject pieceViewToMove = originalBoard.GetVisualPiece(pieceToMove.currentPosition);
        selectedNode = null;
        selectedPiece = null;


        //Update pieceArray
        originalBoard.RemovePieceAt(pieceToMove.currentPosition);
        pieceToMove.currentPosition = target.currentPosition;
        originalBoard.InsertPieceAt(pieceToMove.currentPosition, pieceToMove);
        this.playerPieces.Add(pieceToMove);
        Debug.Log($"Selected Piece:{pieceToMove.currentPosition}|Selected Target:{target.currentPosition}");

        //Update Visuals
        originalBoard.RemovePieceViewAt(pieceViewToMove.currentBoardPosition);
        pieceViewToMove.currentBoardPosition = target.currentPosition;
        pieceViewToMove.transform.position = target.worldPosition;
        originalBoard.InsertPieceViewAt(pieceViewToMove.currentBoardPosition, pieceViewToMove);

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

        //find a piece within said node that is of the same team and store it.
        //Reset any previous paths recorded.
        ResetPath();

        selectedPiece = (selectedNode != null) ? selectedPiece : originalBoard.GetPiece(foundNode.boardCoordinate);
        if (selectedPiece == null) return;


        path = GetPath(BoardModel.originalBoard.GetNode(this.selectedPiece.currentPosition), new List<Node>(), true, this);





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
        if (foundNode == null) return null;
        return originalBoard.GetNode(foundNode.currentPosition);
    }


}