using System;
using ChineseCheckers;
using UnityEngine;

//This class replaces the MouseManager class. 

/// <summary>
/// Official class for actual players in Chinese Checkers
/// </summary>
public class HumanPlayer : MonoBehaviour, IPlayer {

    //Basic variables that store nessesary information to define what a player is.
    #region Variables
    public LayerMask mask;
    [SerializeField] bool hasJumped = false;
    Vector3 mousePos;
    Camera cam;
    RaycastHit2D detectedObject;
    [SerializeField] Node currentNode, selectedNode;
    [SerializeField] TeamGenerator currentTeam;
    TeamGenerator opponent;
    Node[] cachedValidMoves;
    #endregion

    //Advanced Properties that return a variable based on calculations and calls from other scripts.
    #region Properties
    public TeamGenerator CurrentOpponent { get => opponent; set => opponent = value; }
    public TeamGenerator CurrentTeam {
        get =>
            currentTeam;
        set =>
            currentTeam = value;
    }

    public Node SelectedPiece {
        get {
            if (currentNode != null) currentNode.HighlightNode (new Color (), false);
            return currentNode = (cachedValidMoves == null) ? UserManager.AttemptToGetPiece (DetectedNode, currentTeam.Team, true, ref cachedValidMoves) : currentNode;
        }
        set {
            if (currentNode != null) currentNode.HighlightNode (new Color (), false);
            currentNode = (cachedValidMoves == null) ? UserManager.AttemptToGetPiece (value, currentTeam.Team, true, ref cachedValidMoves) : currentNode;
        }
    }
    public Node DesiredTarget {
        get {
            return selectedNode = (currentNode != null) ? UserManager.AttemptToGetTarget (DetectedNode, ref cachedValidMoves) : selectedNode;
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

    #endregion

    /// <summary>
    /// Creates a IPlayer instance of type Human.
    /// </summary>
    /// <param name="currentTeam">What team this player is in</param>
    /// <param name="opponent">What team its opponent is in</param>
    /// <returns>An IPlayer instance of type Human.</returns>
    public static IPlayer CreatePlayer (Team team) {
        HumanPlayer player = new GameObject ($"Player {team}: Human").AddComponent<HumanPlayer> ();
        player.CurrentTeam = UserManager.SetOpponent (player);
        return player;
    }

    //The rest of this script is taken straight from MouseManager, with a few changes to fit the new layout.
    private void OnEnable () {
        cam = Camera.main;
        mask = LayerMask.NameToLayer ("Node");
    }

    void Update () {
        #region Gameobject Detection
        //First, get the mouse position from screen coordinates to world coordinates.
        mousePos = cam.ScreenToWorldPoint (Input.mousePosition);

        //Second, do a Raycast that detects an detectedObject which will be used in the properties above for evaluation.
        detectedObject = Physics2D.Raycast (mousePos - new Vector3 (0, 0, 10), cam.transform.forward, mask.value);
        //Debug.DrawRay(mousePos, cam.transform.forward * 100f, Color.red);

        //Get an input from the left mouse button when pressed down.

        #endregion

        //A temporary button to end a player's turn.
        if (Input.GetKeyDown (KeyCode.Tab)) {
            UserManager.WhenTurnEnds (ref hasJumped, ref currentNode, ref selectedNode, ref cachedValidMoves);
        }

        if (!Input.GetMouseButtonDown (0)) return;

        if (DetectedNode == null) return;
        UserManager.OnActionTaken (this);

    }

}