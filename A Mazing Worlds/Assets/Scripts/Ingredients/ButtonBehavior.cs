using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonBehavior : MonoBehaviour
{
    public UnityEvent onPlayerTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onPlayerTriggerEnter.Invoke();
    }
}
