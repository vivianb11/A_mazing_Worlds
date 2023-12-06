using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity: MonoBehaviour
{
    private GameManager gameManager;

    private Transform[] attractedObjects;

    public bool showGravityRange = true;

    public float gravityForce =1;
    public float gravityRange = 75;

    private void Start()
    {
        if (gameManager == null) gameManager = Transform.FindFirstObjectByType<GameManager>();

        int arraySize = gameManager.players.Count + GameObject.FindGameObjectsWithTag("ObjectToAttract").Length;

        attractedObjects = new Transform[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            if (i < gameManager.players.Count)
                attractedObjects[i] = gameManager.players[i].transform;
            else
                attractedObjects[i] = GameObject.FindGameObjectsWithTag("ObjectToAttract")[i - gameManager.players.Count].transform;
        }
    }

    private void FixedUpdate()
    {
        // for each object in the array, if the object is in the gravity range, add a force to the object
        for (int i = 0; i < attractedObjects.Length; i++)
        {
            if (!attractedObjects[i])
                continue;

            if (Vector3.Distance(transform.position, attractedObjects[i].position) > gravityRange)
                continue;

            attractedObjects[i].GetComponent<Rigidbody>().AddForce((transform.position - attractedObjects[i].position) * gravityForce);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (showGravityRange)
        {
            Gizmos.DrawWireSphere(transform.position, gravityRange);
        }
    }
}
