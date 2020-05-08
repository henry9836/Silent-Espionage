using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatToTime
{
    public static string convertFloatToTime(float time)
    {
        string min = Mathf.FloorToInt((time / 60)).ToString("F0");

        string sec = Mathf.FloorToInt((time % 60)).ToString("F0");

        string ms = Mathf.FloorToInt((((time % 1) * 100))).ToString("F0");


        string timertxt = "";

        if (ms.Length < 1)
        {
            ms = "00";
        }
        if (ms.Length < 2)
        {
            ms = "0" + ms;
        }
        if (sec.Length < 1)
        {
            sec = "00";
        }
        if (sec.Length < 2)
        {
            sec = "0" + sec;
        }


        if (min != "0")
        {
            timertxt = min + ":" + sec + "." + ms;
        }
        else if (sec != "0")
        {
            timertxt = sec + "." + ms;
        }
        else
        {
            timertxt = ms;
        }

        return timertxt;
    }
}
