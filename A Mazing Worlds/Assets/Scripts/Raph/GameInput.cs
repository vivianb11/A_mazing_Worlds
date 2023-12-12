using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance;

    private bool accelerationEnabled = true;

    private Vector2 flatGyro;

    [Range(0, 1)]
    [SerializeField] float minMaxXYaw, minMaxYPitch;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);
        else
            instance = this;

        Input.gyro.enabled = true;

        if (minMaxXYaw == 0 && minMaxYPitch == 0)
            Debug.LogWarning("minMaxXYaw and minMaxYPitch are both set to 0, this means that the gyro will not be clamped" + "If you want to change those values go to the GameManager/GameInput Script");
    }

    public Vector2 GetGyro()
    {
        if (!accelerationEnabled)
            return Vector2.zero;

        Vector2 unclaptedDirection = GetGyroRaw();

        if (minMaxXYaw == 0 && minMaxYPitch == 0)
            return unclaptedDirection;
        
        Vector2 claptedDirection = new Vector2(Mathf.Clamp(unclaptedDirection.x, -minMaxXYaw, minMaxXYaw), Mathf.Clamp(unclaptedDirection.y, -minMaxYPitch, minMaxYPitch));

        return claptedDirection;
    }

    public Vector2 GetGyroRaw()
    {
        if (!accelerationEnabled)
            return Vector2.zero;

        Vector2 unclaptedDirection = new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y) - new Vector2(flatGyro.x, flatGyro.y) + new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        return unclaptedDirection;
    }

    public void SetFlatGyroRotation()
    {
        flatGyro = new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0);
    }

    public void DisableAccelerator()
    {
        accelerationEnabled = false;
    }

    public void EnableAccelerator()
    {
        SetFlatGyroRotation();
        accelerationEnabled = true;
    }
}
