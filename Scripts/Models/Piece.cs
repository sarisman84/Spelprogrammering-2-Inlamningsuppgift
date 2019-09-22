﻿using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using UnityEngine;

[System.Serializable]
public class Piece
{

    Node currentNode;
    [SerializeField] Team currentTeam;

    public Vector2Int CurrentPosition
    {
        get => currentNode.CurrentPosition;
    }

    public Node NodeReference
    {
        set => currentNode = value;
        get => currentNode;
    }
    public Team BelongsTo
    {
        get => currentTeam;
        set => currentTeam = value;
    }

    PieceObject viewReference;
    public PieceObject Object
    {
        get => viewReference;
        set => viewReference = value;
    }

    #region Global Methods
    public static void MovePiece(Piece selectedPiece, Node current, Node destination)
    {
        //if (selectedPiece == null || current == null || destination == null) return;
        Debug.Log($"Moving {selectedPiece} from {current} to {destination}");
        current.StoredPiece = null;
        // Debug.Log (current.StoredPiece);
        // UnityEditor.EditorApplication.isPaused = true;
        selectedPiece.Object.transform.position = destination.Object.transform.position;
        destination.StoredPiece = selectedPiece;
        selectedPiece.NodeReference = destination;

    }

    public static void SimulateMovePiece(Node current, Node destination)
    {
        Debug.Log($"Current: {current}, Destination {destination}");
        foreach (Node node in BoardManager.board)
        {
            if (node == current || node == destination)
            {
                Debug.LogError($"Duplicate found");
            }
        }
        destination.StoredPiece = current.StoredPiece;
        current.StoredPiece = null;

    }

    public static void SimulateMovePiece(Board customBoard, Vector2Int startPos, Vector2Int endPos)
    {
        customBoard[endPos.x, endPos.y] = customBoard[startPos.x, startPos.y];
        customBoard[startPos.x, startPos.y] = null;
    }

    public static void SimulatingMovePiece(Piece selectedPiece, Node current, Node destination)
    {
        current.StoredPiece = null;
        //selectedPiece.Object.transform.position = destination.Object.transform.position;
        // Debug.Log (current.StoredPiece);
        // UnityEditor.EditorApplication.isPaused = true;
        //Debug.Log ($"Moving {selectedPiece} from {current} to {destination}");
        destination.StoredPiece = selectedPiece;
        selectedPiece.NodeReference = destination;
    }

    public Piece(PieceObject prefab, Color playerColor, Node node, Team team, Transform parent)
    {
        PieceObject newPiece = MonoBehaviour.Instantiate(prefab, node.Object.transform.position, Quaternion.identity, parent);
        newPiece.Properties = this;
        newPiece.SetPieceColor = playerColor;
        newPiece.Properties.BelongsTo = team;
        newPiece.Properties.NodeReference = node;
        viewReference = newPiece;

    }

    public Piece(Piece origin)
    {
        if (origin == null) return;
        currentTeam = origin.BelongsTo;

    }
    #endregion

}