using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team { Empty, Unoccupied, Red, Blue, Yellow, Green, Magenta, Orange }


/// <summary>
/// An interface that allows classes to use a Team enum system and a few other more common properties.
/// </summary>
public interface IPlayer {

    bool HasDoneFirstMove { get; set; }
    Node SelectedPiece { get; set; }
    Node DesiredTarget { get; set; }
    Node DetectedNode { get; }
    Node[] PlayerBase { get; set; }
    Node[] CachedValidMoves { get; set; }
    Team BelongsTo { get; set; }
    Team CurrentOpponent { get; set; }

}