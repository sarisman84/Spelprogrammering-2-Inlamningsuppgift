using System;
using UnityEngine;

public class NodeObject : MonoBehaviour
{

    Color defaultColor;
    [SerializeField] Node nodeProperties;

    public Node Properties
    {
        get => nodeProperties;
        set => nodeProperties = value;
    }

    SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public Color SetColor
    {
        set
        {
            defaultColor = (defaultColor == new Color()) ? value : defaultColor;
            _renderer = _renderer ?? GetComponent<SpriteRenderer>();
            _renderer.color = value;
        }
    }


    public void HighlightNode(Color highlight, bool isHighlighting)
    {
        switch (isHighlighting)
        {

            case true:
                _renderer.color = highlight;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }

    public void ResetColor()
    {
        HighlightNode(new Color(), false);
    }
}
