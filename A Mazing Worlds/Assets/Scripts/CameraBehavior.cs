using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private GameObject _target;
    [SerializeField] float cameraDistanceFromTarget = 22;

    private bool _gyroAvailable=false;

    private Camera _camera;
    private Quaternion _initialOrientation;

    private Quaternion rotation;
    private Gyroscope gyro;

    [SerializeField]
    private float gyroSensitivity = 1;
    [SerializeField]
    private  float rotationSpeed = 2;

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
        CameraDistance();
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
            _camera.transform.localPosition = new (0,0,Mathf.Lerp(_camera.transform.localPosition.z, Vector3.Distance(this.transform.position, _target.transform.position) + cameraDistanceFromTarget,0.1f));
    }

    void SetRotation()
    {
        this.transform.LookAt(_target.transform, Vector3.back);
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