using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerSetup : MonoBehaviour
{
    //private static Random rand = new Random();

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject CameraPrefab;
    public GameManagerScript GameManagerScript;
    public GameObject TwoPlayerLine;
    public GameObject FourPlayerLine;
    public GameObject[] PlayerOverlays;

    // Start is called before the first frame update
    void Start()
    {
        float camXScale = EntityInformation.CurrentHumanPlayers > 1 ? 0.5f : 1.0f;
        float camYScale = EntityInformation.CurrentHumanPlayers > 2 ? 0.5f : 1.0f;

        List<GameObject> spawnPoints = GameObject.FindGameObjectsWithTag("Respawn").ToList();
        Vector3[] spawnPointLocations = new Vector3[spawnPoints.Count];
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            spawnPointLocations[i] = spawnPoints[i].transform.position;
        }

        switch (EntityInformation.CurrentHumanPlayers)
        {
            case 2:
                TwoPlayerLine.SetActive(true);
                break;
            case 3:
                FourPlayerLine.SetActive(true);
                break;
            case 4:
                FourPlayerLine.SetActive(true);
                break;
        }

        for (int i = 0; i < EntityInformation.CurrentHumanPlayers; ++i)
        {
            PlayerOverlays[i].SetActive(true);
        }
        
        GameManagerScript.PlayerUIList = new UI[EntityInformation.CurrentHumanPlayers];

        for (int i = 0; i < GameNFO.MaxPlayers; ++i)
        {
            GameObject playerObj = GameNFO.EntityNFOs[i].PlayerGamepad != null ? Instantiate(PlayerPrefab) : Instantiate(EnemyPrefab);

            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            playerObj.transform.position = new Vector3(spawnPoints[spawnPointIndex].transform.position.x, 1.0f, spawnPoints[spawnPointIndex].transform.position.z);
            spawnPoints[spawnPointIndex].SetActive(false);
            spawnPoints.RemoveAt(spawnPointIndex);

            Entity entity = playerObj.GetComponent<Entity>();
            entity.Information = GameNFO.EntityNFOs[i];

            if (entity is Player)
            {
                GameObject camObj = Instantiate(CameraPrefab);
                Player player = (Player)entity;
                BetterCamera camScript = camObj.GetComponent<BetterCamera>();
                HealthBar healthBar = camObj.GetComponent<HealthBar>();
                Camera camera = camObj.GetComponent<Camera>();
                UI ui = camObj.GetComponent<UI>();
                GameManagerScript.PlayerUIList[i] = ui;
                RectTransform sliderRect = camObj.GetComponentInChildren<Slider>().gameObject.transform as RectTransform;
                switch (i)
                {
                    case 0:
                        sliderRect.anchorMin = new Vector2(0.0f, 1.0f);
                        sliderRect.anchorMax = new Vector2(0.0f, 1.0f);
                        sliderRect.pivot = new Vector2(0.0f, 1.0f);
                        sliderRect.anchoredPosition = new Vector2(73.0f + sliderRect.sizeDelta.x, -41.0f);
                        sliderRect.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                        ui.RightAlign = true;

                        if (EntityInformation.CurrentHumanPlayers < 3)
                            ui.FromSide = 24.0f;
                        else
                            ui.FromSide = 16.0f;
                        break;
                    case 1:
                        sliderRect.anchorMin = new Vector2(1.0f, 1.0f);
                        sliderRect.anchorMax = new Vector2(1.0f, 1.0f);
                        sliderRect.pivot = new Vector2(1.0f, 1.0f);
                        sliderRect.anchoredPosition = new Vector2(-73.0f, -41.0f);
                        sliderRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        ui.RightAlign = false;

                        if (EntityInformation.CurrentHumanPlayers < 3)
                            ui.FromSide = 24.0f;
                        else
                            ui.FromSide = 16.0f;
                        break;
                    case 2:
                        sliderRect.anchorMin = new Vector2(0.0f, 0.0f);
                        sliderRect.anchorMax = new Vector2(0.0f, 0.0f);
                        sliderRect.pivot = new Vector2(0.0f, 1.0f);
                        sliderRect.anchoredPosition = new Vector2(73.0f + sliderRect.sizeDelta.x, 60.0f);
                        sliderRect.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                        ui.FromSide = 16.0f;
                        ui.RightAlign = true;
                        break;
                    case 3:
                        sliderRect.anchorMin = new Vector2(1.0f, 0.0f);
                        sliderRect.anchorMax = new Vector2(1.0f, 0.0f);
                        sliderRect.pivot = new Vector2(1.0f, 1.0f);
                        sliderRect.anchoredPosition = new Vector2(-73.0f, 60.0f);
                        sliderRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        ui.FromSide = 16.0f;
                        ui.RightAlign = false;
                        break;
                }

                camScript.ObjectToFollow = playerObj;
                healthBar.TargetEntity = player;
                switch (EntityInformation.CurrentHumanPlayers)
                {
                    case 1:
                        camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                        break;
                    case 2:
                        camera.rect = new Rect((i % 2) * 0.5f, 0.0f, camXScale, camYScale);
                        break;
                    case 3:
                    case 4:
                        camera.rect = new Rect((i % 2) * 0.5f, 0.5f - (Mathf.Floor(i / 2.0f) * 0.5f), camXScale, camYScale);
                        break;
                }
                //camera.rect = new Rect((i % 2) * 0.5f, 0.5f - (Mathf.Floor(i / 2.0f) * 0.5f), camXScale, camYScale);

                player.PlayerCameras.Add(camScript);
                player.HealthBar = healthBar;
                player.PlayerUI = ui;
            }

            //if (entity is Enemy)
            //{
            //    Enemy enemy = (Enemy)entity;
            //    enemy.PointsReference = spawnPointLocations;
            //}
        }

        spawnPoints.ForEach(x => x.SetActive(false));
        GameManagerScript.StartLogic();
    }
}
