using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public List<Transform> players;

    public List<LevelManager> levels;

    public float currentLevel = 1;

    Canvas pauseMenu;

    enum GameState { Playing, Paused, GameOver, FreeCam, Win };
    GameState gameState = GameState.Playing;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        instance = this;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        players = new();
        levels = new();
        SetPlayers();
    }

    private void Start()
    {
        levels = GameObject.FindGameObjectsWithTag("Planet").Select(x => x.GetComponent<LevelManager>()).ToList();

        if (levels.Count == 0)
        {
            Debug.LogError("No levels found" + "Check the the Planet Tag Has been well put on the parent");
        }
        else
        {
            levels.Sort((x, y) => x.levelNumber.CompareTo(y.levelNumber));
            foreach (var level in levels)
            {
                if (level.levelNumber != 1)
                    DeActivatePlanet(level.gameObject);
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Playing)
            {
                gameState = GameState.Paused;
                Time.timeScale = 0;
            }
            else if (gameState == GameState.Paused)
            {
                gameState = GameState.Playing;
                Time.timeScale = 1;
            }
        }
    }

    private void OnGUI()
    {

        if (gameState == GameState.Playing)
        {
            if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 100), "Pause"))
            {
                gameState = GameState.Paused;
                Time.timeScale = 0;
            }
        }

        if (gameState == GameState.Paused)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Resume"))
            {
                gameState = GameState.Playing;
                Time.timeScale = 1;
            }
        }
        else if (gameState == GameState.GameOver)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Restart"))
            {
                gameState = GameState.Playing;
                Time.timeScale = 1;
            }
        }
        else if (gameState == GameState.Win)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Restart"))
            {
                gameState = GameState.Playing;
                Time.timeScale = 1;
            }
        }
        if (gameState == GameState.Paused || gameState == GameState.GameOver || gameState == GameState.Win)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 50, 100, 100), "Quit"))
            {
                Application.Quit();
            }
        }
        if (gameState == GameState.GameOver)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 100), "Game Over");
        }
        else if (gameState == GameState.Win)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 100), "You Win");
        }
        if (gameState == GameState.Paused)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 100), "Paused");
        }
        // button to reset the gyro's flat position
        if (gameState == GameState.Playing)
        {
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), "Reset Gyro"))
            {
                GameInput.instance.SetFlatGyroRotation();
            }
        }
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
