using UnityEngine;
using System;

public static class UtilityClass
{

    //These are methods that are being used all lot  and therefor saved in this UtilityClass.
    
    
    
    //A reference to CustomColor() from NodeObject and PieceObject.
    public static Color HTMLConversion(string v)
    {
        Color desiredColor;
        ColorUtility.TryParseHtmlString(v, out desiredColor);
        return desiredColor;
    }


    //A reference to UserModels CreatePlayer.
    public static T CorrelatedUser<T, U>(UserModel user, out ComputerPlayer comp) where T : HumanPlayer where U : ComputerPlayer
    {
        switch (user)
        {

            case HumanPlayer human:
                comp = null;
                return (T)human;

            case ComputerPlayer computer:
                comp = (U)computer;
                return null;
        }
        comp = null;
        return null;
    }


    public enum tColor { Red = 2, Orange, Yellow, Green, Blue, Magenta, White }


    //A reference to both NodeObject and PieceObjects GetTeamColor().
    public static Color GetColor (tColor team )
    {
        switch (team)
        {
            case tColor.Red: return HTMLConversion("#800000");

            case tColor.Orange: return  HTMLConversion("#994d00"); 

            case tColor.Yellow: return  HTMLConversion("#b3b300"); 

            case tColor.Green: return  HTMLConversion("#00b300"); 

            case tColor.Blue: return  HTMLConversion("#000099"); 

            case tColor.Magenta: return  HTMLConversion("#660066"); 

            case tColor.White: return  Color.gray; 
        }
        return new Color();
    }
}
