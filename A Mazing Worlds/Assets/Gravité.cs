using UnityEngine;

public class Gravité: MonoBehaviour
{
    public GameObject joueur;
    public float gravityForce;
    public float gravityRange; 
    private void Update()
    {

        if (gravityRange >= Vector3.Distance(this.transform.position, joueur.transform.position))
        {
            joueur.GetComponent<Rigidbody>().AddForce((this.transform.position - joueur.transform.position)*(gravityForce*Time.deltaTime));
        }

    }
}
