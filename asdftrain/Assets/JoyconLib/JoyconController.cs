using System.Collections;
using System.Collections.Generic;
using KZLib.Develop;
using UnityEngine;

public class JoyconController : MonoBehaviour 
{
    public Vector3 accel;
    public int jc_ind;
    public Vector3? pre_Position;

    void Update () 
    {
        Joycon joyCon = JoyconManager.Instance.GetJoyConData(jc_ind);

        if(joyCon == null)
        {
            return;
        }

        accel = joyCon.GetAccel();

        if(pre_Position != null)
        {
            var distance = Mathf.Abs(Vector3.Distance(pre_Position.Value,accel));

            if(distance >= JoyconManager.Instance.min_Distance)
            {
                Broadcaster<int>.SendEvent("JoyConMove",jc_ind);
            }
        }

        pre_Position = accel;
    }
}