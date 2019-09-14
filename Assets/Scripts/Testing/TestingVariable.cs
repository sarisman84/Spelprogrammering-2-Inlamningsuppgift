using System;
using UnityEngine;
public class TestingVariable : MonoBehaviour
{
    [SerializeField] Vector2Int boardPosition;
    public Vector2Int BoardPosition
    {
        get => boardPosition;
        set => boardPosition = value;
    }

    Renderer _renderer;
    Color defaultColor;



    public void Initialize()
    {
        _renderer = _renderer ?? GetComponent<Renderer>();
        defaultColor = _renderer.material.color;
    }
    public void HighlightObject(Color color)
    {
        _renderer.material.color = color;
    }
    public void ResetHighlight()
    {
        _renderer.material.color = defaultColor;
    }
}
