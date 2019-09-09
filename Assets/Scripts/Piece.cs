using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    Color defaultColor;
    SpriteRenderer _renderer;

    [SerializeField] Node.Team currentTeam;

    public Node.Team BelongsTo {
        get => currentTeam;
        set => currentTeam = value;
    }

    public Color SetPieceColor {
        set {
            _renderer = _renderer ?? GetComponent<SpriteRenderer> ();
            _renderer.color = value;
            defaultColor = value;
        }
    }

    public void HighlightPiece (Color highlighColor, bool isHighlighting) {
        switch (isHighlighting) {

            case true:
                _renderer.color = highlighColor;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }

    public static void MovePiece (Piece selectedPiece, Node current, Node destination) {
        if (selectedPiece == null || current == null || destination == null) return;
        Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        selectedPiece.transform.position = destination.transform.position;
        destination.StoredPiece = selectedPiece;

    }

    static Transform parent;
    public static Piece CreatePiece (Piece prefab, Color playerColor, Node node, Node.Team team) {
        parent = parent ?? new GameObject ($"{prefab.name}'s list").transform;
        Piece newPiece = Instantiate (prefab, node.transform.position, Quaternion.identity, parent);
        newPiece.SetPieceColor = playerColor;
        newPiece.BelongsTo = team;
        return newPiece;
    }

}