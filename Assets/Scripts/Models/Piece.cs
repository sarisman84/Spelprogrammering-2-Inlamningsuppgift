using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Piece {

    Node currentNode;
    [SerializeField] Team currentTeam;

    public Vector2Int CurrentPosition {
        get => currentNode.CurrentBoardPosition;
    }

    public Node NodeReference{
        set => currentNode = value;
        get => currentNode;
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
        selectedPiece.NodeReference = destination;

    }

    public static void SimulatingMovePiece (Piece selectedPiece, Node current, Node destination) {
        current.StoredPiece = null;
        //selectedPiece.Object.transform.position = destination.Object.transform.position;
        destination.StoredPiece = selectedPiece;
        selectedPiece.NodeReference = destination;
    }

    public Piece (PieceObject prefab, Color playerColor, Node node, Team team, Transform parent) {
        PieceObject newPiece = MonoBehaviour.Instantiate (prefab, node.Object.transform.position, Quaternion.identity, parent);
        newPiece.Properties = this;
        newPiece.SetPieceColor = playerColor;
        newPiece.Properties.BelongsTo = team;
        newPiece.Properties.NodeReference = node;
        viewReference = newPiece;

    }
    #endregion

}