using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventText : MonoBehaviour
{
    public float SplashShownDuration = 1.5f;
    public float SplashFadeDuration = 0.5f;
    public float SplashRiseSpeed = 0.2f;
    public TMP_Text Text;

    private Canvas canvas;
    private Camera eventCamera;
    private float splashShownTimer = 0.0f;

    //public EventText() { }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = eventCamera;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(0.0f, SplashRiseSpeed, 0.0f) * Time.deltaTime, Space.Self);

        splashShownTimer += Time.deltaTime;

        if (splashShownTimer < SplashShownDuration)
        {
            if (splashShownTimer > SplashShownDuration - SplashFadeDuration)
            {
                float fadeValue = (SplashShownDuration - splashShownTimer) / SplashFadeDuration;
                Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, fadeValue);
            }
        }
    }

    public void SetEventTextAndCamera(Camera eventCamera, string text)
    {
        this.eventCamera = eventCamera;
        Text.text = text;
    }
}
