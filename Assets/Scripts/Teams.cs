using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team { Empty, Unoccupied, Red, Yellow, Blue, Magenta, Orange, Green }

[System.Serializable]
public class TeamGenerator
{
    [SerializeField] Team currentTeam;
    [SerializeField] Node[] teamBase;
    public Team Team
    {
        get => currentTeam;
        set => currentTeam = value;
    }

    public static Color SetColorBasedOnTeam(Team team)
    {
        switch (team)
        {

            case Team.Red:
                return Color.red;

            case Team.Blue:
                return Color.blue;

            case Team.Yellow:
                return Color.yellow;

            case Team.Orange:
                return new Color(1, 0.6f, 0);

            case Team.Green:
                return Color.green;

            case Team.Magenta:
                return Color.magenta;

            default:
                return Color.black;
        }

        
    }

    public Node[] TeamBase
    {
        get => teamBase;
        set => teamBase = value;
    }

}