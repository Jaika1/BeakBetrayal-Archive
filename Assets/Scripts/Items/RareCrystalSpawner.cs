using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareCrystalSpawner : MonoBehaviour
{
    public float Delay = 15.0f;
    public GameObject CrystalPrefab;

    private void Awake()
    {
        StartCoroutine(SpawnCrystal());
    }

    IEnumerator SpawnCrystal()
    {
        yield return new WaitForSeconds(Delay);
        GameObject crystal = Instantiate(CrystalPrefab);
        crystal.transform.position = transform.position + new Vector3(0.0f, 0.0f, 3.0f);
        Array.ForEach(FindObjectOfType<GameManagerScript>().PlayerUIList, x => x.SplashText("Rare crystal spawned!", 4.0f));
        Destroy(gameObject);
    }
}
