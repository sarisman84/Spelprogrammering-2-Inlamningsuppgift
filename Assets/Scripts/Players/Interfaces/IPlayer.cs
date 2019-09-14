using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An interface that allows classes to use a Team enum system and a few other more common properties.
/// </summary>
public interface IPlayer {

    bool HasDoneFirstMove { get; set; }
    Node SelectedPiece { get; set; }
    Node DesiredTarget { get; set; }
    Node DetectedNode { get; }

    Node[] CachedValidMoves { get; set; }
    TeamGenerator CurrentTeam { get; set; }
    TeamGenerator CurrentOpponent { get; set; }

}