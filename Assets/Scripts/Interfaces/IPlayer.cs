using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team { Empty, Unoccupied, Red, Blue, Yellow, Green, Magenta, Orange }
[SerializeField]
public interface IPlayer
{

    Team BelongsTo { get; set; }
    Team CurrentOpponent { get; set; }
    Node[] PlayerBase { get; set; }

    
}
