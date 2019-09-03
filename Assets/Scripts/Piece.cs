﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    Color defaultColor;
    SpriteRenderer _renderer;

    private void OnEnable () {
        _renderer = GetComponent<SpriteRenderer> ();
    }
    public void SetPlayerColor (Color color) {

        _renderer.color = color;
        defaultColor = _renderer.color;
    }

}