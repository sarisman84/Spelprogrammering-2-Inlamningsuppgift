using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceOwner { Red, Blue, Orange, Yellow, Green, White }

public class Piece : MonoBehaviour {

    private Vector2 currentCoord;
    public Vector2 GetPiecePosition => currentCoord;
    private PieceOwner owner;
    public PieceOwner CurrentOwner { get => owner; set => owner = value; }
    new public GameObject gameObject => this.gameObject;

}