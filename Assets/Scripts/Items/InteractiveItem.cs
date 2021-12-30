using System.Collections;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    public IPickupItem Item;
    public AudioClip ItemSound;
    public GameObject EventTextRef;
    public float HeightBase = 1.1f;
    public float HeightVariation = 0.4f;
    public float RotationRate = 2.0f;

    private float rotationAmount = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        
        GameObject gObj = other.gameObject;
        if (gObj == null)  return;

        Player entity = gObj.GetComponent<Player>();
 
        if (entity == null)  return;
        entity.AddItem(Item);
        entity.Information.Score += Scoring.ItemPickedUp;
        GameObject eventText = Instantiate(EventTextRef);
        eventText.GetComponent<EventText>().SetEventTextAndCamera(entity.PlayerCameras[0].AttachedCamera, $"+{Scoring.ItemPickedUp}");
        eventText.transform.position = transform.position;
        AudioSource.PlayClipAtPoint(ItemSound, Vector3.zero);
        Destroy(gameObject);
    }

    private void Update()
    {
        rotationAmount += Time.deltaTime * RotationRate;
        transform.rotation = Quaternion.Euler(0.0f, rotationAmount * (180 / Mathf.PI), 0.0f);
        transform.position = new Vector3(transform.position.x, HeightBase + (Mathf.Sin(rotationAmount) * HeightVariation), transform.position.z);
    }
}
