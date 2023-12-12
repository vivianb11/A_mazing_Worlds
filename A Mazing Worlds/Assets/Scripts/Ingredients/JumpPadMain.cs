using System;
using System.Collections;
using UnityEngine;

public class JumpPadMain : StateClass
{
    #region Variables
    enum JumpPadMode { Simple, Controled, Charging};

    [SerializeField] Transform _jumpTarget;

    [Header("Jump pad mode")]
    [SerializeField] JumpPadMode jumpPadMode = JumpPadMode.Simple;

    [Header("Jump pad settings")]
    [SerializeField] bool startVelocityZero = false;
    [SerializeField] bool desactivatesAirControl = false;
    [Space]
    [SerializeField] float jumpForce = 20;
    [Space]
    [SerializeField] float cooldown = 1;

    [Header("Visual")]
    [Min(1)]
    [SerializeField] float forceFactor = 2.5f;
    bool playerLaunched = false;
    #endregion

    private void Awake()
    {
        if(!_jumpTarget)
            _jumpTarget = transform.GetChild(0);

        if (!_jumpTarget)
            Debug.LogError("No jump target found");
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if the player collides with the object it will launch the player with a addforce and call the event
        if (collision.gameObject.tag == "Player" && !playerLaunched)
        {
            print("Player launched");
            playerLaunched = true;
            currentState = EState.Launching;

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
            Gizmos.DrawRay(transform.position, transform.up.normalized * (jumpForce/forceFactor));

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.up.normalized * 0.1f, 0.1f);

        }
        else if(jumpPadMode == JumpPadMode.Controled || jumpPadMode == JumpPadMode.Charging)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position + transform.up.normalized * 0.1f, 0.1f);
            Gizmos.DrawWireSphere(_jumpTarget.position + _jumpTarget.up.normalized * 0.1f, 0.1f);
            Gizmos.DrawLine(transform.position, _jumpTarget.position);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (_jumpTarget.position - transform.position).normalized * (jumpForce/forceFactor));
        }
    }

    void SimpleJump(Collider collision)
    {
        if (startVelocityZero)
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (desactivatesAirControl)
            collision.gameObject.GetComponent<PlayerMovement>().DesactivateAirControl();

        collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
    }

    private void ControledJump(Collider collision)
    {
        if (startVelocityZero)
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (desactivatesAirControl)
            collision.gameObject.GetComponent<PlayerMovement>().DesactivateAirControl();

        collision.gameObject.GetComponent<Rigidbody>().AddForce((_jumpTarget.position - collision.transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    private void ChargingJump(Collider collision)
    {
        throw new NotImplementedException();

        //Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        //for(int i = 0; i < 100; i++)
        //{
        //    if (Vector3.Dot(collision.transform.position - transform.position, transform.up) > 0.1f)
        //        rb.AddForce((this.transform.position - collision.transform.position).normalized * jumpForce * 0.2f, ForceMode.Force);
        //    else
        //        break;
        //}

        //Vector3 midpoint = GetMidpointWithHeight(collision.transform.position, _jumpTarget.position);

        //if (startVelocityZero)
        //    rb.velocity = Vector3.zero;

        //rb.AddForce((midpoint - collision.transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    IEnumerator ResetLaunch()
    {
        currentState = EState.Cooldown;

        // wait for the cooldown to end and reset the playerLaunched bool
        yield return new WaitForSeconds(cooldown);

        currentState = EState.Idle;
        playerLaunched = false;
    }
}
