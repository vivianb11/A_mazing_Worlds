using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetStats", menuName = "ScriptableObjects/PlanetStats", order = 1)]
public class PlanetStats : ScriptableObject
{
    public float BestTime;
    public int numberOfTrys;
}
