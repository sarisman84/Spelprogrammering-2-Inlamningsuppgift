using System.Collections;
using System.Collections.Generic;
using ChineseCheckers;
using static ChineseCheckers.HexagonGrid;
using UnityEngine;

//Temporary script to test the rest of this project.
public class GameManager : MonoBehaviour
{
    const int O = 1;
    public Node nodePrefab;
    public Piece piecePrefab;

    public int amountOfPlayers;

    int[,] blueprint = new int[,] {
{0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 0, 4, 4, 0, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 0, 4, 4, 4, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 4, 4, 4, 4, 0, 0, 0, 0,0,0 },
{0, 6, 6, 6, 6, 1, 1, 1, 1, 1, 5, 5, 5,5,0 },
{0, 6, 6, 6, 1, 1, 1, 1, 1, 1, 5, 5, 5,0,0 },
{0, 0, 6, 6, 1, 1, 1, 1, 1, 1, 1, 5, 5,0,0 },
{0, 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 5, 0,0,0 },
{0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,0,0 },
{0, 0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0,0,0 },
{0, 0, 7, 7, 1, 1, 1, 1, 1, 1, 1, 3, 3,0,0 },
{0, 7, 7, 7, 1, 1, 1, 1, 1, 1, 3, 3, 3,0,0 },
{0, 7, 7, 7, 7, 1, 1, 1, 1, 1, 3, 3, 3,3,0 },
{0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,0,0 },
{0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,0,0 },

    };

    /*
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    
    
     */

    /*
    At the start of the game, create a board using an 2 Dimentional Array that holds a number.
    This number tells the nodes what state they should be!
    0 = Empty. (Collisions and Textures are turned off)
    1-7 = Board peaces (Have a dedicated color per number as well as sets the team to each of the nodes corresponding the color).
     */
    private void Awake()
    {

        BoardManager.board = CreateGrid(blueprint, nodePrefab);
        BoardManager.InsertPiecesToBoard(amountOfPlayers, piecePrefab);

    }

    //Blueprint for the int[,] that is used to shape the grid

    /*

    Star Shape
                           { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,0 }, 
                          { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0,0 }, 
                         { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,0 }, 
                        { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0,0 }, 
                       { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,1 }, 
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,0 }, 
                     { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,0 }, 
                    { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,0 }, 
                   { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,0 }, 
                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,0 }, 
                 { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,0 }, 
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,0 }, 
               { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,1 }, 
              { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0,0 }, 
             { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0,0 }, 
            { 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0,0 }, 
           { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,0 },
 

    Star Shade
 
                            { 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0,0 }, 
                          { 0, 0, 0, 0, 0, 4, 4, 0, 0, 0, 0, 0,0 }, 
                         { 0, 0, 0, 0, 0, 4, 4, 4, 0, 0, 0, 0,0 }, 
                        { 0, 0, 0, 0, 4, 4, 4, 4, 0, 0, 0, 0,0 }, 
                       { 6, 6, 6, 6, 1, 1, 1, 1, 1, 5, 5, 5,5 }, 
                      { 6, 6, 6, 1, 1, 1, 1, 1, 1, 5, 5, 5,0 }, 
                     { 0, 6, 6, 1, 1, 1, 1, 1, 1, 1, 5, 5,0 }, 
                    { 0, 6, 1, 1, 1, 1, 1, 1, 1, 1, 5, 0,0 }, 
                   { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,0 }, 
                  { 0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0,0 }, 
                 { 0, 7, 7, 1, 1, 1, 1, 1, 1, 1, 3, 3,0 }, 
                { 7, 7, 7, 1, 1, 1, 1, 1, 1, 3, 3, 3,0 }, 
               { 7, 7, 7, 7, 1, 1, 1, 1, 1, 3, 3, 3,3 }, 
              { 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0,0 }, 
             { 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0,0 }, 
            { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0,0 }, 
           { 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,0 },
    
     */
}