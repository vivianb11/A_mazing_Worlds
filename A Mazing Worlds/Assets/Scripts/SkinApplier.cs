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
        if (skinsCosmetics.random)
        {
            int random = Random.Range(0, skinsCosmetics.skins.Length);
            meshRenderer.material = skinsCosmetics.skins[random];

            GameManager.instance.onNextLevel.AddListener(ApplySkin);
        }

        ApplySkin();
    }

    public void ApplySkin()
    {
        if (!skinsCosmetics.random)
            return;

        int random = Random.Range(0, skinsCosmetics.skins.Length);
        meshRenderer.material = skinsCosmetics.skins[random];
    }
}
