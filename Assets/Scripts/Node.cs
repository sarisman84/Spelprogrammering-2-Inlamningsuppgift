using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    //[SerializeField] List<Transform> pathReference = new List<Transform>();
    [SerializeField] Piece storedPiece;

    public Piece StoredPiece
    {
        get => storedPiece;
        set
        {
            storedPiece = value;

        }
    }
    Color defaultColor;
    SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
        defaultColor = _renderer.color;
    }
    public void HighlightNode(Color highlight, bool isHighlighting)
    {
        switch (isHighlighting)
        {

            case true:
                _renderer.color = highlight;
                break;

            case false:
                _renderer.color = defaultColor;
                break;
        }
    }

    Vector2 rot;

    int x, y;
    private void OnValidate()
    {

    }

    public Node[] CheckForNeighbores()
    {
        Node[] nodes = new Node[6];
        GetComponent<CircleCollider2D>().enabled = false;
        for (int i = 0; i < 6; i++)
        {
            SetDirection(i);
            Ray2D ray;
            ray = new Ray2D(transform.position, rot);
            //Debug.DrawRay(transform.position, new Vector3(rot.x, rot.y, 0), Color.blue, 1f);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                Debug.Log("Has found " + hit.collider.gameObject.name, hit.collider.gameObject);
                nodes[i] = hit.collider.GetComponent<Node>();
            }


        }
        GetComponent<CircleCollider2D>().enabled = true;
        return nodes;
    }

    private void SetDirection(int i)
    {
        //Debug.Log(i);
        switch (i)
        {
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
                rot = new Vector2(1, -1);

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
                rot = new Vector2(-1, 1);
                break;

            default:
                break;
        }
    }


}