using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using KZLib.Develop;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TrainMover : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _firstMove;
    private bool _secondMove;
    private Joycon _firstJoycon;
    private Joycon _secondJoycon;
    
    private Vector3? _firstPrePosition;
    private Vector3? _secondPrePosition;
    private float _timeFloat;
    private int _checkCount;
    private int _frames;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _firstJoycon = JoyconManager.Instance.GetJoyConData(0);
        _secondJoycon = JoyconManager.Instance.GetJoyConData(1);
    }
    
    private void Update()
    {
        _timeFloat += Time.deltaTime;
        _frames++;
        if (_firstJoycon == null || _secondJoycon == null)
        {
            return;
        }
        
        if (GetInputFirstController() && GetInputSecondController())
        {
            _rigidbody.AddForce(Vector3.right * 25f);
            _checkCount++;
        }

        if (_timeFloat >= 1f)
        {
            _timeFloat = 0;
            Debug.LogError($"checked frame per sec : {(float)_checkCount/(float)_frames}, {_checkCount}, {_frames}");
            _checkCount = 0;
            _frames = 0;
        }
    }

    private bool GetInputFirstController()
    {
        var accel = _firstJoycon.GetAccel();
        var distance = Mathf.Abs(Vector3.Distance(_firstPrePosition ?? Vector3.zero,accel));
        
        _firstPrePosition = accel;
        var result = distance >= JoyconManager.Instance.min_Distance;
        //Debug.Log($"{0} index joycon result : {result}");
        return result;
    }

    private bool GetInputSecondController()
    {
        var accel = _secondJoycon.GetAccel();
        var distance = Mathf.Abs(Vector3.Distance(_secondPrePosition ?? Vector3.zero,accel));
        
        _secondPrePosition = accel;
        var result = distance >= JoyconManager.Instance.min_Distance;
        //Debug.Log($"{1} index joycon result : {result}");
        return result;
    }
}
