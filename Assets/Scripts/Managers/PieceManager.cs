using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager {
    #region Singleton Property
    private static PieceManager instance;
    private PieceManager () { }
    public static PieceManager Do {
        get {
            if (instance == null) instance = new PieceManager ();
            return instance;
        }
    }

    #endregion
    public void MovePiece (Piece selectedPiece, Node current, Node destination) {
        // Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        selectedPiece.transform.position = destination.transform.position;
        destination.StoredPiece = selectedPiece;
    }

    
}