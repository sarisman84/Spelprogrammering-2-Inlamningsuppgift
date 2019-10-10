using UnityEngine;
using System;
public class PieceObject : MonoBehaviour
{

    //Variables that one, stores the position of an item in the array (as a reference, not actually IN the array) and two, hold some basic components.
    public Vector2Int boardCoordinate;
    SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Fake constructor that creates a view piece.
    /// </summary>
    /// <param name="prefab">A prefab reference.</param>
    /// <param name="worldPosition">Where the newrly created view piece will be spawned at.</param>
    /// <param name="team">Which team it lies in.</param>
    /// <param name="parent">A designated parent for sorting.</param>
    /// <param name="boardCoord">A reference to the globalPieceList item.</param>
    /// <returns>A view Piece.</returns>
    public static PieceObject CreatePieceObject(PieceObject prefab, Vector2 worldPosition, PieceColor team, Transform parent, Vector2Int boardCoord)
    {
        PieceObject newPiece = Instantiate(prefab, worldPosition, Quaternion.identity, parent);
        newPiece.SetTeamColor(team);
        newPiece.boardCoordinate = boardCoord;
        return newPiece;

    }

    /// <summary>
    /// Sets a team color depending on team.
    /// </summary>
    /// <param name="team">Current team.</param>
    public void SetTeamColor(PieceColor team)
    {
        switch (team)
        {
            case PieceColor.Red: _renderer.color = CustomColor("#800000"); break;

            case PieceColor.Orange: _renderer.color = CustomColor("#994d00"); break;

            case PieceColor.Yellow: _renderer.color = CustomColor("#b3b300"); break;

            case PieceColor.Green: _renderer.color = CustomColor("#00b300"); break;

            case PieceColor.Blue: _renderer.color = CustomColor("#000099"); break;

            case PieceColor.Magenta: _renderer.color = CustomColor("#660066"); break;

            case PieceColor.Unoccupied: _renderer.color = Color.gray; break;
        }
    }
    //A repeat of the same method from NodeObject.

    /// <summary>
    /// Custom method that converts hmtl color into unity color.
    /// </summary>
    /// <param name="v">Input.</param>
    /// <returns>A color from that html string.</returns>
    private Color CustomColor(string v)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(v, out myColor);
        return myColor;
    }
}