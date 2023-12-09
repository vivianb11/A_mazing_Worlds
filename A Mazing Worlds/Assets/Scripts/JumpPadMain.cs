using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class JumpPadMain : MonoBehaviour
{
    enum JumpPadMode { Simple, Controled, Charging};
    enum JumpPadState { Idle, Launching, Cooldown };

    [SerializeField] JumpPadMode jumpPadMode = JumpPadMode.Simple;

    [SerializeField] float jumpForce = 100;

    public Transform jumpTarget;

    [SerializeField] float cooldown = 1;
    bool playerLaunched = false;

    // different states of the jump pad
    JumpPadState jumpPadState = JumpPadState.Idle;

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        // if the player collides with the object it will launch the player with a addforce and call the event
        if (collision.gameObject.tag == "Player" && !playerLaunched)
        {
            print("Player launched");
            playerLaunched = true;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
            StartCoroutine(ResetLaunch());
        }
    }

    void OnDrawGizmos()
    {
        if(jumpPadMode == JumpPadMode.Simple)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up.normalized * jumpForce);
        }
        else if(jumpPadMode == JumpPadMode.Controled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.up.normalized * 0.1f, 0.08f);
            
            //add a curved line that goes upwards to finaly refall to the target
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(jumpTarget.position + jumpTarget.up.normalized * 0.1f, 0.08f);

            Vector3 midpoint = GetMidpoint();
            midpoint += transform.up.normalized * jumpForce / 10;

            Gizmos.DrawLine(transform.position + transform.up.normalized * 0.1f, midpoint);
            Gizmos.DrawLine(jumpTarget.position + jumpTarget.up.normalized * 0.1f, midpoint);
        }
        else if(jumpPadMode == JumpPadMode.Charging)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }

    Vector3 GetMidpoint()
    {
        return (transform.position + jumpTarget.position) / 2;
    }

    IEnumerator ResetLaunch()
    {
        // wait for the cooldown to end and reset the playerLaunched bool
        yield return new WaitForSeconds(cooldown);
        playerLaunched = false;
    }
}
