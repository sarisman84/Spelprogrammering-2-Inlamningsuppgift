﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] Transform mouseObj;
    public LayerMask mask;

    Node currentNode, selectedNode;

    NodeManager _manager;

    public bool HasPickedUpAPiece
    {
        get => (currentNode.StoredPiece == null) ? false : true;
    }

    bool input;
    private void Awake()
    {
        mouseObj = mouseObj ?? new GameObject("Mouse").transform;
        _manager = new NodeManager();
    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseObj.position = mousePos;

        input = Input.GetMouseButtonDown(0);
        Collider2D go = Physics2D.OverlapCircle(mouseObj.transform.position, 0.1f, mask.value);
        //if(go.GetComponent<Piece>()) selectedPiece = go.GetComponnent<Piece>();
        if (go == null) return;
        if (!input) return;


        Node node = go.GetComponent<Node>();
        AttemptToMovePiece(node);

    }

    private void AttemptToMovePiece(Node node)
    {
        _manager.GetPiece(node);
        _manager.GetTargetPosition(node);
        _manager.MovePiece();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mouseObj.position, 0.1f);
    }
}