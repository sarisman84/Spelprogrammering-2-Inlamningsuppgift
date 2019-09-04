using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void CheckCondition(Node _node, Node go);
public class NodeManager
{
    public NodeManager()
    {

    }


    Node currentNode, selectedNode;
    LayerMask mask;

    //I need a method that checks if:
    /*
    1.X node has a StoredPiece that matches the type of "Team"
    2.If the selectedNode is not the same as the currentNode & if the currentNode is not empty
    4.Check if both selectedNode && currentNode are not empty
     */

    /// <summary>
    /// Gets any available pieces and stores them in a local variable.
    /// </summary>
    /// <param name="go"> The current node that is currently selected.</param>
    public void GetPiece(Node go)
    {
        if (go.StoredPiece != null)
            currentNode = currentNode ?? go;
        currentNode.StoredPiece.HighlightPiece(Color.green, true);
    }

    public void GetTargetPosition(Node go)
    {
        if (currentNode == null) return;
        //Check if the node found is inside an area.
        DetectAvailableNodesInArea(Color.cyan, go, ConditionMethod);

    }

    public void MovePiece()
    {
        if (currentNode == null || selectedNode == null) return;
        PieceManager.Do.MovePiece(currentNode.StoredPiece, currentNode, selectedNode);
        Reset();
    }

    void Reset()
    {
        selectedNode.StoredPiece.HighlightPiece(new Color(), false);
        ResetNodesInArea();
        currentNode = null;
        selectedNode = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(currentNode.transform.position, 2);
    }


    void DetectAvailableNodesInArea(Color highlightColor, Node go, CheckCondition callback)
    {
        Collider2D[] availableNodes = Physics2D.OverlapCircleAll(currentNode.transform.position, 2, LayerMask.NameToLayer("Node"));
        foreach (var node in availableNodes)
        {
            Node _node = node.GetComponent<Node>();
            _node.HighlightNode(highlightColor, true);
            callback(_node, go);

        }
    }

    void ResetNodesInArea()
    {
        Collider2D[] availableNodes = Physics2D.OverlapCircleAll(currentNode.transform.position, 2, LayerMask.NameToLayer("Node"));
        foreach (var node in availableNodes)
        {
            Node _node = node.GetComponent<Node>();
            _node.HighlightNode(new Color(), true);
        }
    }

    void ConditionMethod(Node _node, Node go)
    {
        if (_node == go)
        {
            if (go != currentNode) selectedNode = go;
        }
    }
}
