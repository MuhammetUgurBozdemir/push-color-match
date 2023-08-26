using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string CheckColor(this Color32 color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }
}
