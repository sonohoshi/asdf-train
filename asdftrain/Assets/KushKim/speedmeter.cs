using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class speedmeter : MonoBehaviour
{
    Rigidbody rigd;
    private Vector3 m_LastPosition;
    public float m_Speed;
    public Text m_KilometersPerHour;

    // Start is called before the first frame update
    void Start()
    {
        rigd = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        m_Speed = GetSpeed();
        m_KilometersPerHour.text = string.Format("{0:0} km/h", m_Speed * 10f);
        if (Input.GetKey("up"))
        {
            rigd.velocity = new Vector3(0, 0, 10);
        }

    }
    float GetSpeed()
    {
        float speed = (((transform.position - m_LastPosition).magnitude) / Time.deltaTime);
        m_LastPosition = transform.position;

        return speed;
    }
}
