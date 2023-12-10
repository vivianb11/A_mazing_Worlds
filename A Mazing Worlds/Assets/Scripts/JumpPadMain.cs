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

    [SerializeField] float jumpForce = 20;
    [SerializeField] float jumpHeight = 5;

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

            switch (jumpPadMode)
            {
                case JumpPadMode.Simple:
                    SimpleJump(collision);
                    break;
                case JumpPadMode.Controled:
                    ControledJump(collision);
                    break;
                case JumpPadMode.Charging:
                    ChargingJump(collision);
                    break;
            }

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
        else if(jumpPadMode == JumpPadMode.Controled || jumpPadMode == JumpPadMode.Charging)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position + transform.up.normalized * 0.1f, 0.1f);
            Gizmos.DrawWireSphere(jumpTarget.position + jumpTarget.up.normalized * 0.1f, 0.1f);

            Vector3 midpoint = GetMidpointWithHeight();

            Gizmos.DrawLine(transform.position + transform.up.normalized * 0.1f, midpoint);
            Gizmos.DrawLine(jumpTarget.position + jumpTarget.up.normalized * 0.1f, midpoint);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + transform.up.normalized * 0.2f, ((midpoint + transform.up.normalized * 0.1f) - (transform.position + transform.up.normalized * 0.2f)).normalized * jumpForce);
        }
    }

    void SimpleJump(Collider collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
    }

    private void ChargingJump(Collider collision)
    {
        Vector3 midpoint = GetMidpointWithHeight();

        collision.gameObject.GetComponent<Rigidbody>().AddForce((midpoint - collision.transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    private void ControledJump(Collider collision)
    {
        throw new NotImplementedException();
    }

    Vector3 GetMidpoint()
    {
        return (transform.position + jumpTarget.position) / 2;
    }

    Vector3 GetMidpointWithHeight()
    {
        return (transform.position + jumpTarget.position) / 2 + transform.up.normalized * jumpHeight;
    }

    IEnumerator ResetLaunch()
    {
        // wait for the cooldown to end and reset the playerLaunched bool
        yield return new WaitForSeconds(cooldown);
        playerLaunched = false;
    }
}
