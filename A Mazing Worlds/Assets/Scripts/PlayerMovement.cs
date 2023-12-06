using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float maxVelocity;

    private Vector3 flatGyro;
    private Vector2 acceleration;

    [SerializeField] Transform movementOrientation;
    private Rigidbody rb;

    void Awake()
    {
        if (!movementOrientation)
            movementOrientation = Camera.main.transform;

        rb = GetComponent<Rigidbody>();

        // Enable the gyroscope
        Input.gyro.enabled = true;
    }

    private void Start()
    {
        SetFlatGyroRotation();
    }

    void FixedUpdate()
    {
        Move();

        //if the player doubletaps within a sertain delay the screen, the flat gyro will be reset
        DoubleTap();
        //SetFlatGyroRotation();
    }

    public void Move()
    {
        acceleration = new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y) - new Vector2(flatGyro.x, flatGyro.y);

        if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity) && acceleration.sqrMagnitude > Mathf.Pow(0.05f, 2))
        {
            rb.AddForce(movementOrientation.right * acceleration.x * playerSpeed);
            rb.AddForce(movementOrientation.up * acceleration.y * playerSpeed);
        }
    }

    public void DoubleTap()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float tapTime = 0.5f;
            while (tapTime > 0)
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    print("Double tap");
                    SetFlatGyroRotation();
                    break;
                }
                tapTime -= Time.deltaTime;
            }
        }
    }
    
    public void SetFlatGyroRotation()
    {
        flatGyro = new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0);
    }

    public Vector3 GetMovementDirection()
    {
        return new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0) - flatGyro;
    }
}




//using UnityEngine;
//using UnityEngine.Events;


//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField] float playerspeed;
//    [SerializeField] float maxVelocity;

//    public Vector3 flatGyro;
//    public Vector2 acceleration;

//    [SerializeField] Transform movementOrientation;
//    private Rigidbody rb;


//    // Start is called before the first frame update
//    void Awake()
//    {
//        if(!movementOrientation)
//            movementOrientation = Camera.main.transform; // if no movement orientation is set, use the camera's transform

//        rb = GetComponent<Rigidbody>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //if the the player is in the air verified by raycast, the player will not be able to move
//        if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
//            Move();

//        if (Input.touchCount > 3)
//        {
//            SetFlatGyroRotation();
//        }
//    }

//    public void Move()
//    {
//        //transform.position += Camera.main.transform.right * Input.acceleration.x * playerspeed * Time.deltaTime;
//        //transform.position += Camera.main.transform.up * Input.acceleration.y * playerspeed * Time.deltaTime;

//        acceleration = new Vector2(Input.acceleration.x, Input.acceleration.y) - new Vector2(flatGyro.x, flatGyro.y);

//        if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity) && acceleration.sqrMagnitude > Mathf.Pow(0.05f,2))
//        {
//            rb.AddForce(movementOrientation.right * acceleration.x * playerspeed);
//            rb.AddForce(movementOrientation.up * acceleration.y * playerspeed);
//        }
//    }

//    public void SetFlatGyroRotation()
//    {
//        Debug.Log("Flat gyro set");

//        flatGyro = Input.acceleration;
//    }

//    // public function that returns the direction of the phone with the flatGyro rotation applied
//    public Vector3 GetMovementDirection()
//    {
//        return new Vector3(Input.acceleration.x, Input.acceleration.y, 0) - flatGyro;
//    }

//}
