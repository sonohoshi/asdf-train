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
    
    // --- for Debug ---
    private float _timeFloat;
    private int _checkCount;
    private int _frames;
    // --- for Debug ---
    
    private float _chargedTime;
    private bool _isBoosting;
    
    private const float BoostLimitTime = 5f;

    public float ChargedTime => _chargedTime;
    public bool IsBoosting => _isBoosting;

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
        if (!TrainManager.Instance.IsStarted)
        {
            return;
        }
        
        if (!_isBoosting)
        {
            _chargedTime = _rigidbody.velocity.x >= 28 ? _chargedTime + Time.deltaTime : 0;

            if (_chargedTime >= BoostLimitTime)
            {
                StartCoroutine(Boost());
            }
        }
        
        if (_firstJoycon == null || _secondJoycon == null)
        {
            if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.X))
            {
                _rigidbody.AddForce(Vector3.right * 5f);
                if (_rigidbody.velocity.x >= 30)
                {
                    _rigidbody.velocity = new Vector3(30, 0, 0);
                }
            }

            return;
        }
        
        if (GetInputFirstController() && GetInputSecondController())
        {
            _rigidbody.AddForce(Vector3.right * 25f);
            _checkCount++;
            if (_rigidbody.velocity.x >= 30)
            {
                _rigidbody.velocity = new Vector3(30, 0, 0);
            }
        }
    }

    private bool GetInputFirstController()
    {
        var accel = _firstJoycon.GetAccel();
        var distance = Mathf.Abs(Vector3.Distance(_firstPrePosition ?? Vector3.zero,accel));
        
        _firstPrePosition = accel;
        var result = distance >= JoyconManager.Instance.min_Distance;
        return result;
    }

    private bool GetInputSecondController()
    {
        var accel = _secondJoycon.GetAccel();
        var distance = Mathf.Abs(Vector3.Distance(_secondPrePosition ?? Vector3.zero,accel));
        
        _secondPrePosition = accel;
        var result = distance >= JoyconManager.Instance.min_Distance;
        return result;
    }

    private IEnumerator Boost()
    {
        TrainManager.Instance.OnBoost();
        
        _isBoosting = true;
        _chargedTime = 2f;
        while (_chargedTime > 0f)
        {
            _rigidbody.velocity = new Vector3(30, 0, 0);
            yield return null;
            _chargedTime -= Time.deltaTime;
        }

        _isBoosting = false;
        TrainManager.Instance.DisableBoost();
    }
}
