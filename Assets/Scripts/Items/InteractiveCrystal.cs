using UnityEngine;

public class InteractiveCrystal : MonoBehaviour
{
    public int CrystalHealth = 50;
    public GameObject ItemDropPrefab;
    public GameObject CrystalBroken;
    public AudioClip CrystalSound;
    public AudioClip CrystalHitSound;

    //public ItemRarity Rarity;
    public int ShowProbabilityCommon = 80;
    public int ShowProbabilityUncommon = 20;
    public int ShowProbabilityRare = 0;
    private IPickupItem item;

    private bool firstFramePassed = false;

    private void Awake()
    {
        // Make sure cumulative chances do not exceed 100%
        ShowProbabilityCommon = ShowProbabilityCommon > 100 ? 100 : ShowProbabilityCommon;
        ShowProbabilityUncommon = ShowProbabilityCommon + ShowProbabilityUncommon > 100 ? 100 - ShowProbabilityCommon : ShowProbabilityUncommon;
        ShowProbabilityRare = ShowProbabilityCommon + ShowProbabilityUncommon + ShowProbabilityRare > 100 ? 100 - ShowProbabilityCommon - ShowProbabilityUncommon : ShowProbabilityRare;

        if (ShowProbabilityCommon + ShowProbabilityUncommon + ShowProbabilityRare > 100)
        {
            Debug.LogWarning($"Cumulative item spawn chance is greater than 100%! Actual probabilities will be:\n" +
                $"Common = {ShowProbabilityCommon}\n" +
                $"Uncommon = {ShowProbabilityUncommon}\n" +
                $"Rare = {ShowProbabilityRare}\n", gameObject);
        }
        
        if (ShowProbabilityCommon == 0) ShowProbabilityCommon = -1;
        if (ShowProbabilityUncommon == 0) ShowProbabilityUncommon = -1;
        if (ShowProbabilityRare == 0) ShowProbabilityRare = -1;

        switch (Random.Range(0, 101))
        {
            // Common
            case int n when n <= ShowProbabilityCommon:
                item = GameNFO.CommonItems[Random.Range(0, GameNFO.CommonItems.Count)];
                break;
            // Uncommon
            case int n when n <= ShowProbabilityCommon + ShowProbabilityUncommon:
                item = GameNFO.UncommonItems[Random.Range(0, GameNFO.UncommonItems.Count)];
                break;
            // Rare
            case int n when n <= ShowProbabilityCommon + ShowProbabilityUncommon + ShowProbabilityRare:
                item = GameNFO.RareItems[Random.Range(0, GameNFO.RareItems.Count)];
                break;
        }
    }

    private void Start()
    {
        firstFramePassed = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // This may still fire twice if the crystal doesn't fall below 0 hp, but for now, crystal crits will just be a feature.
        if (CrystalHealth <= 0) return;
        GameObject gObj = other.gameObject;
        if (gObj == null) return;

        if (!firstFramePassed && other.GetComponent<Entity>() != null)
        {
            other.transform.position += new Vector3(0.0f, 0.0f, 2.5f);
        }

        BulletScript bullet = gObj.GetComponent<BulletScript>();
        if (bullet == null) return;
        CrystalHealth -= bullet.Damage;
        if (CrystalHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(CrystalSound, Vector3.zero);
            GameObject droppedItem = Instantiate(ItemDropPrefab);
            droppedItem.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z-.5f);
            droppedItem.GetComponent<InteractiveItem>().Item = item;
            GameObject cBroken = Instantiate(CrystalBroken, gameObject.transform.parent);
            cBroken.transform.position = transform.position;
            Destroy(gameObject);
        }
        else
        {
            AudioSource.PlayClipAtPoint(CrystalHitSound, Vector3.zero);
        }
    }
}
