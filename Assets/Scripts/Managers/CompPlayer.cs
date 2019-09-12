using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChineseCheckers;
using UnityEngine;

public class CompPlayer : MonoBehaviour, IPlayer {

    [SerializeField] Node[] playerBase, cachedValidNodes;
    [SerializeField] Team currentTeam, opponent;

    [SerializeField] Node currentPiece, desiredNode;

    [SerializeField] Piece[] cachedPieces;
    bool hasJumped;
    public Team BelongsTo {
        get => currentTeam;
        set => currentTeam = value;
    }
    public Team CurrentOpponent {
        get => opponent;
        set => opponent = value;
    }
    public Node[] PlayerBase {
        get => playerBase;
        set {
            Piece[] playerPieces = new Piece[value.Length];
            for (int i = 0; i < value.Length; i++) {
                playerPieces[i] = value[i].StoredPiece;
            }
            cachedPieces = cachedPieces ?? playerPieces;
            playerBase = value;
        }
    }
    public Node SelectedPiece {
        get => currentPiece;
        set => currentPiece = value;
    }
    public Node DesiredTarget {
        get => desiredNode;
        set => desiredNode = value;
    }
    public bool HasDoneFirstMove {
        get => hasJumped;
        set => hasJumped = value;
    }

    public Node DetectedNode =>
        throw new NotImplementedException ();

    public Node[] CachedValidMoves {
        get => cachedValidNodes;
        set => cachedValidNodes = value;
    }

    public Piece[] CachedPieces {
        get => cachedPieces;
        set => cachedPieces = value;
    }

    public static IPlayer CreatePlayer (Team currentTeam, Team opponent) {
        IPlayer player = new GameObject ($"Player {currentTeam} : Computer").AddComponent<CompPlayer> ();
        player.BelongsTo = currentTeam;
        player.CurrentOpponent = opponent;
        return player;
    }

    /*
        First, read all of the available pieces that you have that can move and store them somewhere.

        Second, pick the one that is going to be moved (minimax).

        Third, apply same move methods as the human player.
    
    
     */
}