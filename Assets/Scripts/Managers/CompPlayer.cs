using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChineseCheckers;
using UnityEngine;

//Node: This script is not implemented yet. 

/// <summary>
/// The core class for computer based player behaivours.
/// </summary>
public class CompPlayer : MonoBehaviour, IPlayer
{
    
    #region Variables
    [SerializeField] Node[] playerBase, cachedValidNodes;
    [SerializeField] Team currentTeam, opponent;

    [SerializeField] Node currentPiece, desiredNode;

    [SerializeField] Piece[] cachedPieces;
    bool hasJumped;

    #endregion

    #region  Properties
    public Team BelongsTo
    {
        get => currentTeam;
        set => currentTeam = value;
    }
    public Team CurrentOpponent
    {
        get => opponent;
        set => opponent = value;
    }
    public Node[] PlayerBase
    {
        get => playerBase;
        set
        {
            Piece[] playerPieces = new Piece[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                playerPieces[i] = value[i].StoredPiece;
            }
            cachedPieces = cachedPieces ?? playerPieces;
            playerBase = value;
        }
    }
    public Node SelectedPiece
    {
        get => currentPiece;
        set => currentPiece = value;
    }
    public Node DesiredTarget
    {
        get => desiredNode;
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
    #endregion

    /// <summary>
    /// Create a player using the Interface IPlayer. Current method returns a ComputerPlayer.
    /// </summary>
    /// <param name="currentTeam"> What team this player will be in.</param>
    /// <param name="opponent">What opponent will this player face.</param>
    /// <returns>A newly created player. </returns>
    public static IPlayer CreatePlayer(Team currentTeam, Team opponent)
    {
        IPlayer player = new GameObject($"Player {currentTeam} : Computer").AddComponent<CompPlayer>();
        player.BelongsTo = currentTeam;
        player.CurrentOpponent = opponent;
        return player;
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