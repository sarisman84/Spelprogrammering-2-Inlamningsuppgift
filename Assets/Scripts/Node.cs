using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public enum Team { Empty, None, Red, Blue, Yellow, Green, Magenta, Orange }

    Team currentTeam;
    Vector2Int boardPosition;
    [SerializeField] Piece storedPiece;

    Color defaultColor;
    SpriteRenderer _renderer;

    #region Accessors
    public Team BelongsTo {
        get => currentTeam;
        set => currentTeam = value;
    }

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
    public static Node GetPiece (Node selectedNode, Team team) {
        return (selectedNode.StoredPiece != null && selectedNode.BelongsTo == team) ? selectedNode : null;
    }

    /// <summary>
    /// Similar to Instantiate(), CreateNode creates a node and sets the appropiate variables to it at once.
    /// </summary>
    /// <param name="prefab">The prefab used to create a new Node.</param>
    /// <param name="blueprint">An ID system that sets the correct data to the Node.</param>
    /// <param name="parent">A parent that sorts the Nodes for convenience sake. </param>
    /// <returns></returns>
    public static Node CreateNode (Node prefab, int blueprint, Transform parent) {
        Node newNode = Instantiate (prefab, parent);
        UpdateNode (blueprint, newNode);
        newNode.BelongsTo = (Node.Team) blueprint;

        return newNode;
    }

    /// <summary>
    /// Updates the color and data contained within the Node with the new data inputed.
    /// </summary>
    /// <param name="blueprint"> Used as an ID for setting up the color of the node. </param>
    /// <param name="newNode">The node in question. </param>
    private static void UpdateNode (int blueprint, Node newNode) {
        switch (blueprint) {

            case 2:
                //Set color to Red.
                newNode.SetColor = Color.red;
                break;

            case 3:
                //Set color to Blue.
                newNode.SetColor = Color.blue;
                break;

            case 4:
                //Set color to Yellow.
                newNode.SetColor = Color.yellow;
                break;

            case 5:
                //Set color to Green.
                newNode.SetColor = Color.green;
                break;

            case 6:
                //set color to Magenta.
                newNode.SetColor = Color.magenta;
                break;

            case 7:
                //Set color to Orange.
                newNode.SetColor = new Color (1, 0.6f, 0);
                break;

            case 1:
                //Set color to White.
                newNode.SetColor = Color.white;
                break;

            default:
                newNode.SetColor = new Color ();
                newNode.GetComponent<CircleCollider2D> ().enabled = false;
                break;
        }
    }

    #endregion

}