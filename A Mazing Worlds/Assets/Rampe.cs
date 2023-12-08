using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rampe : MonoBehaviour
{
    public float force;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            other.GetComponent<Rigidbody>().AddForce(transform.right * force);
        }
    }
}
