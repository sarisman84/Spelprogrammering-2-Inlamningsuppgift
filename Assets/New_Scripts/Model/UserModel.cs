using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using static BoardModel;

public enum Team { None, Unoccupied, Red, Orange, Yellow, Green, Blue, Magenta, BigRed, BigGreen = 11 }
public abstract class UserModel : MonoBehaviour
{
    [SerializeField] protected Piece selectedPiece;
    [SerializeField] protected Node selectedNode;
    public Team currentTeam;
    public List<Piece> playerPieces;

    protected bool hasTurnEnded;


    public abstract void OnTurnTaken();


    protected List<Node> GetPath(Node source, List<Node> savedResults, bool searchForGeneralMoves, UserModel owner)
    {
        for (int curDir = 0; curDir < 6; curDir++)
        {
            if (owner.currentTeam != source.belongsTo) continue;
            Node potentialNode = GetValidMove(source, savedResults, curDir);
            if (potentialNode == null) continue;
            if (originalBoard.GetPiece(potentialNode.currentPosition) != null)
            {
                Node newSource = GetValidMove(potentialNode, savedResults, curDir);
                if (newSource == null) continue;
                if (originalBoard.GetPiece(newSource.currentPosition) == null)
                {
                    savedResults.Add(newSource);
                    originalBoard.boardViewArray[newSource.currentPosition.x, newSource.currentPosition.y].OnInteract("#ffcc99");
                    List<Node> newResults = GetPath(newSource, savedResults, false, owner);
                    savedResults.AddRange(newResults);

                }
                continue;
            }
            if (searchForGeneralMoves)
            {
                savedResults.Add(potentialNode);
                originalBoard.boardViewArray[potentialNode.currentPosition.x, potentialNode.currentPosition.y].OnInteract("#ffcc99");
            }


        }


        return savedResults;
    }




    Node GetValidMove(Node source, List<Node> savedResults, int index)
    {
        if (source == null || OutOfBounds(source.currentPosition)) return null;
        Vector2Int boardDir = source.currentPosition + BoardDirection(source.currentPosition, index);
        if (OutOfBounds(boardDir)) return null;
        Node foundNode = BoardModel.originalBoard.GetNode(boardDir);
        if (foundNode == source || foundNode.belongsTo == Team.None || savedResults.Any(p => p == foundNode)) return null;
        return foundNode;
    }

    private bool OutOfBounds(Vector2Int source)
    {
        return (source.x >= BoardModel.originalBoard.boardArray.GetLength(0) || source.x < 0 || source.y >= BoardModel.originalBoard.boardArray.GetLength(1) || source.y < 0);
    }



    Vector2Int BoardDirection(Vector2Int source, int index)
    {

        switch (index)
        {

            case 0:
                return new Vector2Int(0, -1);// Left

            case 1:
                return new Vector2Int(0, 1); // Right

            case 2:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, 0);
                return new Vector2Int(-1, 1); //Top Right
            case 3:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, -1);
                return new Vector2Int(-1, 0);//Top left



            case 4:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, 0);
                return new Vector2Int(1, 1); //Bottom Right
            case 5:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, -1);
                return new Vector2Int(1, 0);//Bottom Left
        }
        return Vector2Int.zero;
    }
}
