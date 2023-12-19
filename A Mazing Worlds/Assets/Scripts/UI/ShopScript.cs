using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public SkinsCosmetics skinsCosmetics;

    public List<Transform> transforms;

    public List<GameObject> skins;

    [SerializeField] List<Material> materials;

    public int selected;

    private void Awake()
    {
        foreach (var skin in skins)
        {
            materials.Add(skin.GetComponent<MeshRenderer>().material);
        }
    }

    private void Update()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            if (i == selected)
            {
                skins[i].SetActive(true);
                skins[i].transform.position = transforms[1].position;
            }
            else if (i == selected - 1)
            {
                skins[i].SetActive(true);
                skins[i].transform.position = transforms[0].position;
            }
            else if (i == selected + 1)
            {
                skins[i].SetActive(true);
                skins[i].transform.position = transforms[2].position;
            }
            else
                skins[i].SetActive(false);
        }
    }

    public void PrevSkin()
    {
        if (selected <= skins.Count-2)
            selected++;
    }

    public void NextSkin()
    {
        if (selected >= 1)
            selected--;
    }

    public void ApplySkin()
    {
        // Sets the skin selected to the scriptable object SkinsCosmetics
        skinsCosmetics.SetSkin(new (materials[selected]));
    }
}

class SkinMat
{
    public Material skin;
    public bool owned;

    public SkinMat(Material skin)
    {
        this.owned = false;
        this.skin = skin;
    }
}
