
using System;
using UnityEngine;
using TMPro;

public class NodeObject : MonoBehaviour
{
    public Vector2Int boardCoordinate;
    PolygonCollider2D col2D;
    SpriteRenderer _renderer;

    [SerializeField] public SpriteRenderer childRenderer;
    [SerializeField] public TMP_Text evenCounter;
    private void OnEnable()
    {
        col2D = GetComponent<PolygonCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        
        OnInteract();

    }

    private void Awake()
    {
        col2D = GetComponent<PolygonCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        OnInteract();
        float val = boardCoordinate.x % 2;
        evenCounter.text = $"{val}";
    }

    public static NodeObject CreateNodeObject(NodeObject prefab, Vector2 position, NodeColor team, Transform parent, Vector2Int boardCoord)
    {
        NodeObject newNode = Instantiate(prefab, position, Quaternion.identity, parent);
        newNode.SetTeamColor(team);
        newNode.boardCoordinate = boardCoord;
        newNode.childRenderer = newNode.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>();
        newNode.evenCounter = newNode.GetComponentInChildren<TMP_Text>();
        newNode.evenCounter.text = $"{newNode.boardCoordinate.x % 2}";
        return newNode;

    }

    public void OnInteract(string hexcode)
    {
        if (childRenderer == null) return;
        childRenderer.color = CustomColor(hexcode);
    }

    public void OnInteract()
    {
        if (childRenderer == null) return;
        childRenderer.color = new Color();
    }


    public void SetTeamColor(NodeColor team)
    {
        switch (team)
        {
            case NodeColor.Red:
                _renderer.color = Color.red;
                break;
            case NodeColor.RedFade:
                _renderer.color = CustomColor("#ff5050");
                break;
            case NodeColor.RedOrange:
                _renderer.color = CustomColor("#ff7d52");
                break;


            case NodeColor.Orange:
                _renderer.color = CustomColor("#ffbf00");
                break;

            case NodeColor.OrangeFade:
                _renderer.color = CustomColor("#ff9933");
                break;

            case NodeColor.OrangeYellow:
                _renderer.color = CustomColor("#ffb84d");
                break;


            case NodeColor.Yellow:
                _renderer.color = Color.yellow;
                break;
            case NodeColor.YellowFade:
                _renderer.color = CustomColor("#ffd633");
                break;

            case NodeColor.YellowGreen:
                _renderer.color = CustomColor("#ccff33");
                break;


            case NodeColor.Green:
                _renderer.color = Color.green;
                break;
            case NodeColor.GreenFade:
                _renderer.color = CustomColor("#b3ff66");
                break;

            case NodeColor.GreenBlue:
                _renderer.color = CustomColor("#00cc99");
                break;


            case NodeColor.Blue:
                _renderer.color = Color.blue;
                break;
            case NodeColor.BlueFade:
                _renderer.color = CustomColor("#0066ff");
                break;

            case NodeColor.BlueMagenta:
                _renderer.color = CustomColor("#cc33ff");
                break;



            case NodeColor.Magenta:
                _renderer.color = Color.magenta;
                break;
            case NodeColor.MagentaFade:
                _renderer.color = CustomColor("#ff66ff");
                break;

            case NodeColor.MagentaRed:
                _renderer.color = CustomColor("#ff5050");
                break;

            case NodeColor.Unoccupied:
                _renderer.color = Color.white;
                break;

            default:
                col2D.enabled = false;

                break;
        }
    }

    private Color CustomColor(string v)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(v, out myColor);
        return myColor;
    }
}

