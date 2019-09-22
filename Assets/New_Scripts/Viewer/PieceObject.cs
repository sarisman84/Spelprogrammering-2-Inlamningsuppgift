using UnityEngine;
using System;
public class PieceObject : MonoBehaviour
{

    public Vector2Int boardCoordinate;
    SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public Vector2Int currentBoardPosition;
    public static PieceObject CreatePieceObject(PieceObject prefab, Vector2 worldPosition, PieceColor team, Transform parent, Vector2Int boardCoord)
    {
        PieceObject newPiece = Instantiate(prefab, worldPosition, Quaternion.identity, parent);
        newPiece.SetTeamColor(team);
        newPiece.boardCoordinate = boardCoord;
        return newPiece;

    }

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
        }
    }

    private Color CustomColor(string v)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(v, out myColor);
        return myColor;
    }
}