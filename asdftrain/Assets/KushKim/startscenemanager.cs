         using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startscenemanager : MonoBehaviour
{
    
    // Start is called before the first frame update
    public void gamestart()
    {
        GameObject.Find("MainCanvas").transform.Find("fadeImage").gameObject.SetActive(true);
        StartCoroutine(NextScene());
        
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("ksmdev");
    }
    public void exitgame()
    {
        Application.Quit();
        Debug.Log("QUIT GAME");
    }


}

