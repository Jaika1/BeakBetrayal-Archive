using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EntityMovement : MonoBehaviour
{
    public Vector3[] PathPoints = new Vector3[0];
    [HideInInspector]
    public List<Entity> NearbyPlayers = new List<Entity>();
    public float PathTolerance = 0.05f;
    public Animator[] Animations;
    private Rigidbody rbody;
    private Enemy entity;
    private Transform waistTransform;
    private Transform legTransform;

    void Start()
    {
        entity = GetComponent<Enemy>();
        rbody = GetComponent<Rigidbody>();
        rbody.freezeRotation = true;

        waistTransform = transform.Find("ToucanTopHalfIdle");
        legTransform = transform.Find("ToucanBottomHalfIdle");
    }

    void FixedUpdate()
    {
        if (PathPoints.Length > 0)
        {
            if (Mathf.Abs((PathPoints[0].x - transform.position.x) + (PathPoints[0].z - transform.position.z)) <= PathTolerance)
            {
                PathPoints = PathPoints.Skip(1).ToArray();
                if (PathPoints.Length == 0) return;
            }

            float moveAngle = Mathf.Atan2(-(PathPoints[0].z - transform.position.z), PathPoints[0].x - transform.position.x) * (180/Mathf.PI) - 90;
            waistTransform.rotation = Quaternion.Euler(0.0f, moveAngle, 0.0f);
            legTransform.rotation = Quaternion.Euler(0.0f, moveAngle, 0.0f);

            rbody.velocity += new Vector3(PathPoints[0].x - transform.position.x, 0.0f, PathPoints[0].z - transform.position.z).normalized * entity.EntityModifiers.Acceleration;
            rbody.velocity = rbody.velocity.magnitude > entity.EntityModifiers.SpeedCap ? rbody.velocity.normalized * entity.EntityModifiers.SpeedCap : rbody.velocity;
            Debug.DrawLine(transform.position, transform.position + rbody.velocity);
            rbody.velocity *= Time.deltaTime;
            if (entity.WispModifiers.SpeedAbilityDuration > 0.0f) rbody.velocity *= WispAbilties.SpeedModifier;
        }

        Array.ForEach(Animations, a => a.SetFloat("runSpeed", rbody.velocity.magnitude));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.gameObject) return;
        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null && entity.EntityModifiers.Health > 0 && !NearbyPlayers.Contains(entity)) NearbyPlayers.Add(entity);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == this.gameObject) return;
        Entity entity = other.GetComponentInParent<Entity>();
        if (entity != null && NearbyPlayers.Contains(entity)) NearbyPlayers.Remove(entity);
    }
}
