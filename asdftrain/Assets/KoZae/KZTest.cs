using System.Collections;
using System.Collections.Generic;
using KZLib.Develop;
using UnityEngine;

public class KZTest : MonoBehaviour
{
    private void OnEnable()
    {
        Broadcaster<int>.EnableListener("JoyConMove", OnMove);
    }

    private void OnDisable()
    {
        Broadcaster<int>.DisableListener("JoyConMove", OnMove);
    }

    void OnMove(int _id)
    {
        Debug.Log($"{_id} !!!!");
    }
}
