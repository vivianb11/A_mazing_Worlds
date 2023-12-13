using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Level Info")]
    [Min(1)]
    public int levelNumber;

    public string levelName;

    [Header("Level Carasteristics")]
    public bool timer;
    public float time;

    public Transform finish, start;

    [Header("Level Events")]
    public UnityEvent onLevelStart;
    public UnityEvent onLevelEnd;
    public UnityEvent onRespawn;

    private void Awake()
    {
        gameObject.tag = "Planet";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        onRespawn.Invoke();

        GameManager.instance.players[0].position = start.position;
        GameManager.instance.players[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}