using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardUser {

    // Color playerColor { get; }

    Vector2Int MovePiece (Piece selectedPiece, Vector2 desiredPosition);
    void SaveGame ();
    void LoadGame ();

}