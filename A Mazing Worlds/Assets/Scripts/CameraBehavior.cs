using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class CameraBehavior : MonoBehaviour
{
    private GameManager gameManager;
    
    private Camera _camera;

    [Header("Gyro Parameters")][Space]
    [SerializeField] float gyroSensitivity = 1;
    Gyroscope gyro;
    bool _gyroAvailable = false;

    Quaternion _initialOrientation;
    Quaternion rotation;

    [Header("Target Behavior")][Space]
    [SerializeField] GameObject _target;
    
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

    private void Start()
    {
        if (gameManager == null) { gameManager = Transform.FindFirstObjectByType<GameManager>(); }
        _target = gameManager.players[0];
        _camera = Camera.main;

        GyroSetup();
        ResetInitialGyroRotation();

        ResetViewTarget();
    }

    void Update()
    {
        if (_gyroAvailable)
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
                    Debug.Log("Pinch In");
                }
                else
                {
                    // Pinch Out
                    Debug.Log("Pinch Out");
                }
            }

        }
        else
        {
            pinchActive= false;
        }

        if (pinchActive)
        CameraDistance();

        ResetViewTarget();
    }

    void GyroSetup()
    {
        _gyroAvailable = SystemInfo.supportsGyroscope;

        if (_gyroAvailable)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }

    void ResetInitialGyroRotation()
    {
        _initialOrientation = Input.gyro.attitude;
    }

    void ResetViewTarget ()
    {
        foreach (var player in gameManager.players)
        {
            if (Vector3.Distance(player.transform.position,this.transform.position) < Vector3.Distance(_target.transform.position, this.transform.position))
            {
                _target = player;
            }
        }
    }

    void CameraDistance()
    {
        if (Vector3.Distance(_camera.transform.position, _target.transform.position) != cameraDistanceFromTarget)
            _camera.transform.localPosition = new (0,0,Mathf.Lerp(_camera.transform.localPosition.z, Vector3.Distance(this.transform.position, _target.transform.position) + cameraDistanceFromTarget,translationSpeed * Time.deltaTime));
    }

    void SetRotation()
    {
        // Get the target direction
        Vector3 targetDirection = _target.transform.position - transform.position;

        // Create the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.back);

        // Smoothly interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetCameraPivotPoint(Vector3 position)
    {
        this.transform.position = position;
    }
}




/*
float dt = Time.deltaTime;
rotation = gyro.attitude;
rotation = new Quaternion(rotation.x - _initialOrientation.x, rotation.y - _initialOrientation.y, rotation.z - _initialOrientation.z, rotation.w - _initialOrientation.w);
//rotation = gyro.attitude * _initialOrientation;

//Quaternion newRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, rotation.eulerAngles.z);

Quaternion newRotation = new Quaternion(transform.rotation.x + (rotation.x * rotationSpeed * dt), 0, transform.rotation.z + (rotation.z * rotationSpeed * dt), rotation.w);
//transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * dt);
transform.rotation = newRotation;

*/