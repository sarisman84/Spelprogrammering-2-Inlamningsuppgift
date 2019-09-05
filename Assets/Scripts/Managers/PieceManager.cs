using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles how a piece behaives. It currently holds a function that allows the piece to move from one place to another.
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
    /// <summary>
    /// Moves the current piece selected from one place to another.
    /// </summary>
    /// <param name="selectedPiece">The current piece that is going to be moved.</param>
    /// <param name="current">The current node that the piece lies in. </param>
    /// <param name="destination">The destination that the current piece will be moved to. </param>
    public void MovePiece (Piece selectedPiece, Node current, Node destination) {
        //Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        selectedPiece.transform.position = destination.transform.position;
        destination.StoredPiece = selectedPiece;
    }

}