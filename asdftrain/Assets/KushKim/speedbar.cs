using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedbar : MonoBehaviour
{

    public GameObject dummyobject;
    public speedmeter m_speed;
    public float speed;

    public Image barSprite;

    public void Update()
    {
        m_speed = dummyobject.GetComponent<speedmeter>();
        m_speed.m_Speed = 0;



    }
}
