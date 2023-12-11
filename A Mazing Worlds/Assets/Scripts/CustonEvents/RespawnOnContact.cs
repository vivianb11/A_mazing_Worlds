using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnOnContact : MonoBehaviour
{
    public UnityEvent OnPlayerCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnPlayerCollision.Invoke();
    }
}
