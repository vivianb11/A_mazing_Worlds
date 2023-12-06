using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorBehavior : MonoBehaviour
{
    enum AcceleratorState { Idle, Launching, Cooldown };
    enum AcceleratorMode { SimpleBoost, ControledBoost };

    [SerializeField] AcceleratorMode acceleratorMode = AcceleratorMode.SimpleBoost;
    
    [SerializeField] float boostForce = 10;
    [SerializeField] float cooldown = 1;
    bool playerAccelerated = false;

    // different states of the jump pad
    AcceleratorState acceleratorState = AcceleratorState.Idle;

    private void Update()
    {
        
    }

    // on collision with the player, add a force to the players current direction flattened to the controler point plane XY
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !playerAccelerated)
        {
            print("Player Accelerated");
            playerAccelerated = true;
            acceleratorState = AcceleratorState.Launching;

            switch (acceleratorMode)
            {
                case AcceleratorMode.SimpleBoost:
                    SimpleBoost(collision);
                    break;
                case AcceleratorMode.ControledBoost:
                    ControledBoost(collision);
                    break;
            }

            StartCoroutine(ResetLaunch(cooldown));
        }
    }

    void SimpleBoost(Collider collision)
    {
        Vector3 playerDirection = collision.gameObject.GetComponent<Rigidbody>().velocity;
        playerDirection = Vector3.ProjectOnPlane(playerDirection, this.transform.right);

        collision.gameObject.GetComponent<Rigidbody>().AddForce(playerDirection.normalized * boostForce, ForceMode.Impulse);
    }

    void ControledBoost(Collider collision)
    {
        Vector3 playerDirection = collision.gameObject.GetComponent<Rigidbody>().velocity;
        playerDirection = Vector3.ProjectOnPlane(playerDirection, this.transform.right);

        collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * boostForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        switch (acceleratorMode)
        {
            case AcceleratorMode.ControledBoost:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + transform.up*0.15f, 0.05f);
                Gizmos.DrawLine(transform.position + transform.up * 0.15f, transform.position + transform.up * 0.15f + transform.forward.normalized * boostForce);
                break;
        }
    }

    IEnumerator ResetLaunch(float duration)
    {
        yield return new WaitForSeconds(0.1f);
        
        acceleratorState = AcceleratorState.Cooldown;
        yield return new WaitForSeconds(duration - 0.1f);
        
        acceleratorState = AcceleratorState.Idle;
        playerAccelerated = false;
    }
}
