using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spyro;
using UnityEngine;

//This script handles how a user can find a piece, a destination and the proceed to move them using the PieceManager
//This is a temporary script as this will be massively reworked at a later date.
public class NodeManager {
    public NodeManager (LayerMask _mask) {
        mask = _mask;
    }

    //currentNode stores the chosen node by the player that holds a piece.
    //selectedNode stores the destination node chosen by the player.
    Node currentNode, selectedNode;
    LayerMask mask;

    /// <summary>
    /// Gets any available pieces and stores them in a local variable.
    /// </summary>
    /// <param name="go"> The current node that is currently selected.</param>
    public void GetPiece (Node go) {

        if (currentNode != null) {
            currentNode.StoredPiece.HighlightPiece (new Color (), false);
            ResetNodesInArea ();
        }
        if (go.StoredPiece != null)
            currentNode = go;
        if (currentNode == null) return;
        currentNode.StoredPiece.HighlightPiece (Color.green, true);

    }
    /// <summary>
    /// Gets any available targets and stores them in a local variable.
    /// </summary>
    /// <param name="go">The current node that is currently selected.</param>
    public void GetTargetPosition (Node go) {
        if (currentNode == null) return;
        //Check if the node found is inside an area.
        DetectAvailableNodesInArea (Color.cyan, go);

    }

    //This method allows the user to finally move the piece from its currentNode to the destinationNode, then proceeds to Reset() everything.
    public void MovePiece () {
        if (currentNode == null || selectedNode == null) return;
        PieceManager.Do.MovePiece (currentNode.StoredPiece, currentNode, selectedNode);
        Reset ();
    }

    //Resets all variables used as well as reset their highlight colors.
    void Reset () {
        selectedNode.StoredPiece.HighlightPiece (new Color (), false);
        ResetNodesInArea ();
        currentNode = null;
        selectedNode = null;
    }

    //This list is used to store all nodes that are being highlighted in the DetectAvailableNodesInArea() method.
    List<Node> allSelectedNodes;

    //This method checks for the nearest nodes that are empty in the Node.StorePiece property then proceeds to highlight them.
    //If Node go and a node in the list of nearest nodes is matched, store go into selectedNode.
    void DetectAvailableNodesInArea (Color highlightColor, Node go) {
        allSelectedNodes = new List<Node> ();

        foreach (var node in currentNode.GetNearestNodes) {
            //Debug.Log (currentNode.StoredPiece);
            if (node == null) continue;
            allSelectedNodes.Add (node);
            node.HighlightNode (highlightColor, true);
            if (go == node) {
                if (go != currentNode && go.StoredPiece == null) {

                    selectedNode = go;
                    //Debug.Log (selectedNode);
                }
            }
        }
    }
    //Resets the highlightedArea from the DetectAvailableNodesInArea().
    void ResetNodesInArea () {
        if (allSelectedNodes == null || allSelectedNodes.Count <= 0) return;
        foreach (var node in allSelectedNodes) {
            Node _node = node.GetComponent<Node> ();
            _node.HighlightNode (new Color (), false);
        }
    }

}