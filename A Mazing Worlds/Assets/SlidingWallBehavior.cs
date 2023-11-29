using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingWallBehavior : MonoBehaviour
{
    // variables needed for the sliding wall depending on the direction of the orientation of the phone that we get from the player
    [SerializeField] float slidingWallSpeed;
    [SerializeField] float maxVelocity;
    [SerializeField] float minVelocity;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotates the up vector of the sliding wall to the vector form the center of the planet whitch is 2 parents above to this object
        transform.right = transform.position - transform.parent.parent.position;

        ApplyForce(playerMovement.GetMovementDirection());
    }

    //applyes a force to the sliding wall
    private void ApplyForce(Vector3 direction)
    {
        if (rb.velocity.sqrMagnitude < (maxVelocity * maxVelocity) && rb.velocity.sqrMagnitude > (minVelocity * minVelocity))
        {
            rb.AddForce(direction * slidingWallSpeed);
        }
    }
}
