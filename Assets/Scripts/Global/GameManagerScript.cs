using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector]
    public UI[] PlayerUIList;

    public float xTeleportMin = -8.25f;
    public float xTeleportMax = 9.6f;
    public float zTeleportMin = -4.25f;
    public float zTeleportMax = 13.8f;

    public GameObject Scoreboard;
    public GameObject RoundOver;
    public GameObject AIWin;
    public GameObject PlayerWin;
    public GameObject[] PlayerWinNumbers;

    public GameObject RareCrystalSpawnerRef;
    public LoadGameScript LoadingCanvas;

    public AudioClip StartRoundSound;
    public AudioClip PlayMusic;
    public AudioClip PinchMusic;
    public AudioClip IntermissionMusic;
    public AudioClip ResultsMusic;

    private AudioSource musicPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0.0f;
        musicPlayer = GetComponent<AudioSource>();
    }

    public void StartLogic()
    {
        GameNFO.RoundNum++;
        string splashText = GameNFO.RoundNum == GameNFO.RoundCount ? "Final Round..." : $"Round {GameNFO.RoundNum}...";
        Array.ForEach(PlayerUIList, x => x.SplashText(splashText, 2.0f, 0.5f));
        StartCoroutine(StartGameAfter(2.0f));
    }

    IEnumerator StartGameAfter(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Array.ForEach(PlayerUIList, x => x.SplashText("GO!!!", 2.0f, 0.5f));
        musicPlayer.clip = PlayMusic;
        musicPlayer.Play();
        Time.timeScale = 1.0f;
        AudioSource.PlayClipAtPoint(StartRoundSound, Vector3.zero);
        StartCoroutine(TeleportWarning(GameNFO.FinalTeleportSeconds));
        //StartCoroutine(RareCrystalSpawn(10.0f));
        StartCoroutine(RareCrystalSpawn(Random.Range((GameNFO.FinalTeleportSeconds/2.0f) - 15.0f, (GameNFO.FinalTeleportSeconds / 2.0f) + 15.0f)));
    }

    IEnumerator TeleportWarning(float teleportDelay, float warningDuration = 10.0f)
    {
        yield return new WaitForSeconds(teleportDelay - warningDuration);
        Array.ForEach(PlayerUIList, x => x.SplashText("Center teleportation imminent!", 5f, 2.0f));
        StartCoroutine(Teleport(warningDuration));
    }

    IEnumerator Teleport(float teleportDelay, float fireLock = 5.0f)
    {
        yield return new WaitForSeconds(teleportDelay);
        Array.ForEach(PlayerUIList, x => x.SplashText("Begin the standoff!", fireLock, 1.0f));
        Array.ForEach(FindObjectsOfType<TimedWall>(),x => x.ResetPosition()); // Call each wall to reset to its starting position. 

        musicPlayer.clip = PinchMusic;
        musicPlayer.Play();

        // Iterate through all entities if there location is not inside a selected area then teleport inside slected area
        Entity[] entityComponets = FindObjectsOfType<Entity>();
        foreach (Entity eScript in entityComponets)
        {
            eScript.WispModifiers.FireAbilityDuration = fireLock;
            eScript.BulletDeletion(eScript.gameObject);

            // if the entity is outside a set area
            if (eScript.transform.position.x < xTeleportMin || eScript.transform.position.x > xTeleportMax || eScript.transform.position.z <= zTeleportMin || eScript.transform.position.z >= zTeleportMax)
            {
                // move the player inside the center area
                // variabls to be changed later
                eScript.transform.position = new Vector3(Random.Range(xTeleportMin, xTeleportMax), 1f, Random.Range(zTeleportMin, zTeleportMax));
            }
        }
    }

    IEnumerator RareCrystalSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Array.ForEach(PlayerUIList, x => x.SplashText("A rare crystal is spawning in the middle!", 4.0f, 1.0f));
        GameObject crystalSpawner = Instantiate(RareCrystalSpawnerRef);
        crystalSpawner.transform.position = new Vector3(0.0f, 0.1f, 0.0f);
    }

    public void CheckGameFinished()
    {
        Entity[] entitiesLeft = FindObjectsOfType<Entity>();
        entitiesLeft = Array.FindAll(entitiesLeft, x => x.EntityModifiers.Health > 0.0);

        bool playerWinCondition = entitiesLeft.Length > 1 ? false : true;
        bool enemyWinCondition = Array.FindAll(entitiesLeft, x => x is Player).Length == 0 ? true : false;

        if (playerWinCondition || enemyWinCondition)
        {
            Time.timeScale = 0.0f;
            Entity winner = entitiesLeft[Random.Range(0, entitiesLeft.Length)];
            Player playerWinner = winner is Player ? (Player)winner : null;
            Enemy aiWinner = winner is Enemy ? (Enemy)winner : null;
            winner.Information.Score += Scoring.WinBonus;


            if (GameNFO.RoundNum != GameNFO.RoundCount)
            {
                RoundOver.SetActive(true);
                Scoreboard.GetComponent<Scoreboard>().UpdateScoreboard();
                Scoreboard.SetActive(true);
                musicPlayer.clip = IntermissionMusic;
                musicPlayer.loop = false;
                musicPlayer.Play();
                StartCoroutine(NextSceneAfter());
            }
            else
            {
                GameNFO.RoundNum = 0;
                GameNFO.SortPlayerListByScore();
                EntityInformation ultimateWinner = GameNFO.EntityNFOs[0];
                if (ultimateWinner.PlayerGamepad != null)
                {
                    int num = ultimateWinner.EntityId;
                    PlayerWin.SetActive(true);
                    PlayerWinNumbers[num].SetActive(true);
                }
                else
                {
                    AIWin.SetActive(true);
                }
                Scoreboard.GetComponent<Scoreboard>().UpdateScoreboard(true);
                Scoreboard.SetActive(true);
                musicPlayer.clip = ResultsMusic;
                musicPlayer.Play();
                StartCoroutine(NextSceneAfter(true, 10.0f));
            }
        }
    }

    IEnumerator NextSceneAfter(bool title = false, float delay = 6.0f)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (title) Time.timeScale = 1.0f;
        LoadingCanvas.NextSceneName = title ? "TitleScreen" : SceneManager.GetActiveScene().name;
        LoadingCanvas.gameObject.SetActive(true);
    }
}
