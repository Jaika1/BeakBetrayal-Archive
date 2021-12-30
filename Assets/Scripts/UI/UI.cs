using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Canvas[] UICanvas;
    public int AmountOfItems = 1;
    public float Spacing = 16.0f;
    public float BoxSize = 48.0f;
    public float FromSide = 0.0f;
    public int ItemsBeforeNewLine = 6;
    public bool RightAlign = true;
    public GameObject ItemBoxRef;
    public ItemPopup ItemPopupRef;
    public TMP_Text AnnouncerText;

    private float splashShownTimer = 0.0f;
    private float splashShownDuration = 0.0f;
    private float splashFadeDuration= 0.0f;
    private List<GameObject> ItemHolders = new List<GameObject>();
    private Vector2 location;

    public void Start()
    {
        if (RightAlign)
            location = new Vector2(-Spacing - FromSide, Spacing + FromSide);
        else
            location = new Vector2(Spacing + FromSide, Spacing + FromSide);
        ((RectTransform)ItemPopupRef.gameObject.transform).anchoredPosition += new Vector2(0.0f, FromSide);
    }

    public void AddItem(IPickupItem item)
    {
        GameObject itemBox = ItemHolders.Count(x => x.name == item.Name) == 0 ? Instantiate(ItemBoxRef) : ItemHolders.Find(x => x.name == item.Name);
        TMP_Text textComponent = itemBox.GetComponentInChildren<TMP_Text>();

        ItemPopupRef.ShowItem(item);
        if (itemBox.name != item.Name)
        {
            Image itemBoxSprite = itemBox.GetComponent<Image>();
            itemBoxSprite.sprite = GetBoxFromRarity(item.Rarity);
            Image itemBoxItemSprite = itemBox.GetComponentsInChildren<Image>()[1];
            itemBoxItemSprite.sprite = item.Sprite;
            itemBox.name = item.Name;
            itemBox.transform.SetParent(UICanvas[0].gameObject.transform, false);
            RectTransform rectTransform = (RectTransform)itemBox.transform;

            textComponent.text = "1x";
            ItemHolders.Add(itemBox);

            if (RightAlign)
            {
                rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.pivot = rectTransform.anchorMin;
                rectTransform.anchoredPosition = new Vector2(location.x, location.y);
                rectTransform.sizeDelta = new Vector2(BoxSize, BoxSize * (4.0f / 3.0f));
                
                location -= new Vector2(BoxSize + Spacing, 0.0f);

                if (AmountOfItems % ItemsBeforeNewLine == 0)
                {
                    location = new Vector2(-Spacing - FromSide, location.y + Spacing + (BoxSize * (4.0f / 3.0f)));
                }
            }
            else
            {
                rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.pivot = rectTransform.anchorMin;
                rectTransform.anchoredPosition = new Vector2(location.x, location.y);
                rectTransform.sizeDelta = new Vector2(BoxSize, BoxSize * (4.0f / 3.0f));

                location += new Vector2(BoxSize + Spacing, 0.0f);

                if (AmountOfItems % ItemsBeforeNewLine == 0)
                {
                    location = new Vector2(Spacing + FromSide, location.y + Spacing + (BoxSize * (4.0f / 3.0f)));
                }
            }
            AmountOfItems++;
        }
        else
        {
            textComponent.text = $"{int.Parse(textComponent.text.Replace("x", "")) + 1}x";
        }
    }

    public void EnableCanvas(uint index)
    {
        if (index < UICanvas.Length)
        {
            // Turn off all canvasses
            for (uint i=0; i<UICanvas.Length; i++)
            {
                UICanvas[i].gameObject.SetActive(false);
            }

            // Turn on the canvas we want
            UICanvas[index].gameObject.SetActive(true);
        }
    }

    public void SplashText(string text, float duration = 3.0f, float fadeTime = 1.0f)
    {
        AnnouncerText.text = text;
        AnnouncerText.color = Color.white;
        splashShownTimer = 0.0f;
        splashShownDuration = duration;
        splashFadeDuration = fadeTime;
        StartCoroutine(HideAfter(splashShownDuration));
    }

    private void Update()
    {
        splashShownTimer += Time.unscaledDeltaTime;

        if (splashShownTimer < splashShownDuration)
        {
            if (splashShownTimer > splashShownDuration - splashFadeDuration)
            {
                float fadeValue = (splashShownDuration - splashShownTimer) / splashFadeDuration;
                AnnouncerText.color = new Color(1.0f, 1.0f, 1.0f, fadeValue);
            }
        }
    }

    private Sprite GetBoxFromRarity(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return Resources.Load<Sprite>("ItemBoxes/T_ItemBox_Common");
            case ItemRarity.Uncommon:
                return Resources.Load<Sprite>("ItemBoxes/T_ItemBox_Uncommon");
            case ItemRarity.Rare:
                return Resources.Load<Sprite>("ItemBoxes/T_ItemBox_Rare");
        }
        return Resources.Load<Sprite>("ItemBoxes/T_ItemBox_Common");
    }

    IEnumerator HideAfter(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        AnnouncerText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
