using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;


public class CameraBehavior : MonoBehaviour
{
    #region Variables
    public enum TargetBehavior { Average, Closest };

    private GameManager gameManager;

    private Camera _camera;

    [Header("Target Behavior")]
    [Space]
    [SerializeField] TargetBehavior targetBehavior = TargetBehavior.Closest;
    
    [Space]
    [SerializeField] Transform _target;

    [Space]
    [SerializeField] float rotationSpeed = 1;

    [Space]
    [SerializeField] float cameraDistanceFromTarget = 22;
    [SerializeField] float translationSpeed = 1;

    [Header("Touch Behavior")]
    public float pinchThreshold = 0.1f;
    bool pinchActive = false;
    Vector2 finger1StartPos;
    Vector2 finger2StartPos;
    public float pinchSpeed = 1f;
    public Vector2 minMaxZoom = new Vector2(-5, 5);
    private float pinchTransformAddition;
    #endregion

    private void Start()
    {
        if (gameManager == null) { gameManager = Transform.FindFirstObjectByType<GameManager>(); }
        _target = gameManager.players[0];
        _camera = Camera.main;

        SetClosestTarget();
    }

    void Update()
    {
        SetRotation();

        if (Input.touches.Length > 1)
        {
            if (!pinchActive)
            {
                finger1StartPos = Input.touches[0].position;
                finger2StartPos = Input.touches[1].position;
            }

            pinchActive = true;

            Vector2 finger1CurrentPos = Input.touches[0].position;
            Vector2 finger2CurrentPos = Input.touches[1].position;

            float startDistance = Vector2.Distance(finger1StartPos, finger2StartPos);
            float currentDistance = Vector2.Distance(finger1CurrentPos, finger2CurrentPos);

            float pinchDelta = currentDistance - startDistance;

            if (Mathf.Abs(pinchDelta) > pinchThreshold)
            {
                // Pinch detected
                if (pinchDelta > 0)
                {
                    // Pinch In
                    pinchTransformAddition -= pinchSpeed * Time.deltaTime;
                }
                else
                {
                    // Pinch Out
                    pinchTransformAddition += pinchSpeed * Time.deltaTime;
                }

                pinchTransformAddition = math.clamp(pinchTransformAddition, minMaxZoom.x, minMaxZoom.y);
            }

        }
        else
        {
            pinchActive = false;
        }

        //if (!pinchActive)
        CameraPlanetDistance();


        if (targetBehavior == TargetBehavior.Average)
            SetAverageTarget();
        else if (targetBehavior == TargetBehavior.Closest)
            SetClosestTarget();
    }

    //sets the target to the intermediate point between all players
    void SetAverageTarget()
    {
        Vector3 target = Vector3.zero;
        foreach (var player in gameManager.players)
        {
            target += player.transform.position;
        }
        target /= gameManager.players.Count;
        _target.position = target;
    }

    void SetClosestTarget()
    {
        foreach (var player in gameManager.players)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) < Vector3.Distance(_target.transform.position, this.transform.position))
            {
                _target = player;
            }
        }
    }

    void CameraPlanetDistance()
    {
        if (Vector3.Distance(_camera.transform.position, _target.transform.position) != cameraDistanceFromTarget)
            _camera.transform.localPosition = new(0, 0, Mathf.Lerp(_camera.transform.localPosition.z, Vector3.Distance(this.transform.position, _target.transform.position) + cameraDistanceFromTarget + pinchTransformAddition, translationSpeed * Time.deltaTime));
    }

    void SetRotation()
    {
        // Smoothly interpolate between the current rotation and the target rotation
        transform.LookAt(_target.position, transform.up);
    }

    public void SetCameraPivotPoint(Vector3 position)
    {
        this.transform.position = position;
    }
    //creates a false offset to the camera that is a child of this camera pivot to make it feel like it's rotating around the planet depending on the player's movement
    public void SetCameraOffset(Vector3 position)
    {
        this.transform.GetChild(0).transform.localPosition = position;
    }
}
