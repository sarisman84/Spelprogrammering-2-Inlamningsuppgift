using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TestBoardModel;

public class HumanPlayer : UserModel
{
    #region OldCode
    // RaycastHit2D hit2D;
    // Camera cam;

    // bool isTurnEnabled = false;
    // private void OnEnable () {
    //     cam = Camera.main;
    // }

    // [SerializeField] Piece selectedPiece;
    // [SerializeField] Node selectedNode;

    // public void OnGameAwake () {
    //     throw new NotImplementedException ();
    // }
    // public void OnTurnTaken () {

    //     Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
    //     hit2D = Physics2D.Raycast (mousePos, cam.transform.forward);
    //     if (hit2D.collider == null) return;
    //     if (Input.GetMouseButtonDown (0)) {
    //         NodeObject foundNode = hit2D.collider.GetComponent<NodeObject> ();
    //         if (foundNode != null) {

    //             GetTarget (foundNode);
    //             GetSelectedPiece (foundNode);
    //             MovePiece (ref selectedPiece, ref selectedNode, ref path);

    //         }

    //     }
    //     if (Input.GetKeyDown (KeyCode.Tab)){
    //         EndTurn();
    //     }

    // }

    // private void Update () {
    //     if (isTurnEnabled)
    //         OnTurnTaken ();
    // }

    // public override void StartTurn () {
    //     isTurnEnabled = true;
    // }

    // public override void EndTurn () {
    //     isTurnEnabled = false;
    //     GameModel.PlayerDone();
    // }

    // private void GetTarget (NodeObject foundNode) {
    //     if (path == null) return;
    //     if (FromBoard (selectedPiece) == FromBoard (foundNode)) return;
    //     Node potentialTarget = FromBoard (foundNode);
    //     foreach (var node in path) {

    //         if (potentialTarget == node) {
    //             selectedNode = potentialTarget;
    //             return;
    //         }
    //     }
    // }

    // List<Node> path;
    // private void GetSelectedPiece (NodeObject foundNode) {

    //     //find a piece within said node that is of the same team and store it.
    //     //Reset any previous paths recorded.
    //     ResetPath (ref path);

    //     selectedPiece = (selectedNode != null) ? selectedPiece : originalBoard.GetPiece (foundNode.boardCoordinate);
    //     if (selectedPiece == null) return;

    //     path = GetPath (BoardModel.originalBoard.GetNode (this.selectedPiece.pos), new List<Node> (), true, true);

    // }

    // private static Node FromBoard (NodeObject foundNode) {
    //     return originalBoard.GetNode (foundNode.boardCoordinate);
    // }

    // private static Node FromBoard (Piece foundNode) {
    //     if (foundNode == null) return null;
    //     return originalBoard.GetNode (foundNode.pos);
    // }

    #endregion


    RaycastHit2D detection;
    Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
    }

    public Vector2Int currentPiece;
    NodeObject storedObject;
    Vector2Int target;
    bool hasntDoneFirstMove = true;
    public List<TestPiece> ownedPieces = new List<TestPiece>();

    public List<PieceObject> visualOwnedPieces = new List<PieceObject>();
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        detection = Physics2D.Raycast(mousePos, cam.transform.forward);
        if (detection.collider == null) return;
        OnInteraction(detection.collider.GetComponent<NodeObject>());

    }

    List<Vector2Int> results = new List<Vector2Int>();

    public override List<TestPiece> OwnedPieces { get => ownedPieces; set => ownedPieces = value; }
    public override List<PieceObject> VisualOwnedPieces { get => visualOwnedPieces; set => visualOwnedPieces = value; }

    void OnInteraction(NodeObject node)
    {
        #region Reset
        if (storedObject != null)
        {
            storedObject.OnInteract();
        }

        if (results != null || results.Count != 0)
        {
            ResetSelection();

        }
        #endregion


        if (node == null) return;
        if (ConfirmTarget(node, results))
        {

            target = node.boardCoordinate;
            MovePiece(currentPiece, target);
            results.Clear();
            //currentPiece = target;
        }
        if (!ConfirmingPiece(node, TestBoardModel.globalPieceList)) return;
        storedObject = node;
        currentPiece = (ConfirmingPiece(node, TestBoardModel.globalPieceList)) ? node.boardCoordinate : currentPiece;
        storedObject.OnInteract("#00ff00");
        results = PathOfMoves(node.boardCoordinate, new List<Vector2Int>(), hasntDoneFirstMove);
        HighlightSelection();
    }

    private void MovePiece(Vector2Int currentPiece, Vector2Int target)
    {
        int globalI = globalPieceList.FindIndex(0, p => p.pos == currentPiece);
        TestPiece piece = new TestPiece();

        int i = visualOwnedPieces.FindIndex(0, p => p.boardCoordinate == currentPiece);

        piece.pos = target;
        piece.worldPos = TestBoardModel.test_OriginalBoard[target.x, target.y].worldPos;

        visualOwnedPieces[i].transform.position = piece.worldPos;
        visualOwnedPieces[i].boardCoordinate = piece.pos;

        globalPieceList[globalI] = piece;


        currentPiece = target;
        hasntDoneFirstMove = false;

    }

    private void HighlightSelection()
    {

        for (int i = 0; i < results.Count; i++)
        {
            NodeObject obj = globalNodeViewList.Find(p => p.boardCoordinate == results[i]);
            obj.OnInteract("#00ffff");
        }
    }

    private void ResetSelection()
    {
        for (int i = 0; i < results.Count; i++)
        {
            NodeObject obj = globalNodeViewList.Find(p => p.boardCoordinate == results[i]);
            obj.OnInteract();
        }
    }


    bool ConfirmingPiece(NodeObject node, List<TestPiece> pieceList)
    {
        return pieceList.Any(p => p.pos == node.boardCoordinate);
    }

    bool ConfirmTarget(NodeObject node, List<Vector2Int> result)
    {
        return result.Any(p => p == node.boardCoordinate);
    }

}