using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Temporary script to test the rest of this project.
public class GameManager : MonoBehaviour {

    public Piece piecePrefab;

    [SerializeField] List<Node> listOfNodes = new List<Node> ();

    //At the start of the scene, create an X amount of Pieces, set their position to an X node and give them a default rotation, based on the amount of Nodes added into the listOfNodes from the Inspector
    private void Awake () {

        for (int i = 0; i < listOfNodes.Count; i++) {
            Piece newPiece = Instantiate (piecePrefab, listOfNodes[i].transform.position, Quaternion.identity) as Piece;
            newPiece.SetPlayerColor (Color.red);
            listOfNodes[i].StoredPiece = newPiece;

        }
    }
}