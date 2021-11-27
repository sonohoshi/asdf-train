using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class startFadeInvoke : MonoBehaviour
{
    public GameObject fadeimage;
    public bool btntrue;

    void fadestart()
    {
        if ( btntrue == true)
        {
            fadeimage.gameObject.SetActive(true);
        }
    }
}
