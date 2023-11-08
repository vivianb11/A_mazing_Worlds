using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _gyroOrientation;
    private Vector3 _flatOrientation;

    public float gyroSensitivity = 1;
    public float rotationSpeed = 2;

    private void Start()
    {
        _camera = Camera.main;
        Input.gyro.enabled = true;
        _flatOrientation = Input.gyro.attitude.eulerAngles;
    }

    void Update()
    {
        _gyroOrientation = (Input.gyro.attitude.eulerAngles - _flatOrientation) * Time.deltaTime * rotationSpeed;

        if (_gyroOrientation.x > gyroSensitivity || _gyroOrientation.y > gyroSensitivity || _gyroOrientation.z > gyroSensitivity)
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + _gyroOrientation.x, this.transform.rotation.eulerAngles.y + _gyroOrientation.y,this.transform.rotation.eulerAngles.z + _gyroOrientation.z);
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

        }

        
    }
}
