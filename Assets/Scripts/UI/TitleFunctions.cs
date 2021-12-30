using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleFunctions : MonoBehaviour
{
    public Animator OpeningAnimation;
    public EventSystem EventSystem;
    public GameObject[] MainMenuToHide;
    public GameObject ControllerConfigCanvas;
    public GameObject LoadingCanvas;

    public void SelectButton()
    {
        EventSystem.SetSelectedGameObject(Array.Find(MainMenuToHide, x => x.GetComponent<Button>() != null));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowControllerCanvas()
    {
        Array.ForEach(MainMenuToHide, x => x.SetActive(false));
        ControllerConfigCanvas.SetActive(true);
    }

    public void HideControllerCanvas()
    {
        ControllerConfigCanvas.SetActive(false);
        Array.ForEach(MainMenuToHide, x => x.SetActive(true));
    }

    public void ToGameScene()
    {
        ControllerConfigCanvas.SetActive(false);
        LoadingCanvas.SetActive(true);
    }
}
