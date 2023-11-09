using System.Collections.Generic;
using UnityEngine;

public class Gravité: MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public bool showGravityRange = true;

    public float gravityForce =12;
    public float gravityRange = 30;

    private void Start()
    {
        if (gameManager == null) { gameManager = Transform.FindFirstObjectByType<GameManager>(); }
    }

    private void Update()
    {
        for (int i = 0; i < gameManager.players.Count; i++)
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
