using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuStartTransition : MonoBehaviour
{
    public string SceneName;
    public Animator PanelAnimation;
    public float TransitionDelay = 1.0f;

    private void Start()
    {
        PanelAnimation.SetTrigger("Fade");

    }

    public void SceneChange()
    {
        PanelAnimation.SetTrigger("Idle");
        StartCoroutine(SceneChangerCoroutine());
    }

    IEnumerator SceneChangerCoroutine()
    {
        yield return new WaitForSeconds(TransitionDelay);
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
