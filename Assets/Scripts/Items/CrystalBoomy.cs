using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBoomy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            Rigidbody rBody = gameObject.transform.GetChild(i).gameObject.GetComponent<Rigidbody>();
            rBody.AddForce(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 5.0f,ForceMode.VelocityChange);
        }
    }
}
