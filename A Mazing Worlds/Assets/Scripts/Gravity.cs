using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity: MonoBehaviour
{
    private GameManager gameManager;

    private Transform[] attractedObjects;

    public bool showGravityRange = true;

    public float gravityForce =12;
    public float gravityRange = 30;

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

    private void Update()
    {
        // for each object in the array, if the object is in the gravity range, add a force to the object
        for (int i = 0; i < attractedObjects.Length; i++)
        {
            if (gravityRange >= Vector3.Distance(this.transform.position, gameManager.players[i].transform.position) && gameManager.players[i])
            {
                gameManager.players[i].GetComponent<Rigidbody>().AddForce((this.transform.position - gameManager.players[i].transform.position) * (gravityForce * Time.deltaTime));
            }
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
