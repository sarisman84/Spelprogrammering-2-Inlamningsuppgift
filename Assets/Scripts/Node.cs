using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] Piece storedPiece;

    public Piece StoredPiece {
        get => storedPiece;
        set {
            storedPiece = value;

        }
    }
    Color defaultColor;
    SpriteRenderer _renderer;
    private void OnEnable () {
        _renderer = GetComponent<SpriteRenderer> ();
        defaultColor = _renderer.color;
    }
    public void HighlightNode (Color highlight, bool isHighlighting) {
        switch (isHighlighting) {

            case true:
                _renderer.color = highlight;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }
}