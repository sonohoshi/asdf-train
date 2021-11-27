         using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startscenemanager : MonoBehaviour
{
    // Start is called before the first frame update
    public void gamestart()
    {
        SceneManager.LoadScene("gamescene");
        Debug.Log("scene loaded!");
    }

    public void exitgame()
    {
        Application.Quit();
    }
}

