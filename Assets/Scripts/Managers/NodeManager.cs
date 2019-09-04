using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spyro;


public class NodeManager
{
    public NodeManager(LayerMask _mask)
    {
        mask = _mask;
    }


    Node currentNode, selectedNode;
    LayerMask mask;

    //I need methods that check if:
    /*
    1.X node has a StoredPiece that matches the type of "Team"
    2.If the selectedNode is not the same as the currentNode & if the currentNode is not empty
    4.Check if both selectedNode && currentNode are not empty
     */

    bool toggle;
    /// <summary>
    /// Gets any available pieces and stores them in a local variable.
    /// </summary>
    /// <param name="go"> The current node that is currently selected.</param>
    public void GetPiece(Node go)
    {
        // if (currentNode != null)
        // {
        //     currentNode.StoredPiece.HighlightPiece(new Color(), false);
        //     ResetNodesInArea();
        //     currentNode = null;
        //     return;
        // }
        if(currentNode != null){
            currentNode.StoredPiece.HighlightPiece(new Color(), false);
            ResetNodesInArea();
        }
        if (go.StoredPiece != null)
            currentNode = go;
        if (currentNode == null) return;
        currentNode.StoredPiece.HighlightPiece(Color.green, true);

    }

    public void GetTargetPosition(Node go)
    {
        if (currentNode == null) return;
        //Check if the node found is inside an area.
        DetectAvailableNodesInArea(Color.cyan, go);

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


    // public void AttemptingToMovePiece(Node go)
    // {

    //     //If the player hovers over their selected piece : set an enum to SelectPiece
    //     //If the player hovers over an empty node that is available and is also empty: seet an enum to CanMove
    //     //If the player uses input on an empty node while CanMove is active: Move the selected piece and reset the enum.
    // }


    void DetectAvailableNodesInArea(Color highlightColor, Node go)
    {
        // Collider2D[] availableNodes = 
        Node[] availableNodes = currentNode.CheckForNeighbores() ?? Utility.OverlapCircleAll<Node>(currentNode.transform.position, 1, mask.value);
        foreach (var node in availableNodes)
        {
            Debug.Log(currentNode.StoredPiece);
            if (node == null) continue;
            node.HighlightNode(highlightColor, true);
            if (go == node)
            {
                if (go != currentNode && go.StoredPiece == null)
                {
                    
                    selectedNode = go;
                    Debug.Log(selectedNode);
                }
            }
        }
    }



    void ResetNodesInArea()
    {
        Collider2D[] availableNodes = Physics2D.OverlapCircleAll(currentNode.transform.position, 1, mask.value);
        foreach (var node in availableNodes)
        {
            Node _node = node.GetComponent<Node>();
            _node.HighlightNode(new Color(), false);
        }
    }


}
