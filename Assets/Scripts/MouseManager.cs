using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {
    [SerializeField] Transform mouseObj;
    public LayerMask mask;

    [SerializeField] Node currentNode, selectedNode;

    public bool HasPickedUpAPiece {
        get => (currentNode.StoredPiece == null) ? false : true;
    }

    bool input;
    private void Awake () {
        mouseObj = mouseObj ?? new GameObject ("Mouse").transform;
    }
    void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mouseObj.position = mousePos;

        input = Input.GetMouseButtonDown (0);
        Collider2D go = Physics2D.OverlapCircle (mouseObj.transform.position, 0.1f, mask.value);
        //if(go.GetComponent<Piece>()) selectedPiece = go.GetComponnent<Piece>();
        if (go == null) return;
        if (input)
            foreach (var component in go.GetComponents<Component> ()) {
                switch (component) {
                    case Node node:
                        if (node.StoredPiece != null)
                            currentNode = currentNode ?? node;;
                        if (node != currentNode && currentNode != null)
                            selectedNode = node;
                        if (selectedNode != null && currentNode != null) {
                            PieceManager.Methods.MovePiece (currentNode.StoredPiece, currentNode, selectedNode);

                            currentNode = null;
                            selectedNode = null;
                        }

                        return;

                }
            }
    }

    private void FixedUpdate () {

    }

    private void OnDrawGizmos () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (mouseObj.position, 0.1f);
    }
}