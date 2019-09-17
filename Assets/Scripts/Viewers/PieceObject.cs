using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PieceObject : MonoBehaviour
{
    [SerializeField] Color defaultColor;
    SpriteRenderer _renderer;
    [SerializeField] Piece pieceProperties;
    public Piece Properties
    {
        get => pieceProperties;
        set => pieceProperties = value;
    }
    public Color SetPieceColor
    {
        set
        {
            _renderer = _renderer ?? GetComponent<SpriteRenderer>();
            _renderer.color = value;
            defaultColor = value;
        }
    }

    public void HighlightPiece(Color highlighColor, bool isHighlighting)
    {
        switch (isHighlighting)
        {

            case true:
                _renderer.color = highlighColor;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }
}
