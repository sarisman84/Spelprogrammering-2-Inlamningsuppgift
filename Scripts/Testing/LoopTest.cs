using System;
using System.Collections;
using UnityEngine;

public class LoopTest : MonoBehaviour {

    string currentUser;
    string[] names = new string[] { "Jacob", "Joseph", "Spyros", "Elina", "John", "Jon", "Kai" };

    int i = 0;
    private void Awake () {

        StartCoroutine (TurnSystem ());
    }

    IEnumerator TurnSystem () {
        string tempName;
        while (true) {

            if (Input.GetKeyDown (KeyCode.Tab)) {
                i++;
                i = (names.Length == i) ? 0 : i;
            }

            tempName = names[i];
            Debug.Log (tempName);

            yield return null;
        }
    }

    /*
    Idea:

     We modify my valid moves method to also add a value based on the distance at the opponents base.

     Each node that has been found will have a value set to them as the program searches for any avaliable moves while on the computers turn.

     We then make the computer pick the highest/lowest pick.

     Is this really minimax though?..


     Update:

     Maybe we input the same list in multiple times?.. We just want to check how both the owner and the opponent would interact on the field.


     Or maybe we can store any neightbours into a node that we found (or into another class <- most likely the solution).

     So potential goal: create a class that is equal to the current node and that also stores any neighbours.
     */
}