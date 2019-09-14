using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    [SerializeField] TestingVariable[,] board;
    [SerializeField] Vector2Int pos;
    [SerializeField] Color color;
    [SerializeField] VariablePosition test;
    Camera cam;
    RaycastHit hit;
    private void Awake()
    {
        board = board ?? Grid.CreateGrid(20, 20);
        cam = cam ?? Camera.main;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(board[pos.x, pos.y].transform.position, Vector3.one);
    }


    private void Update()
    {
        OnSelection(test);
    }

    private void OnSelection(VariablePosition test)
    {
        List<TestingVariable> variableList = new List<TestingVariable>();
        if (Input.GetMouseButton(0))
            variableList.Add(hit.collider.GetComponent<TestingVariable>());

        test.variables = variableList.ToArray();

        if (Input.GetMouseButtonDown(1))
        {

            test.child = new VariablePosition();
            OnSelection(test.child);
        }
    }

    private void FixedUpdate()
    {
        pos = RaycastToBoardPos();
    }

    private Vector2Int RaycastToBoardPos()
    {

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Physics.Raycast(mousePos - new Vector3(0, 0, 10), cam.transform.forward, out hit);
        if (hit.collider == null) return Vector2Int.zero;
        if (hit.collider.GetComponent<TestingVariable>())
            return hit.collider.GetComponent<TestingVariable>().BoardPosition;

        return Vector2Int.zero;
    }
}
