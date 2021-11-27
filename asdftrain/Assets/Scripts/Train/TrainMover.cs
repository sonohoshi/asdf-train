using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMover : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetInputFirstController() && GetInputSecondController())
        {
            _rigidbody.AddForce(Vector3.right * 5f);
        }
    }

    private bool GetInputFirstController()
    {
        return Input.GetKey(KeyCode.Z);
    }

    private bool GetInputSecondController()
    {
        return Input.GetKey(KeyCode.X);
    }
}
