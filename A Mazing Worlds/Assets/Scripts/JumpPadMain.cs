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

    //add a event so that when the player is launched a event is called
    public delegate void LaunchPlayer();
    public static event LaunchPlayer LaunchPlayerEvent;

    [SerializeField] float jumpForce = 100;
    [SerializeField] float cooldown = 1;
    bool playerLaunched = false;

    // different states of the jump pad
    JumpPadState jumpPadState = JumpPadState.Idle;

    // different modes of the jump pad
    [SerializeField] JumpPadMode jumpPadMode = JumpPadMode.Simple;

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
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
            //LaunchPlayerEvent();
            StartCoroutine(ResetLaunch());
        }
    }

    void OnDrawGizmos()
    {
        if(jumpPadMode == JumpPadMode.Simple)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.up * 2);
        }
        else if(jumpPadMode == JumpPadMode.Controled)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
        else if(jumpPadMode == JumpPadMode.Charging)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }

    IEnumerator ResetLaunch()
    {
        // wait for the cooldown to end and reset the playerLaunched bool
        yield return new WaitForSeconds(cooldown);
        playerLaunched = false;
    }
}
