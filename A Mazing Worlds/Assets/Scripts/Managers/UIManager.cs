using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
    UIManager instance;

    [Foldout("Screens")]
    public GameObject ingameUI, pauseUI, startUI, endUI;

    [Foldout("Updated Text")]
    public List<Text> levelName, levelTime, levelTries;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }

    private void Update()
    {
        foreach (var levelName in levelName)
            levelName.text = GameManager.instance.levels[GameManager.instance.currentLevel].levelName.ToString();
        foreach (var levelTime in levelTime)
            levelTime.text = GameManager.instance.levels[GameManager.instance.currentLevel].time.ToString();
        foreach (var levelTries in levelTries)
            levelTries.text = GameManager.instance.levels[GameManager.instance.currentLevel].tries.ToString();
    }
}
