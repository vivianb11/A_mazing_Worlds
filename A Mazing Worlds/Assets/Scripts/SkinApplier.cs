using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkinApplier : MonoBehaviour
{
    public SkinsCosmetics skinsCosmetics;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = this.transform.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (skinsCosmetics.GetRandom())
        {
            int random = Random.Range(0, skinsCosmetics.GetSkins().Length);
            meshRenderer.material = skinsCosmetics.GetSkins()[random];

            GameManager.instance.onNextLevel.AddListener(ApplySkin);
        }
        else
            meshRenderer.material = skinsCosmetics.GetSkin();
    }

    public void ApplySkin()
    {
        if (!skinsCosmetics.GetRandom())
            return;

        int random = Random.Range(0, skinsCosmetics.GetSkins().Length);
        meshRenderer.material = skinsCosmetics.GetSkins()[random];
    }
}
