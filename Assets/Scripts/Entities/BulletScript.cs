using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector]
    public Vector2 AttackDirection;
    [HideInInspector]
    public float TracerRadius = 0.0f;
    [HideInInspector]
    public GameObject Sender;
    public TracerBulletManager Tracer;
    public AudioClip BulletSound;
    public AudioClip PogSound;
    public int PogChance = 500;
    public GameObject BulletMesh;
    public GameObject PogSprite;
    public float LifeTime = 5.0f;
    public float PullEffect = 1.0f;
    public int Damage;

    //[HideInInspector]
    private Rigidbody rBody;
    private float speedCap;

    private void Start()
    {
        if (Random.Range(0, PogChance) == 0 && Sender.GetComponent<Player>() != null)
        {
            PogSprite.SetActive(true);
            BulletMesh.SetActive(false);
            AudioSource.PlayClipAtPoint(PogSound, Vector3.zero);
        }
        else
        {
            AudioSource.PlayClipAtPoint(BulletSound, Vector3.zero);
        }

        if (TracerRadius <= 0.0f)
        {
            DestroyImmediate(Tracer.gameObject);
        }
        else
        {
            Tracer.Sender = Sender;
            Tracer.GetComponent<SphereCollider>().enabled = true;
            Tracer.GetComponent<SphereCollider>().radius = TracerRadius;
        }

        rBody = GetComponent<Rigidbody>();
        rBody.AddForce(new Vector3(AttackDirection.x, 0.0f, AttackDirection.y), ForceMode.VelocityChange);
        speedCap = AttackDirection.magnitude;
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), Sender.GetComponent<CapsuleCollider>(), true);
        float Angle = 90.0f + Mathf.Atan2(-AttackDirection.normalized.y, AttackDirection.normalized.x) * (180.0f / Mathf.PI);
        transform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime <= 0.0f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (Tracer.Target != null)
        {
            Vector3 dirForce = Tracer.Target.position - transform.position;
            dirForce.Normalize();
            dirForce *= PullEffect;
            rBody.AddForce(dirForce, ForceMode.VelocityChange);
            if (rBody.velocity.magnitude > speedCap)
                rBody.velocity = rBody.velocity.normalized * speedCap;

            float Angle = 90.0f + Mathf.Atan2(-rBody.velocity.normalized.z, rBody.velocity.normalized.x) * (180.0f / Mathf.PI);
            transform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true) return;
        GameObject otherObject = other.gameObject;
        // Checks if it hits the sender
        if (otherObject == Sender)
            return;


        Entity otherEntity = otherObject.GetComponent<Entity>();
        // check if it is an entity
        if (otherEntity != null)
        {
            otherEntity.TakeDamage(this);
        }
        // if it hits anything other than the player or another entity destroy the bullet
        Destroy(gameObject);
    }
}
