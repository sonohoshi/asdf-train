using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainManager : MonoBehaviour
{
    [SerializeField] 
    private Vector3 destination;
    
    [SerializeField]
    private CanvasGroup blur;

    [SerializeField]
    private Text countText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Image powerLine;

    [SerializeField]
    private Text toCheonanText;
    
    [SerializeField]
    private GameObject virtualCam;

    [SerializeField]
    private TrainMover train;

    [SerializeField] 
    private Vector3 badEndingCamRotation;

    [SerializeField]
    private Vector3 badEndingCamPosition;

    private Rigidbody _trainRigidbody;
    private int _startCount = 3;
    private float _chargedTime;

    public bool IsStarted { get; private set; }
    public static TrainManager Instance { get; private set; }

    private void Awake()
    {
        _trainRigidbody = train.GetComponent<Rigidbody>();
        Instance = this;
    }

    private IEnumerator Start()
    {
        while (_startCount > 0)
        {
            countText.text = _startCount.ToString();
            yield return new WaitForSeconds(1f);
            _startCount--;
            if (_startCount == 0)
            {
                blur.DOFade(0f, 1f);
            }
        }
        
        blur.gameObject.SetActive(false);
        IsStarted = true;
    }

    private void Update()
    {
        if (IsStarted && _trainRigidbody.velocity.y < 0f)
        {
            OnBadEnd();
        }
        
        if (IsStarted)
        {
            speedText.text = $"{_trainRigidbody.velocity.x * 10f:0} km/h";
            toCheonanText.text =
                $"천안까지 {Mathf.Clamp(100 - 100 * train.transform.position.x / destination.x, 0f, 100f):0.0} km";
            powerLine.DOFade(_trainRigidbody.velocity.x * 10f / 300f, 0f);
        }
    }

    private void OnBadEnd()
    {
        Destroy(train);
        Destroy(virtualCam);
        var mainCamTransform = Camera.main.transform;
        mainCamTransform.rotation = Quaternion.Euler(badEndingCamRotation);
        mainCamTransform.position = badEndingCamPosition;
        IsStarted = false;
        powerLine.DOFade(0f, .5f);
        speedText.text += " 이었던 것";
    }
}
