using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Level Info")]
    [Min(1)]
    public int levelNumber;

    public string levelName;
    public string levelDescription;

    [Header("Level Carasteristics")]
    public bool timer;
    public float time;

    public Collider finish, start;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}