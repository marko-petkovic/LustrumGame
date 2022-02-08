using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerScore
{
    public static int Score;
    public static int Time;
    public static int ammoAmount=90;
    public static int bulletsInClip=30;

    public static void SetDefaultValues()
    {
        Score = 0;
        ammoAmount = 90;
        bulletsInClip = 30;
    }
}
