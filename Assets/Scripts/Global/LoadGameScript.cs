using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScript : MonoBehaviour
{
    public bool GameBoot = false;
    public string NextSceneName;

    void Awake()
    {
        if (GameBoot) Time.timeScale = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameBoot)
        {
            FirstTimeSetupMethod();
        }
        else
        {
            LoadScene();
        }
    }

    async void FirstTimeSetupMethod()
    {
        GameNFO.PickupItems = (from t in Assembly.GetExecutingAssembly().GetTypes()
                               where t.GetInterfaces().Contains(typeof(IPickupItem))
                               select (IPickupItem)Activator.CreateInstance(t)).ToList();

        GameNFO.CommonItems = GameNFO.PickupItems.Where(x => x.Rarity == ItemRarity.Common).ToList();
        GameNFO.UncommonItems = GameNFO.PickupItems.Where(x => x.Rarity == ItemRarity.Uncommon).ToList();
        GameNFO.RareItems = GameNFO.PickupItems.Where(x => x.Rarity == ItemRarity.Rare).ToList();

        LoadScene();
    }

    async void LoadScene()
    {
        //SceneManager.LoadScene(NextSceneName);

        //yield return new WaitForSeconds(1.0f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneName);

        //while (!asyncLoad.isDone)
        //{
        //    yield return null;
        //}
    }
}
