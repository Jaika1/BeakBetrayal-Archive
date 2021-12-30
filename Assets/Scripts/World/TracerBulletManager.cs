using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerBulletManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject Sender;
    [HideInInspector]
    public Transform Target = null;

    private bool canTrace = true;
    private SphereCollider triggerZone;
    private bool beenClose = false;

    private void Awake()
    {
        triggerZone = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTrace &&
            !other.isTrigger &&
            Target == null &&
            other.gameObject != Sender &&
            other.GetComponent<Entity>() != null &&
            other.GetComponent<Entity>().EntityModifiers.Health > 0.0f)
        {
            Target = other.transform;
        }
    }

    private void Update()
    {
        if (Target != null)
        {
            if (!beenClose)
            {
                if (Vector3.Distance(transform.position, Target.position) < triggerZone.radius / 2.0f)
                    beenClose = true;
            }
            else
            {
                if (Vector3.Distance(transform.position, Target.position) > triggerZone.radius / 2.0f)
                    Target = null;
                canTrace = false;
            }
        }
    }
}
