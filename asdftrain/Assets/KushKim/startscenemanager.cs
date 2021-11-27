         using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startscenemanager : MonoBehaviour
{
    startFadeInvoke fadeinvoke;
    // Start is called before the first frame update
    public void gamestart()
    {
        fadeinvoke = GameObject.Find("fadeimage").GetComponent<startFadeInvoke>();
        SceneManager.LoadScene("ksmdev");
        Debug.Log("scene loaded!");
    }

    public void exitgame()
    {
        Application.Quit();
        Debug.Log("QUIT GAME");
    }
}

