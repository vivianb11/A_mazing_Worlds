using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    #region Variables
    public enum LevelType { CountDown, Timed, Endless };

    [Header("Level Info")]
    [Min(1)]
    public int levelNumber;

    public string levelName;

    public PlanetStats planetStats;

    public int tries;

    [Header("Level Carasteristics")]
    public LevelType levelType = LevelType.Endless;
    [ShowIf("levelType", LevelType.CountDown)]
    public float time;

    public Transform finish, start;

    [Foldout("Events")]
    public UnityEvent onLevelStart;
    [Foldout("Events")]
    public UnityEvent onLevelEnd;
    [Foldout("Events")]
    public UnityEvent onRespawn;
    #endregion

    private void Awake()
    {
        gameObject.tag = "Planet";

        if (!planetStats)
            planetStats = new();
    }

    public void LevelStart()
    {
        onLevelStart.Invoke();

        if (levelType == LevelType.CountDown)
            Invoke("LevelEnd", time);
        if (levelType == LevelType.Timed)
        {
            time = 0;
            
            InvokeRepeating("Time", 1, 1);

            tries = 0;
        }
    }

    public void Time()
    {
        time++;
    }

    public void LevelEnd()
    {
        if (levelType == LevelType.Timed)
        {
            CancelInvoke("Time");
            planetStats.SetBestTime(time);
        }

        planetStats.SetNumberOfTrys(tries);

        onLevelEnd.Invoke();
    }

    public void Respawn()
    {
        GameManager.instance.players[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameManager.instance.players[0].position = start.position;

        tries++;
        
        onRespawn.Invoke();
    }
}