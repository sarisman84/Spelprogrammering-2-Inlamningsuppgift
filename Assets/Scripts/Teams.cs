using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team { Empty, Unoccupied, Red, Yellow, Blue, Magenta, Orange, Green, BigRed, Debug, BigBlue, BigOrange, BigGreen, BigMagenta, BigYellow, BigRedToOrange, BigOrangeToGreen, BigGreenToBlue, BigBlueToYellow, BigYellowToMagenta, BigMagentaToRed }

[System.Serializable]
public class TeamGenerator {
    [SerializeField] Team currentTeam;
    [SerializeField] Node[] teamBase;
    public Team Team {
        get => currentTeam;
        set => currentTeam = value;
    }

    public static Color SetColorBasedOnTeam (Team team) {
        switch (team) {

            case Team.Red:
                return Color.red;

            case Team.Blue:
                return Color.blue;

            case Team.Yellow:
                return Color.yellow;

            case Team.Orange:
                return ConvertFromHexCode ("#ff6600");

            case Team.Green:
                return Color.green;

            case Team.Magenta:
                return Color.magenta;

            case Team.BigBlue:
                return ConvertFromHexCode ("#4d79ff");
            case Team.BigRed:
                return ConvertFromHexCode ("#ac3939");

            case Team.BigYellow:
                return ConvertFromHexCode ("#ffcc66");

            case Team.BigOrange:
                return ConvertFromHexCode ("#ff9966");

            case Team.BigGreen:
                return ConvertFromHexCode ("#66ff66");

            case Team.BigMagenta:
                return ConvertFromHexCode ("#ff66cc");

            case Team.BigRedToOrange:
                return ConvertFromHexCode ("#ff5c33");

            case Team.BigOrangeToGreen:
                return ConvertFromHexCode ("#ccff66");

            case Team.BigGreenToBlue:
                return ConvertFromHexCode ("#006699");

            case Team.BigBlueToYellow:
                return ConvertFromHexCode ("#80dfff");

            case Team.BigYellowToMagenta:
                return ConvertFromHexCode ("#ffcc99");

            case Team.BigMagentaToRed:
                return ConvertFromHexCode ("#ff5050");
            default:
                return Color.black;
        }

    }

    private static Color ConvertFromHexCode (string code) {
        Color newColorRed;
        ColorUtility.TryParseHtmlString (code, out newColorRed);
        return newColorRed;
    }

    public Node[] TeamBase {
        get => teamBase;
        set => teamBase = value;
    }

}