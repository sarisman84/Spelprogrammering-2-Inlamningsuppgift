using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TestBoardModel;

public class HumanPlayer : UserModel {
   

    RaycastHit2D detection;
    Camera cam;

    private void OnEnable () {
        cam = Camera.main;
    }

    //Some mainline variables used to store information depending on player action.
    public Vector2Int currentPiece;
    NodeObject storedObject;
    Vector2Int target;
    bool hasntDoneFirstMove = true;
   

    //Used as a way to end and start this player's turn.
    bool isReady = false;

    //Obligatory Raycast Update loop.
    private void Update () {
        if (!isReady) return;
        if (Input.GetKeyDown (KeyCode.Tab) && hasntDoneFirstMove) EndTurn ();
        if (!Input.GetMouseButtonDown (0)) return;
        Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
        detection = Physics2D.Raycast (mousePos, cam.transform.forward);
        //Since i cant really think of a better way (and due to time constraints), i am using a view element as a reference to find something on the array.
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

    
    //This method handles the interaction between the raycast information and the piece search and movement logic.
    void OnInteraction (NodeObject node) {
        #region Reset
        Reset ();
        #endregion

        if (node == null) return;
        //If the node is confirmed to be a target, move the stored piece to it.
        if (ConfirmTarget (node, results)) {

            target = node.boardCoordinate;
            MovePiece (currentPiece, target, OwnedViewPieces, ref hasntDoneFirstMove);
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
            obj.OnInteract ("#004d99");
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
    bool ConfirmingPiece (NodeObject node, List<BoardPiece> pieceList) {
        return pieceList.Any (p => p.pos == node.boardCoordinate);
    }

    //Confirms if said target is inside a list of results.
    bool ConfirmTarget (NodeObject node, List<Vector2Int> result) {
        return result.Any (p => p == node.boardCoordinate);
    }

}