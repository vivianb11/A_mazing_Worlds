using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxPlayerSpeed;
    [SerializeField] float maxVelocity;

    [SerializeField] Transform movementOrientation;
    private Rigidbody rb;

    private bool airControl = true;

    void Awake()
    {
        if (!movementOrientation)
            movementOrientation = GameObject.FindGameObjectWithTag("ControlerPoint").transform;

        if (!movementOrientation)
            Debug.LogError("No movement orientation found" + "Please place the Camera Pivot Prefab");

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(airControl || rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
            Move();
        else if (!airControl && Physics.Raycast(transform.position, movementOrientation.forward, 1.1f))
            airControl = true;

        //if the player doubletaps within a sertain delay the screen, the flat gyro will be reset
        DoubleTap();
    }

    public void Move()
    {
        Vector2 direction = GameInput.instance.GetGyro();
        Vector2 movement = direction * maxPlayerSpeed;

        rb.AddForce(movementOrientation.right * movement.x);
        rb.AddForce(movementOrientation.up * movement.y);
    }

    public void DoubleTap()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float tapTime = 1f;
            while (tapTime > 0)
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    print("Double tap");
                    GameInput.instance.SetFlatGyroRotation();
                    break;
                }
                tapTime -= Time.fixedDeltaTime;
            }
        }
    }

    public void DesactivateAirControl()
    {
        StartCoroutine(AirControlDesactivator());
    }
    
    IEnumerator AirControlDesactivator()
    {
        yield return new WaitUntil(() => !Physics.Raycast(transform.position, movementOrientation.forward, 1.1f));

        airControl = true;
    }

    public bool GetAirControl()
    {
        return airControl;
    }
}