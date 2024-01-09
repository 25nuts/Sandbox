using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Battle()
    {
        SceneManager.LoadSceneAsync("Arena");
    }

    public void Build()
    {
        SceneManager.LoadSceneAsync("Editor");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
    }
}
