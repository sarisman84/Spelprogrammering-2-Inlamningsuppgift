using System;
using ChineseCheckers;
using UnityEngine;

public class HumanPlayer : MonoBehaviour, IPlayer {

    public LayerMask mask;
    [SerializeField] bool hasJumped = false;
    Vector3 mousePos;
    Camera cam;
    RaycastHit2D detectedObject;
    [SerializeField] Node currentNode, selectedNode;
    [SerializeField] Team currentTeam, opponent;
    [SerializeField] Node[] playerBase;
    Node[] cachedValidMoves;

    public Team BelongsTo { get => currentTeam; set => currentTeam = value; }
    public Team CurrentOpponent { get => opponent; set => opponent = value; }
    public Node[] PlayerBase { get => playerBase; set => playerBase = value; }
    public Node SelectedPiece {
        get {
            if (currentNode != null) currentNode.HighlightNode (new Color (), false);
            return currentNode = (cachedValidMoves == null) ? UserManager.AttemptToGetPiece (DetectedNode, currentTeam, hasJumped, ref cachedValidMoves) : currentNode;
        }
        set {
            if (currentNode != null) currentNode.HighlightNode (new Color (), false);
            currentNode = (cachedValidMoves == null) ? UserManager.AttemptToGetPiece (value, currentTeam, hasJumped, ref cachedValidMoves) : currentNode;
        }
    }
    public Node DesiredTarget {
        get {
            return selectedNode = (currentNode != null) ? UserManager.AttemptToGetTarget (DetectedNode, currentNode, ref cachedValidMoves) : selectedNode;
        }

        set {
            selectedNode = value;
        }
    }
    public bool HasDoneFirstMove {
        get => hasJumped;
        set => hasJumped = value;
    }
    public Node DetectedNode => (detectedObject.collider != null) ? detectedObject.collider.GetComponent<Node> () : null;
    public Node[] CachedValidMoves {
        get => cachedValidMoves;
        set {
            UserManager.ResetValidMoves (ref cachedValidMoves);
        }
    }

    public static IPlayer CreatePlayer (Team currentTeam, Team opponent) {
        IPlayer player = new GameObject ($"Player {currentTeam}: Human").AddComponent<HumanPlayer> ();
        player.BelongsTo = currentTeam;
        player.CurrentOpponent = opponent;
        return player;
    }

    private void OnEnable () {
        cam = Camera.main;
        mask = LayerMask.NameToLayer ("Node");
    }

    void Update () {
        #region Gameobject Detection
        //First, get the mouse position from screen coordinates to world coordinates.
        mousePos = cam.ScreenToWorldPoint (Input.mousePosition);

        //Then, set that position as the center of an Physics2D.OverlapCircle with a small radius. 
        //AGAIN: THIS IS TEMPRORARY AND IS MEANT TO BE REPLACED LATER!!!
        detectedObject = Physics2D.Raycast (mousePos - new Vector3 (0, 0, 10), cam.transform.forward, mask.value);
        Debug.DrawRay (mousePos, cam.transform.forward * 100f, Color.red);

        //Get an input from the left mouse button when pressed down.

        #endregion
        if (Input.GetKeyDown (KeyCode.Tab)) {
            UserManager.WhenTurnEnds (ref hasJumped, ref currentNode, ref selectedNode, ref cachedValidMoves);
        }
        if (!Input.GetMouseButtonDown (0)) return;

        if (DetectedNode == null) return;
        UserManager.OnActionTaken (this);

    }

    // private void OnDrawGizmos () {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere (detectedObject.point, 0.1f);
    //     Gizmos.color = Color.red;
    //     if (currentNode == null) return;
    //     Node[] moves = UserManager.cachedValidMoves;

    //     if (moves == null) return;
    //     foreach (var node in moves) {
    //         if (node != null)
    //             Gizmos.DrawWireSphere (node.transform.position, 0.5f);
    //     }
    //     Gizmos.color = Color.blue;
    //     if (selectedNode != null)
    //         Gizmos.DrawWireCube (selectedNode.transform.position, new Vector3 (0.5f, 0.5f, 0.5f));
    // }
}