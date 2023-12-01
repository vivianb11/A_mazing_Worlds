using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class JumpPadMain : MonoBehaviour
{
    //add a event so that when the player is launched a event is called
    public delegate void LaunchPlayer();
    public static event LaunchPlayer LaunchPlayerEvent;

    [SerializeField] float jumpForce = 100;
    [SerializeField] float cooldown = 1;
    bool playerLaunched = false;

    // different states of the jump pad
    enum JumpPadState { Idle, Launching, Cooldown };
    JumpPadState jumpPadState = JumpPadState.Idle;

    // different modes of the jump pad
    enum JumpPadMode { Simple, Controled, Charging};

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
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * jumpForce, ForceMode.Impulse);
            LaunchPlayerEvent();
            StartCoroutine(ResetLaunch());
        }
    }

    IEnumerator ResetLaunch()
    {
        // wait for the cooldown to end and reset the playerLaunched bool
        yield return new WaitForSeconds(cooldown);
        playerLaunched = false;
    }
}
