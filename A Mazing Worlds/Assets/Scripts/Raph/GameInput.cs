using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;

    private bool accelerationEnabled = true;

    private Vector2 flatGyro;

    private void Start()
    {
        instance ??= this;
    }

    public Vector2 GetGyro()
    {
        return accelerationEnabled ? new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y) - new Vector2(flatGyro.x, flatGyro.y) + new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) : new Vector2();
    }

    public void SetFlatGyroRotation()
    {
        flatGyro = new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0);
    }
}
