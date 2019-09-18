using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Piece {

    Node currentNode;
    [SerializeField] Team currentTeam;

    public Node CurrentlyLiesIn {
        get => currentNode;
        private set => currentNode = value;
    }
    public Team BelongsTo {
        get => currentTeam;
        set => currentTeam = value;
    }

    PieceObject viewReference;
    public PieceObject Object {
        get => viewReference;
    }

    #region Global Methods
    public static void MovePiece (Piece selectedPiece, Node current, Node destination) {
        //if (selectedPiece == null || current == null || destination == null) return;
        //Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        selectedPiece.Object.transform.position = destination.Object.transform.position;
        destination.StoredPiece = selectedPiece;
        selectedPiece.CurrentlyLiesIn = destination;

    }

    public static void SimulatingMovePiece (Piece selectedPiece, Node current, Node destination) {
        current.StoredPiece = null;
        //selectedPiece.Object.transform.position = destination.Object.transform.position;
        destination.StoredPiece = selectedPiece;
        selectedPiece.CurrentlyLiesIn = destination;
    }

    public Piece (PieceObject prefab, Color playerColor, Node node, Team team, Transform parent) {
        PieceObject newPiece = MonoBehaviour.Instantiate (prefab, node.Object.transform.position, Quaternion.identity, parent);
        newPiece.Properties = this;
        newPiece.SetPieceColor = playerColor;
        newPiece.Properties.BelongsTo = team;
        newPiece.Properties.CurrentlyLiesIn = node;
        viewReference = newPiece;

    }
    #endregion

}