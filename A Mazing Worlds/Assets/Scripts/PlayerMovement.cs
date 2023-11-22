    using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerspeed;
    [SerializeField] float maxVelocity;
    public Transform target;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        //transform.position += Camera.main.transform.right * Input.acceleration.x * playerspeed * Time.deltaTime;
        //transform.position += Camera.main.transform.up * Input.acceleration.y * playerspeed * Time.deltaTime;

        if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity))
        {
            rb.AddForce(Camera.main.transform.right * Input.acceleration.x * playerspeed);
            rb.AddForce(Camera.main.transform.up * Input.acceleration.y * playerspeed);
        }
    }
}
