using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadMain : MonoBehaviour
{
    //add a event so that when the player is launched a event is called
    public delegate void LaunchPlayer();
    public static event LaunchPlayer LaunchPlayerEvent;

    [SerializeField] float jumpForce = 100;
    [SerializeField] float cooldown = 1;
    bool playerLaunched = false;

    private void Awake()
    {
        // add the event to the camera
        LaunchPlayerEvent += Camera.main.GetComponent<CameraShake>().ShakeCamera;
    }

    private void OnDisable()
    {
        // remove the event from the camera
        LaunchPlayerEvent -= Camera.main.GetComponent<CameraShake>().ShakeCamera;
    }

    private void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        // if the player collides with the object it will launch the player with a addforce and call the event
        if (collision.gameObject.tag == "Player" && !playerLaunched)
        {
            playerLaunched = true;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * jumpForce);
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
