using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script acts as an Object which is represented in the game.
public class Node : MonoBehaviour {

    [SerializeField] Piece storedPiece;
    //This property uses Raycast to find the closest nodes in 6 different angles and returns them into a list.
    //NOTE: THIS PROPERTY AND ITS LOGIC WILL BE REPLACED SOON AS IT IS SUPER INEFFICIENT! THIS IS ONLY MEANT AS A TEMPORARY MEAN TO GET THE NODES!!!
    public Node[] GetNearestNodes {
        get {
            Node[] nodes = new Node[6];
            GetComponent<CircleCollider2D> ().enabled = false;
            for (int i = 0; i < 6; i++) {
                Ray2D ray;
                ray = new Ray2D (transform.position, SetDirection (i));
                //Debug.DrawRay(transform.position, new Vector3(rot.x, rot.y, 0), Color.blue, 1f);
                RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);
                if (hit.collider != null) {
                    Debug.Log ("Has found " + hit.collider.gameObject.name, hit.collider.gameObject);
                    nodes[i] = hit.collider.GetComponent<Node> ();
                }

            }
            GetComponent<CircleCollider2D> ().enabled = true;
            return nodes;
        }
    }
    //This property gives access to other scripts about whenever or not it holds a piece.
    public Piece StoredPiece {
        get => storedPiece;
        set {
            storedPiece = value;

        }
    }

    Color defaultColor;
    SpriteRenderer _renderer;

    //When this is enabled, Get a reference to its sprite renderer, then proceed to save its defaultColor for later uses.
    private void OnEnable () {
        _renderer = GetComponent<SpriteRenderer> ();
        defaultColor = _renderer.color;
    }
    //This method is used to change the color of the sprite by using a color property and a boolean to check if the user wants to highlight the object or reset it.
    public void HighlightNode (Color highlight, bool isHighlighting) {
        switch (isHighlighting) {

            case true:
                _renderer.color = highlight;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }

    //This method uses an index parameter and a switch in order to get 6 different angles depending on the current Index number.

    private Vector2 SetDirection (int i) {
        Vector2 rot = Vector2.zero;
        //Debug.Log(i);
        switch (i) {
            case 0:
                //Upper right
                rot = Vector2.one;
                break;

            case 1:
                //Middle right
                rot = Vector2.right;
                break;

            case 2:
                //Botton right
                rot = new Vector2 (1, -1);

                break;

            case 3:
                //Buttom left
                rot = -Vector2.one;
                break;
            case 4:
                //Middle left
                rot = Vector2.left;
                break;

            case 5:
                //Upper left
                rot = new Vector2 (-1, 1);
                break;

            default:
                break;
        }
        return rot;
    }

}