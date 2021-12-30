using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileAnimation : MonoBehaviour
{
    public Image Tile;
    public float speed = 0.1f;
    public float tileScale = 0.5f;
    private RectTransform panelRect;

    // Start is called before the first frame update
    void Start()
    {
        panelRect = (RectTransform)Tile.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentOffset = Tile.material.GetVector("Vector2_Offset");

        Tile.material.SetVector("Vector2_Tiling", new Vector2(panelRect.rect.width / Tile.material.mainTexture.width, panelRect.rect.height / Tile.material.mainTexture.height) / tileScale);
        Tile.material.SetVector("Vector2_Offset", currentOffset + new Vector2(Time.deltaTime * speed, 0));
    }
}
