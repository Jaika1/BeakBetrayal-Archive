using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    public Image SpriteImage;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text LoreText;
    public float ShowTime = 5.0f;

    private float countdown = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f) gameObject.SetActive(false);
    }

    public void ShowItem(IPickupItem item)
    {
        SpriteImage.sprite = item.Sprite;
        NameText.text = item.Name;
        DescriptionText.text = item.Description;
        LoreText.text = item.Lore;
        countdown = ShowTime;
        gameObject.SetActive(true);
    }
}
