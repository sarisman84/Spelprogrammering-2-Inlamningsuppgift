using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    

    Vector2Int boardPosition;
    [SerializeField] Piece storedPiece;

    Color defaultColor;
    SpriteRenderer _renderer;

    [SerializeField] Team currentTeam;

    public Team BelongsTo {
        get => currentTeam;
        set => currentTeam = value;
    }

    #region Accessors

    public Node[] GetNearestNodes {
        get {
            throw new NotImplementedException ();
        }
    }

    public Vector2Int CurrentBoardPosition {
        get => boardPosition;
        set => boardPosition = value;
    }

    public Piece StoredPiece {
        get => storedPiece;
        set {
            storedPiece = value;

        }
    }

    public Color SetColor {
        set {
            defaultColor = (defaultColor == new Color ()) ? value : defaultColor;
            _renderer = _renderer ?? GetComponent<SpriteRenderer> ();
            _renderer.color = value;
        }

    }
    #endregion

    private void OnEnable () {
        _renderer = GetComponent<SpriteRenderer> ();

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

    #region Global Node Methods

    /// <summary>
    /// Gets a piece (if it has any) and returns it.
    /// </summary>
    /// <param name="selectedNode">The node in question. </param>
    /// <param name="team">What team the one who selected it actually is. </param>
    /// <returns>A Node with a piece.</returns>

    /// <summary>
    /// Similar to Instantiate(), CreateNode creates a node and sets the appropiate variables to it at once.
    /// </summary>
    /// <param name="prefab">The prefab used to create a new Node.</param>
    /// <param name="blueprint">An ID system that sets the correct data to the Node.</param>
    /// <param name="parent">A parent that sorts the Nodes for convenience sake. </param>
    /// <returns></returns>
    public static Node CreateNode (Node prefab, int blueprint, Transform parent) {
        Node newNode = Instantiate (prefab, parent);
        SetTeamColor (blueprint, newNode);
        newNode.BelongsTo = (Team) blueprint;

        return newNode;
    }

    /// <summary>
    /// Updates the color and data contained within the Node with the new data inputed.
    /// </summary>
    /// <param name="blueprint"> Used as an ID for setting up the color of the node. </param>
    /// <param name="newNode">The node in question. </param>
    public static Color SetTeamColor (int blueprint, Node newNode) {
        Color newColor = new Color ();
        switch (blueprint) {

            case 2:
                //Set color to Red.
                newNode.SetColor = Color.red;
                newColor = Color.red;
                break;

            case 3:
                //Set color to Blue.
                newNode.SetColor = Color.blue;
                newColor = Color.blue;
                break;

            case 4:
                //Set color to Yellow.
                newNode.SetColor = Color.yellow;
                newColor = Color.yellow;
                break;

            case 5:
                //Set color to Green.
                newNode.SetColor = Color.green;
                newColor = Color.green;
                break;

            case 6:
                //set color to Magenta.
                newNode.SetColor = Color.magenta;
                newColor = Color.magenta;
                break;

            case 7:
                //Set color to Orange.
                newNode.SetColor = new Color (1, 0.6f, 0);
                newColor = new Color (1, 0.6f, 0);
                break;

            case 1:
                //Set color to White.
                newNode.SetColor = Color.white;
                newColor = Color.white;
                break;

            default:
                newNode.SetColor = new Color ();
                newColor = new Color ();
                newNode.GetComponent<PolygonCollider2D> ().enabled = false;
                break;
        }
        return newColor;
    }
    public static Color SetTeamColor (int blueprint) {
        Color newColor = new Color ();
        switch (blueprint) {

            case 2:
                //Set color to Red.

                newColor = Color.red;
                break;

            case 3:
                //Set color to Blue.

                newColor = Color.blue;
                break;

            case 4:
                //Set color to Yellow.

                newColor = Color.yellow;
                break;

            case 5:
                //Set color to Green.

                newColor = Color.green;
                break;

            case 6:
                //set color to Magenta.

                newColor = Color.magenta;
                break;

            case 7:
                //Set color to Orange.

                newColor = new Color (1, 0.6f, 0);
                break;

            case 1:
                //Set color to White.

                newColor = Color.white;
                break;

            default:

                newColor = new Color ();

                break;
        }
        return newColor;
    }

    static class Cache {
        public static Vector2Int[] directions;
        public static Node cachedNode;
    }
    public static Vector2Int DirectionToBoardCoordinate (Node go, int index) {
        if (Cache.cachedNode == null || Cache.cachedNode != go) {
            Cache.directions = new Vector2Int[] {
            new Vector2Int ((ChineseCheckers.BoardManager.board.GetLength (0) - 1 <= go.CurrentBoardPosition.x) ? 0 : 1, (go.CurrentBoardPosition.x % 2 == 2) ? -1 : 0), //Upper Right
            new Vector2Int ((ChineseCheckers.BoardManager.board.GetLength (0) - 1 <= go.CurrentBoardPosition.x) ? 0 : 1, (go.CurrentBoardPosition.x % 2 == 1) ? 1 : -1), //Upper Left
            new Vector2Int (0, (go.CurrentBoardPosition.x % 2 == 2) ? -1 : 1), //Middle Right
            new Vector2Int (0, (go.CurrentBoardPosition.x % 2 == 1) ? -1 : -1), //Middle Left
            new Vector2Int ((go.CurrentBoardPosition.x <= 0) ? 0 : -1, (go.CurrentBoardPosition.x % 2 == 2) ? -1 : 0), //Bottom Right
            new Vector2Int ((go.CurrentBoardPosition.x <= 0) ? 0 : -1, (go.CurrentBoardPosition.x % 2 == 1) ? 1 : -1), //Buttom Left
            };
            Cache.cachedNode = go;
        }
        if (index > Cache.directions.Length) return Vector2Int.zero;
        return Cache.directions[index];
    }

    public static Vector2Int DirectionInBoard (Vector2Int startPos, int index) {
        Vector2Int pos = Vector2Int.zero;
        switch (index) {

            case 0:
                return new Vector2Int (pos.y, pos.x - 1); // Left
            case 1:
                return new Vector2Int (pos.y, pos.x + 1); // Right
            case 2:
                if (startPos.x % 2 == 0)
                    return new Vector2Int (pos.y + 1, pos.x);
                return new Vector2Int (pos.y + 1, pos.x + 1); //Top Right
            case 3:
                if (startPos.x % 2 != 0)
                    return new Vector2Int (pos.y + 1, pos.x);
                return new Vector2Int (pos.y + 1, pos.x - 1); //Top Left
            case 4:
                if (startPos.x % 2 != 0)
                    return new Vector2Int (pos.y - 1, pos.x + 1); //Bot Right
                return new Vector2Int (pos.y - 1, pos.x);
            case 5:
                if (startPos.x % 2 == 0)
                    return new Vector2Int (pos.y - 1, pos.x - 1); //Bot Left
                return new Vector2Int (pos.y - 1, pos.x);
        }
        return Vector2Int.zero;
    }

    #endregion

}