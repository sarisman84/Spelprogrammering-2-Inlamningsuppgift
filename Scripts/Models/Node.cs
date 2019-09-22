using System;
using System.Collections.Generic;
using ChineseCheckers;
using UnityEngine;

[System.Serializable]
public class Node
{
    NodeObject viewReference;
    Vector2Int boardPosition;
    [SerializeField] Piece storedPiece;

    [SerializeField] Team currentTeam;

    public Team BelongsTo
    {
        get => currentTeam;
        set => currentTeam = value;
    }

    #region Accessors



    public Vector2Int CurrentPosition
    {
        get => boardPosition;
        set => boardPosition = value;
    }

    public Piece StoredPiece
    {
        get => storedPiece;
        set
        {
            storedPiece = value;

        }
    }
    public NodeObject Object
    {
        get => viewReference;
        set => viewReference = value;
    }

    

    #endregion

    #region Global Node Methods
    public Node(NodeObject prefab, string name, Vector2 position, int blueprint, Transform parent)
    {
        NodeObject newNode = MonoBehaviour.Instantiate(prefab, parent);
        newNode.Properties = this;
        newNode.SetColor = SetTeamColor(blueprint, newNode);
        newNode.Properties.BelongsTo = (blueprint > 14) ? Team.Unoccupied : (Team)blueprint;
        newNode.transform.position = position;
        newNode.name = name;
        viewReference = newNode;
    }

    public Node(Node origin, Piece piece)
    {

        CurrentPosition = origin.CurrentPosition;
        BelongsTo = origin.BelongsTo;
        StoredPiece = piece;
        if (StoredPiece != null)
            StoredPiece.NodeReference = origin;
    }

    /// <summary>
    /// Updates the color and data contained within the Node with the new data inputed.
    /// </summary>
    /// <param name="blueprint"> Used as an ID for setting up the color of the node. </param>
    /// <param name="newNode">The node in question. </param>
    public static Color SetTeamColor(int blueprint, NodeObject newNode)
    {
        switch (blueprint)
        {
            case 0:
                newNode.GetComponent<PolygonCollider2D>().enabled = false;
                return new Color();
            case 1:
            case (int)Team.Debug:
                return Color.white;

        }

        return TeamGenerator.SetColorBasedOnTeam((Team)blueprint);
    }

    /// <summary>
    /// Sets a direction that is offseted for the Hexagonical Grid.
    /// </summary>
    /// <param name="startPos">The current position to check if the row in question is odd or even</param>
    /// <param name="index">An index to loop through all directions</param>
    /// <returns>A direction based on index</returns>
    public static Vector2Int DirectionInBoard(Vector2Int startPos, int index)
    {
        Vector2Int pos = Vector2Int.zero;
        switch (index)
        {

            case 0:
                return new Vector2Int(pos.y, pos.x - 1); // Left
            case 3:
                return new Vector2Int(pos.y, pos.x + 1); // Right
            case 2:
                if (startPos.x % 2 == 0)
                    return new Vector2Int(pos.y + 1, pos.x);
                return new Vector2Int(pos.y + 1, pos.x + 1); //Top Right
            case 1:
                if (startPos.x % 2 != 0)
                    return new Vector2Int(pos.y + 1, pos.x);
                return new Vector2Int(pos.y + 1, pos.x - 1); //Top Left
            case 4:
                if (startPos.x % 2 != 0)
                    return new Vector2Int(pos.y - 1, pos.x + 1); //Bot Right
                return new Vector2Int(pos.y - 1, pos.x);
            case 5:
                if (startPos.x % 2 == 0)
                    return new Vector2Int(pos.y - 1, pos.x - 1); //Bot Left
                return new Vector2Int(pos.y - 1, pos.x);
        }
        return Vector2Int.zero;
    }

    #endregion

}