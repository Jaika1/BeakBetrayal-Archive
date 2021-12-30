using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAroundTarget : MonoBehaviour
{
    public Transform Target;
    private float r;
    public float PiMultiple = 2.0f;
    public float Distance = 1.0f;

    void Awake()
    {
        r = Time.realtimeSinceStartup;
    }

    void Update()
    {
        r += Time.deltaTime * (Mathf.PI * PiMultiple);
        gameObject.transform.position = new Vector3(Target.position.x + (Mathf.Cos(r) * Distance), Target.position.y, Target.position.z - (Mathf.Sin(r) * Distance));
    }
}
