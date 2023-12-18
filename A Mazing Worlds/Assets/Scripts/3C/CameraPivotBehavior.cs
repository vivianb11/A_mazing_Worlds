using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;


public class CameraPivotBehavior : MonoBehaviour
{
    #region Variables
    public enum TargetBehavior { Average, Closest };

    private GameManager gameManager;

    private Transform _controlerPoint;

    [Header("Target Behavior")]
    [Space]
    [SerializeField] TargetBehavior targetBehavior = TargetBehavior.Closest;
    
    [Space]
    [SerializeField] Transform _target;

    [Space]
    [SerializeField] float cameraDistanceFromTarget = 22;

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
        _controlerPoint = this.transform.GetChild(0);

        SetClosestTarget();
    }

    void Update()
    {
        SetRotation();

        SetZoom();

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

    void SetZoom()
    {
        while (Input.touchCount == 2)
        {
            if (!pinchActive)
            {
                finger1StartPos = Input.GetTouch(0).position;
                finger2StartPos = Input.GetTouch(1).position;
                pinchActive = true;
            }
            else
            {
                float currentPinchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                float previousPinchDistance = Vector2.Distance(finger1StartPos, finger2StartPos);
                float difference = currentPinchDistance - previousPinchDistance;

                if (Mathf.Abs(difference) > pinchThreshold)
                {
                    pinchTransformAddition -= difference * pinchSpeed * Time.deltaTime;
                    pinchTransformAddition = Mathf.Clamp(pinchTransformAddition, minMaxZoom.x, minMaxZoom.y);
                }
            }
            break;
        }
        if (Input.touchCount < 2)
        {
            pinchActive = false;
        }
    }

    void CameraPlanetDistance()
    {
        if (Vector3.Distance(_controlerPoint.transform.position, _target.transform.position) != cameraDistanceFromTarget)
            _controlerPoint.localPosition = new(0, 0, Vector3.Distance(this.transform.position, _target.transform.position) + cameraDistanceFromTarget + pinchTransformAddition);
    }

    void SetRotation()
    {
        transform.LookAt(_target.position, transform.up);
    }

    public void SetCameraPivotPoint(Vector3 position)
    {
        this.transform.position = position;
    }
}
