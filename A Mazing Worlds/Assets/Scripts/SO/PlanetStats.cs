using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetStats", menuName = "ScriptableObjects/PlanetStats", order = 1)]
public class PlanetStats : ScriptableObject
{
    public static float BestTime;
    public static int numberOfTrys;

    public void SetBestTime(float time)
    {
        if (BestTime <= time)
            return;

        BestTime = time;
    }

    public void SetNumberOfTrys(int tries)
    {
        if (numberOfTrys <= tries)
            return;

        numberOfTrys = tries;
    }
}
