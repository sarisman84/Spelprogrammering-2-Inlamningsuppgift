using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    public Piece piecePrefab;
    static GameManager instance;
    public static GameManager _Manager
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>() ?? new GameObject("Game Manager").AddComponent<GameManager>();
            }
            return instance;
        }
    }


    [SerializeField] List<Piece> listOfPieces;
    [SerializeField] List<Node> listOfNodes = new List<Node>();

    private void Awake()
    {
        listOfPieces = new List<Piece>(listOfNodes.Count);
        for (int i = 0; i < listOfNodes.Count; i++)
        {
            Piece newPiece = Instantiate(piecePrefab, listOfNodes[i].transform.position, Quaternion.identity) as Piece;
            newPiece.SetPlayerColor(Color.red);
            listOfNodes[i].StoredPiece = newPiece;
            listOfPieces.Add(newPiece);
        }
    }
}
