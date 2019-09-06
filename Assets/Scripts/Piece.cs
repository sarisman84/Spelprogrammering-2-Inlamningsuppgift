using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    Color defaultColor;
    SpriteRenderer _renderer;

    private void OnEnable () {
        _renderer = GetComponent<SpriteRenderer> ();
        defaultColor = _renderer.color;
    }
    public void SetPlayerColor (Color color) {

        _renderer.color = color;
        defaultColor = _renderer.color;
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
        //Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        selectedPiece.transform.position = destination.transform.position;
        destination.StoredPiece = selectedPiece;
    }

}