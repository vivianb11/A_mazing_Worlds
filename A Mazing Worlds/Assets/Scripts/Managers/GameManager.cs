using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public List<Transform> players;

    public List<LevelManager> levels;
    public List<SplineAnimate> splines;

    public Camera mainCamera;
    public CameraFolow cameraFolow;

    public GameObject cameraPivot;

    public int currentLevel = 1;

    public enum GameState { Playing, Paused, GameOver, FreeCam, Win };
    public GameState gameState = GameState.Playing;

    [Foldout("Events")]
    public UnityEvent onNextLevel, onFinishAllLevels;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);
        else
            instance = this;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        players = new();
        levels = new();
        SetPlayers();
        SetLevels();

        mainCamera = Camera.main;
        cameraFolow = mainCamera.GetComponent<CameraFolow>();

        if (levels.Count <= 0)
            Debug.LogWarning("No levels found" + "Check the the Planet Tag Has been well put on the parent");

        levels.Sort((x, y) => x.levelNumber.CompareTo(y.levelNumber));
        
        foreach (var level in levels)
        {
            if (!level.GetComponent<SplineAnimate>())
                level.gameObject.AddComponent<SplineAnimate>();

            splines.Add(level.GetComponent<SplineAnimate>());
        }

        for (int i = 0; i < levels.Count; i++)
        {
            LevelManager level = levels[i];
            SplineAnimate spline = splines[i];
            if (level.levelNumber != currentLevel)
            {
                DeActivatePlanet(level.gameObject); 
                spline.enabled = true;
            }
            else
                spline.enabled = false;
            //{
            //    ActivatePlanet(level.gameObject);
            //    cameraPivot.transform.position = level.transform.position;
            //    cameraFolow.SetCameraPosition(cameraFolow.target);
            //    cameraFolow.SetCameraRotation(cameraFolow.target);
            //    players[0].position = level.GetComponent<LevelManager>().start.position;
            //}
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
                    Destroy(players[i].gameObject);
                else
                    levels[currentLevel - 1].Respawn();
            }
        }
    }

    [Button]
    public void NextLevel()
    {
        if (currentLevel >= levels.Count)
        {
            Debug.Log("No more levels");
            onFinishAllLevels.Invoke();
            return;
        }

        GameObject actualLevel = levels[this.currentLevel - 1].gameObject;
        SplineAnimate actualSpline = splines[this.currentLevel - 1];
        
        currentLevel++;

        GameObject nextLevel = levels[this.currentLevel - 1].gameObject;
        
        SplineAnimate nextSpline = splines[this.currentLevel - 1];

        cameraFolow.enabled = false;

        onNextLevel.Invoke();

        nextSpline.enabled = false;
        cameraPivot.transform.position = nextLevel.transform.position;
        ActivatePlanet(nextLevel);
        
        Respawn();

        cameraFolow.enabled = true;
        cameraFolow.SetCameraPosition(cameraFolow.target);

        DeActivatePlanet(actualLevel);

        actualSpline.enabled = true;
    }

    private void SetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.transform).ToList();
    }

    private void SetLevels()
    {
        levels = GameObject.FindGameObjectsWithTag("Planet").Select(x => x.GetComponent<LevelManager>()).ToList();
    }

    public List<Transform> GetPlayers()
    {
        return players;
    }
}
