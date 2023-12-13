using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public List<Transform> players;

    public List<LevelManager> levels;

    public Camera mainCamera;
    public CameraFolow cameraFolow;

    public int currentLevel = 1;

    Canvas pauseMenu;

    enum GameState { Playing, Paused, GameOver, FreeCam, Win };
    GameState gameState = GameState.Playing;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);
        else
            instance = this;

        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        players = new();
        levels = new();
        SetPlayers();

        mainCamera = Camera.main;
        cameraFolow = mainCamera.GetComponent<CameraFolow>();
    }

    private void Start()
    {
        levels = GameObject.FindGameObjectsWithTag("Planet").Select(x => x.GetComponent<LevelManager>()).ToList();

        Debug.Assert(levels.Count == 0, "No levels found" + "Check the the Planet Tag Has been well put on the parent");
        
        levels.Sort((x, y) => x.levelNumber.CompareTo(y.levelNumber));

        foreach (var level in levels)
        {
            if (level.levelNumber != 1)
                DeActivatePlanet(level.gameObject);
        }

        GameInput.instance.SetFlatGyroRotation();
    }

    private void Update()
    {

    }

    private void DeActivatePlanet(GameObject level)
    {
        level.transform.GetChild(0).gameObject.SetActive(false);
        level.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void ActivatePlanet(GameObject level)
    {
        level.transform.GetChild(0).gameObject.SetActive(true);
        level.transform.GetChild(2).gameObject.SetActive(true);
    }

    [Button]
    public void Respawn()
    {
        if(players.Count <= 0)
        {
            levels[currentLevel - 1].Respawn();
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (i > 1)
                {
                    Destroy(players[i].gameObject);
                }
                else
                    levels[currentLevel - 1].Respawn();

            }
        }
    }

    [Button]
    public void NextLevel()
    {
        currentLevel++;
    }

    private void SetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.transform).ToList();
    }

    public List<Transform> GetPlayers()
    {
        return players;
    }
}
