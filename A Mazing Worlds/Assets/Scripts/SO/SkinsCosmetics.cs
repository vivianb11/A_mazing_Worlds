using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinsCosmetics", menuName = "ScriptableObjects/SkinsCosmetics", order = 1)]
public class SkinsCosmetics : ScriptableObject
{
    public static bool random;

    [NaughtyAttributes.ShowIf("random")]
    public static Material[] skins;

    [NaughtyAttributes.HideIf("random")]
    public static Material skin;

    public void SetSkin(Material material)
    {
        skin = material;
    }

    public void SetSkins(Material[] materials)
    {
        skins = materials;
    }

    public void SetRandom(bool value)
    {
        random = value;
    }

    public Material GetSkin() { return skin; }
    public Material[] GetSkins() { return skins; }
    public bool GetRandom() { return random; }
}
