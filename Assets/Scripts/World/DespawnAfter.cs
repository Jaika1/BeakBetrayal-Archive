using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfter : MonoBehaviour
{
    public float SecondsDelay = 3.0f;
    public float FadeTime = 1.0f;
    float elapsedTime = 0.0f;
    MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= SecondsDelay)
        {
            Destroy(gameObject);
        }

        if (elapsedTime > SecondsDelay - FadeTime)
        {
            float fadeValue = (SecondsDelay - elapsedTime) / FadeTime;
            renderer.material.color = new Color(fadeValue, fadeValue, fadeValue, fadeValue);
        }
    }
}
