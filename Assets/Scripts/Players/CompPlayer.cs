using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ChineseCheckers;
using TMPro;
using UnityEngine;

//Node: This script is not implemented yet. 

/// <summary>
/// The core class for computer based player behaivours.
/// </summary>
public class CompPlayer : MonoBehaviour, IPlayer
{

    #region Variables
    [SerializeField] Node[] cachedValidNodes;
    [SerializeField] TeamGenerator currentTeam;
    TeamGenerator opponent;

    [SerializeField] Node currentPiece, desiredNode;

    [SerializeField] Piece[] cachedPieces;
    bool hasJumped;

    #endregion

    #region  Properties

    public TeamGenerator CurrentOpponent
    {
        get => opponent;
        set => opponent = value;
    }

    public Node SelectedPiece
    {
        get
        {
            Node newPiece = null;
            if (cachedPieces == null)
            {
                Piece[] pieceArray = new Piece[currentTeam.TeamBase.Length];
                for (int i = 0; i < pieceArray.Length; i++)
                {
                    pieceArray[i] = currentTeam.TeamBase[i].StoredPiece;
                }
                cachedPieces = pieceArray;
            }
            int index = UnityEngine.Random.Range(0, cachedPieces.Length);
            if (index >= cachedPieces.Length) return null;
            newPiece = UserManager.AttemptToGetPiece(cachedPieces[index].CurrentlyLiesIn, currentTeam.Team, hasJumped, false, ref cachedValidNodes);
            if (cachedValidNodes.Length == 0)
            {
                return null;
            }
            opponent = opponent ?? UserManager.SetOpponent(this);
            if (text != null && opponent != null)
            {
                text.text = Value(newPiece.StoredPiece, opponent.TeamBase).ToString();
                text.color = TeamGenerator.SetColorBasedOnTeam(currentTeam.Team);
            }

            currentPiece = newPiece;
            return currentPiece;
        }

        set => currentPiece = value;
    }
    public Node DesiredTarget
    {
        get
        {
            int index = UnityEngine.Random.Range(0, cachedValidNodes.Length - 1);
            if (index >= cachedValidNodes.Length) return desiredNode;
            return desiredNode = (currentPiece != null) ? UserManager.AttemptToGetTarget(cachedValidNodes[index], ref cachedValidNodes) : desiredNode;
        }

        set => desiredNode = value;
    }
    public bool HasDoneFirstMove
    {
        get => hasJumped;
        set => hasJumped = value;
    }

    public Node DetectedNode =>
        throw new NotImplementedException();

    public Node[] CachedValidMoves
    {
        get => cachedValidNodes;
        set => cachedValidNodes = value;
    }

    public Piece[] CachedPieces
    {
        get => cachedPieces;
        set => cachedPieces = value;
    }

    public TeamGenerator CurrentTeam
    {
        get =>
            currentTeam;
        set =>
            currentTeam = value;
    }

    #endregion

    /// <summary>
    /// Create a player using the Interface IPlayer. Current method returns a ComputerPlayer.
    /// </summary>
    /// <param name="currentTeam"> What team this player will be in.</param>
    /// <param name="opponent">What opponent will this player face.</param>
    /// <returns>A newly created player. </returns>
    static TextMeshPro text;
    public TextMeshPro SetupText
    {
        get => text;
        set => text = value;
    }
    public static IPlayer CreatePlayer(Team team)
    {
        CompPlayer player = new GameObject($"Player {team} : Computer").AddComponent<CompPlayer>();
        Canvas parent = FindObjectOfType<Canvas>();
        player.SetupText = player.SetupText ?? new GameObject("Max Distance").AddComponent<TextMeshPro>();
        player.SetupText.transform.parent = parent.transform;
        return player;
    }

    private void Start()
    {
        StartCoroutine(DelayedUpdate());
    }
    IEnumerator DelayedUpdate()
    {
        while (true)
        {
            UserManager.OnActionTaken(this);
            yield return new WaitForSeconds(0.5f);
        }

    }

    float Value(Piece knownPiece, Node[] targets)
    {
        float maxDist = 0;
        foreach (Node target in targets)
        {
            float dist = Vector2.Distance(knownPiece.CurrentlyLiesIn.CurrentBoardPosition, target.CurrentBoardPosition);
            if (target.StoredPiece != null && target.StoredPiece.BelongsTo == currentTeam.Team) continue;
            maxDist += dist;
        }
        return maxDist;
    }

    void Minimax(Node[] knownValidPositions, int depth, bool maximizingPos)
    {

        if (depth == 0)
        {
            //Return current position at index.
        }
    }

    /*
        First, read all of the available pieces that you have that can move and store them somewhere.

        Second, pick a piece that is the best move possible.
        To do so, i need to define the following:
            1.What "position" is in the minimax method?
            2.What are the maximizingEval and minimizingEval?


        Third, apply same move methods as the human player.
        
        
     */
}