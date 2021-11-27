using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainManager : MonoBehaviour
{
    [SerializeField] 
    private Vector3 destination;
    
    [SerializeField]
    private CanvasGroup blur;

    [SerializeField]
    private Text speedText;

    [SerializeField] 
    private Image speedGauge;

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

    [SerializeField]
    private Image[] countImages;

    [SerializeField] 
    private Image startImage;

    [SerializeField] 
    private GameObject gameOverUI;

    [SerializeField] 
    private GameObject gameClearUI;

    [SerializeField] 
    private Image feverGaugeImage;

    [SerializeField]
    private Slider destinationSlider;

    [SerializeField]
    private Text clearText;

    [SerializeField] 
    private Button[] reStartButtons; 

    [SerializeField] 
    private Button[] exitButtons;
    
    private Rigidbody _trainRigidbody;
    private int _startCount = 3;
    private float _chargedTime;
    private Color _originLineColor;
    private float _totalTime;

    public bool IsStarted { get; private set; }
    public static TrainManager Instance { get; private set; }

    private void Awake()
    {
        _trainRigidbody = train.GetComponent<Rigidbody>();
        Instance = this;
        _originLineColor = powerLine.color;
        foreach (var button in reStartButtons)
        {
            button.onClick.AddListener(() => SceneManager.LoadScene(1));
        }

        foreach (var button in exitButtons)
        {
            button.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }

    private IEnumerator Start()
    {
        while (_startCount > 0)
        {
            countImages[_startCount - 1].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            countImages[--_startCount].gameObject.SetActive(false);
            if (_startCount == 0)
            {
                blur.DOFade(0f, 1f);
            }
        }

        IsStarted = true;
        startImage.gameObject.SetActive(true);
        startImage.DOFade(0f, 0f);
        startImage.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(1f);
        blur.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsStarted && _trainRigidbody.velocity.y < 0f)
        {
            OnBadEnd();
            return;
        }

        if (IsStarted && _trainRigidbody.velocity.x == 0 && train.transform.position.x > destination.x)
        {
            OnClear();
            return;
        }

        if (IsStarted)
        {
            _totalTime += Time.deltaTime;
            speedText.text = $"{_trainRigidbody.velocity.x * 10f:0} km/h";
            var normalizedDistance = train.transform.position.x / destination.x;
            toCheonanText.text =
                $"천안까지 {Mathf.Clamp(100 - 100 * normalizedDistance, 0f, 100f):0.0} km";
            var hundredPercent = _trainRigidbody.velocity.x * 10f / 300f;
            powerLine.DOFade(hundredPercent, 0f);
            speedGauge.fillAmount = hundredPercent;
            destinationSlider.value = normalizedDistance;

            if (train.transform.position.x >= 480)
            {
                _trainRigidbody.freezeRotation = false;
            }
        }

        feverGaugeImage.fillAmount = train.ChargedTime / (train.IsBoosting ? 2f : 5f);
    }

    private void OnClear()
    {
        Destroy(train);
        IsStarted = false;
        powerLine.DOFade(0f, .5f);
        gameClearUI.SetActive(true);
        clearText.text = $"축하합니다!\n{_totalTime:0.0초}의 시간에 걸쳐 천안아산역에 도착하셨습니다!";
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
        gameOverUI.SetActive(true);
    }

    public void OnBoost()
    {
        powerLine.color = Color.red;
    }

    public void DisableBoost()
    {
        powerLine.color = _originLineColor;
    }
}
