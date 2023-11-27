    using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerspeed;
    [SerializeField] float maxVelocity;

    public Vector3 flatGyro;
    public Vector2 acceleration;

    [SerializeField] Transform movementOrientation;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        if(!movementOrientation)
            movementOrientation = Camera.main.transform; // if no movement orientation is set, use the camera's transform

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.touchCount > 3)
        {
            Debug.Log("Space pressed");
            SetFlatGyroRotation();
        }
    }

    public void Move()
    {
        //transform.position += Camera.main.transform.right * Input.acceleration.x * playerspeed * Time.deltaTime;
        //transform.position += Camera.main.transform.up * Input.acceleration.y * playerspeed * Time.deltaTime;

        acceleration = new Vector2(Input.acceleration.x, Input.acceleration.y) - new Vector2(flatGyro.x, flatGyro.y);

        if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity) && acceleration.sqrMagnitude > Mathf.Pow(0.05f,2))
        {
            rb.AddForce(movementOrientation.right * acceleration.x * playerspeed);
            rb.AddForce(movementOrientation.up * acceleration.y * playerspeed);
        }
    }

    public void SetFlatGyroRotation()
    {
        Debug.Log("Flat gyro set");

        flatGyro = Input.acceleration;
    }
}
