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

    }

    public void Move()
    {
        Vector2 direction = GameInput.instance.GetGyroPercent();
        Vector2 movement = direction * maxPlayerSpeed;

        rb.AddForce(movementOrientation.right * movement.x);
        rb.AddForce(movementOrientation.up * movement.y);
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

    public void Impluls(float force)
    {
        rb.AddForce(rb.velocity.normalized * force, ForceMode.Impulse);
    }
}