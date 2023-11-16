using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StormStudioUtils
{
    public static string GetValidParameterValue(string value)
    {
        if (value.Length > 90)
            return value.Substring(0, 90);
        return value;
    }
}
