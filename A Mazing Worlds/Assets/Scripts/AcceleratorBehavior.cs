using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBehavior : MonoBehaviour
{
    //bost force
    [SerializeField] float boostForce = 100;
    [SerializeField] float cooldown = 1;
    bool playerLaunched = false;

    // different states of the jump pad
    enum JumpPadState { Idle, Launching, Cooldown };
    JumpPadState jumpPadState = JumpPadState.Idle;

    //different materials for each state
    [SerializeField] Material idleMaterial, launchingMaterial, CooldownMaterial;

    private void Update()
    {
        // switch the material based on the state
        switch (jumpPadState)
        {
            case JumpPadState.Idle:
                GetComponent<Renderer>().material = (idleMaterial)? idleMaterial: GetComponent<Renderer>().material;
                break;
            case JumpPadState.Launching:
                GetComponent<Renderer>().material = (launchingMaterial) ? launchingMaterial : GetComponent<Renderer>().material;
                break;
            case JumpPadState.Cooldown:
                GetComponent<Renderer>().material = (CooldownMaterial) ? CooldownMaterial : GetComponent<Renderer>().material;
                break;
        }
    }

    // on collision with the player, add a force to the players current direction flattened to the controler point plane XY
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !playerLaunched)
        {
            playerLaunched = true;

            Vector3 playerDirection = collision.gameObject.GetComponent<Rigidbody>().velocity;
            playerDirection = Vector3.ProjectOnPlane(playerDirection, this.transform.right);

            jumpPadState = JumpPadState.Launching;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(playerDirection.normalized * boostForce,ForceMode.Impulse);

            StartCoroutine(ResetLaunch());
        }
    }

    IEnumerator ResetLaunch()
    {
        yield return new WaitForSeconds(0.1f);
        
        jumpPadState = JumpPadState.Cooldown;
        yield return new WaitForSeconds(cooldown);
        
        jumpPadState = JumpPadState.Idle;
        playerLaunched = false;
    }
}
