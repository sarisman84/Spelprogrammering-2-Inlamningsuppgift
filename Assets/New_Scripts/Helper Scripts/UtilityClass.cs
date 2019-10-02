using UnityEngine;
using System;

public static class UtilityClass
{
    public static Color HTMLConversion(string v){
        Color desiredColor;
        ColorUtility.TryParseHtmlString(v, out desiredColor);
        return desiredColor;
    }


    public static T CorrelatedUser<T, U>(UserModel user, out ComputerPlayer comp) where T: HumanPlayer where U: ComputerPlayer{
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
}
