using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinsCosmetics", menuName = "ScriptableObjects/SkinsCosmetics", order = 1)]
public class SkinsCosmetics : ScriptableObject
{
    public bool random;

    [NaughtyAttributes.ShowIf("random")]
    public Material[] skins;

    [NaughtyAttributes.HideIf("random")]
    public Material skin;
}
