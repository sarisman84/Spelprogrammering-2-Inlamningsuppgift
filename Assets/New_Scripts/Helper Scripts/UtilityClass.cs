using UnityEngine;
using System;

public static class UtilityClass
{
    public static Color HTMLConversion(string v){
        Color desiredColor;
        ColorUtility.TryParseHtmlString(v, out desiredColor);
        return desiredColor;
    }
}
