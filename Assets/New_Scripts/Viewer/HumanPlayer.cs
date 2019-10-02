using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TestBoardModel;

public class HumanPlayer : UserModel {
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

    private void OnEnable () {
        cam = Camera.main;
    }

    public Vector2Int currentPiece;
    NodeObject storedObject;
    Vector2Int target;
    bool hasntDoneFirstMove = true;
    List<TestNode> playerBase = new List<TestNode> ();
    public List<TestPiece> ownedPieces = new List<TestPiece> ();

    public List<PieceObject> visualOwnedPieces = new List<PieceObject> ();

    bool isReady = false;

    //Obligatory Raycast Update loop.
    private void Update () {
        if (!isReady) return;
        if (Input.GetKeyDown (KeyCode.Tab)) EndTurn ();
        if (!Input.GetMouseButtonDown (0)) return;
        Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
        detection = Physics2D.Raycast (mousePos, cam.transform.forward);
        if (detection.collider == null) return;
        OnInteraction (detection.collider.GetComponent<NodeObject> ());

    }

    public override void StartTurn () {
        opponent = opponent ?? (TestManager.ins.allPlayers.Count % 2 == 1) ? TestManager.ins.allPlayers[UnityEngine.Random.Range (1, TestManager.ins.allPlayers.Count)] : TestManager.ins.allPlayers.Find (p => p.currentTeam == GetOpponent (this));
        isReady = true;
    }

    public override void EndTurn () {

        isReady = false;
        Reset ();
        hasntDoneFirstMove = true;
        TestGameModel.PlayerDone ();
    }

    List<Vector2Int> results = new List<Vector2Int> ();

    public override List<TestPiece> OwnedPieces { get => ownedPieces; set => ownedPieces = value; }
    public override List<PieceObject> VisualOwnedPieces { get => visualOwnedPieces; set => visualOwnedPieces = value; }
    public override List<TestNode> PlayerBase {
        get =>
            playerBase;
        set =>
            playerBase = value;
    }
    public List<TestNode> targetBase = new List<TestNode> ();
    public override List<TestNode> TargetBase { get => targetBase; set => targetBase = value; }

    //This method handles the interaction between the raycast information and the piece search and movement logic.
    void OnInteraction (NodeObject node) {
        #region Reset
        Reset ();
        #endregion

        if (node == null) return;
        //If the node is confirmed to be a target, move the stored piece to it.
        if (ConfirmTarget (node, results)) {

            target = node.boardCoordinate;
            MovePiece (currentPiece, target, visualOwnedPieces, ref hasntDoneFirstMove);
            results.Clear ();

        }
        //If the piece is not confirmed to be a valid piece, return the method.
        if (!ConfirmingPiece (node, TestBoardModel.globalPieceList)) return;

        //This section checks for the current Piece. If the hasntDoneFirstMOve is equal to false, always return the target as its new piece. Otherwise, Confirm if the found node is indeed a piece.
        storedObject = node;
        currentPiece = (!hasntDoneFirstMove) ? target : (ConfirmingPiece (node, TestBoardModel.globalPieceList)) ? node.boardCoordinate : currentPiece;

        if (globalPieceList.Any (p => p.pos == currentPiece && p.belongsTo != currentTeam)) return;
        storedObject.OnInteract ("#00ff00");

        //If the hasntDoneFIrstMove is equal to false, use the target as a baseline for searching for any available paths. Otherwise, use the found node as a baseline for searching any available paths.
        if (!hasntDoneFirstMove) {
            results = PathOfMoves (target, new List<Vector2Int> (), hasntDoneFirstMove);
            HighlightSelection ();
            return;
        }
        results = PathOfMoves (node.boardCoordinate, new List<Vector2Int> (), hasntDoneFirstMove);
        HighlightSelection ();
    }

    private void Reset () {
        if (storedObject != null) {
            storedObject.OnInteract ();
        }

        if (results != null || results.Count != 0) {
            ResetSelection ();

        }
    }

    //Highlights the list of nodes with a color.
    private void HighlightSelection () {

        for (int i = 0; i < results.Count; i++) {
            NodeObject obj = globalNodeViewList.Find (p => p.boardCoordinate == results[i]);
            obj.OnInteract ("#00ffff");
        }
    }

    //Resets the lists highlight.
    private void ResetSelection () {
        for (int i = 0; i < results.Count; i++) {
            NodeObject obj = globalNodeViewList.Find (p => p.boardCoordinate == results[i]);
            obj.OnInteract ();
        }
    }

    //Confirms if said piece belongs inside said list.
    bool ConfirmingPiece (NodeObject node, List<TestPiece> pieceList) {
        return pieceList.Any (p => p.pos == node.boardCoordinate);
    }

    //Confirms if said target is inside a list of results.
    bool ConfirmTarget (NodeObject node, List<Vector2Int> result) {
        return result.Any (p => p == node.boardCoordinate);
    }

}