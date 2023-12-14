using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetStats", menuName = "ScriptableObjects/PlanetStats", order = 1)]
public class PlanetStats : ScriptableObject
{
    public float BestTime;
    public int numberOfTrys;
}

[CreateAssetMenu(fileName = "SkinsCosmetics", menuName = "ScriptableObjects/SkinsCosmetics", order = 1)]
public class SkinsCosmetics : ScriptableObject
{
    public bool random;

    [NaughtyAttributes.ShowIf("random")]
    public Material[] skins;

    [NaughtyAttributes.HideIf("random")]
    public Material skin;
}
