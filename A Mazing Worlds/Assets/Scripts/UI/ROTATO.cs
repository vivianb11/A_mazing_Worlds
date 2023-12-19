using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTATO : MonoBehaviour
{
    public Vector3 RotatData;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotatData * Time.deltaTime);
    }
}
