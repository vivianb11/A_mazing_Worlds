using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class EV_PlayerInteraction : MonoBehaviour
{
    [Foldout("Trigger")]
    public UnityEvent PlayerTriggerEnter;
    [Foldout("Trigger")]
    public UnityEvent PlayerTriggerExit;
    [Foldout("Collision")]
    public UnityEvent PlayerCollisionEnter;
    [Foldout("Collision")]
    public UnityEvent PlayerCollisionExit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PlayerCollisionEnter.Invoke();
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PlayerCollisionExit.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerTriggerEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerTriggerExit.Invoke();
    }
}
