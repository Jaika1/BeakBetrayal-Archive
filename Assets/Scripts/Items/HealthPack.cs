using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int HealthPackSize = 0;
    public float RespawnCountdownTimer = 0f;
    public float RespawnCountdownMax = 50f;
    public float HeightBase = 1.1f;
    public float HeightVariation = 0.4f;
    public float RotationRate = 2.0f;
    public AudioClip PickupSound;
    public GameObject EventTextRef;

    private Collider collider;
    private Renderer renderer;
    private float rotationAmount = 0.0f;

    void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        //will countdown on the RespawnTimer
        if (RespawnCountdownTimer >= 0.0f)
        {
            RespawnCountdownTimer -= Time.deltaTime;
            if (RespawnCountdownTimer <= 0)
            {
                collider.enabled = true;
                renderer.enabled = true;
            }
        }
        else
        {
            rotationAmount += Time.deltaTime * RotationRate;
            transform.rotation = Quaternion.Euler(-90.0f, rotationAmount * (180 / Mathf.PI), 0.0f);
            transform.position = new Vector3(transform.position.x, HeightBase + (Mathf.Sin(rotationAmount) * HeightVariation), transform.position.z);
        }
    }

    // will deal with applying health
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.isTrigger) return;
        if (RespawnCountdownTimer > 0.0f) return;
        GameObject objectCol = collision.gameObject;
        Entity entity = objectCol.GetComponent<Entity>();

        // check if collsion is with a player entity
        if(entity != null)
        {
            entity.AdjustHealth(HealthPackSize);
            AudioSource.PlayClipAtPoint(PickupSound, Vector3.zero);

            entity.Information.Score += Scoring.HeathPackActivated;
            if (entity is Player)
            {
                GameObject eventText = Instantiate(EventTextRef);
                eventText.GetComponent<EventText>().SetEventTextAndCamera(((Player)entity).PlayerCameras[0].AttachedCamera, $"+{Scoring.HeathPackActivated}");
                eventText.transform.position = transform.position;
            }
            RespawnCountdownTimer = RespawnCountdownMax;

            collider.enabled = false;
            renderer.enabled = false;
        }
    }
}