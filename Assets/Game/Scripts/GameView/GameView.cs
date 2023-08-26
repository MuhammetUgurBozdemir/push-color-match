using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject initialPanel;


    private SignalBus _signalBus;
    private LevelModel _levelModel;
    private ApplicationController _applicationController;

    private DynamicJoystick _dynamicJoystick;

    [Inject]
    private void Construct(SignalBus signalBus,
        LevelModel levelModel,
        ApplicationController applicationController,[Inject(Id = "Dynamic")] DynamicJoystick dynamicJoystick)
    {
        _signalBus = signalBus;
        _levelModel = levelModel;
        _dynamicJoystick = dynamicJoystick;
        _applicationController = applicationController;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<LevelEndSignal>(ShowPlayButton);
        _signalBus.Subscribe<LevelEndSignal>(Init);
        _signalBus.Subscribe<EnemyTouchedPlayerSignal>(Init);
        _signalBus.Subscribe<LeftHealthSignal>(UpdateLeftHealth);
        _signalBus.Subscribe<KilledEnemySignal>(Init);
        _signalBus.Subscribe<LevelFailedSignal>(ShowRetryButton);
        Init();
        
        healthText.text = "Remaining Health: " + 3;
        
    }

    private void Init()
    {
        levelText.text = "Level: " + (_levelModel.CurrentLevel + 1);
        scoreText.text = "Score: " + _levelModel.Score;
    }

    private void UpdateLeftHealth(LeftHealthSignal signal)
    {
        if(signal.LeftHealth<0) return;
        
        healthText.text = "Remaining Health: " + signal.LeftHealth.ToString();
    }

    private void ShowPlayButton()
    {
        nextLevelButton.gameObject.SetActive(true);
    }

    private void ShowRetryButton()
    {
        retryButton.gameObject.SetActive(true);
    }

    public void OnClickNextButton()
    {
        Time.timeScale = 1;
        _applicationController.InitNextLevel();
        nextLevelButton.gameObject.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        Time.timeScale = 1;
        playButton.gameObject.SetActive(false);
        _dynamicJoystick.gameObject.SetActive(true);
        initialPanel.SetActive(false);
    }

    public void OnClickRetryButton()
    {
        Time.timeScale = 1;
        _applicationController.Dispose();
        _applicationController.InitCurrentLevel().Forget();
        retryButton.gameObject.SetActive(false);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<LevelEndSignal>(ShowPlayButton);
        _signalBus.Unsubscribe<LevelEndSignal>(Init);
        _signalBus.Unsubscribe<EnemyTouchedPlayerSignal>(Init);
        _signalBus.Unsubscribe<LeftHealthSignal>(UpdateLeftHealth);
        _signalBus.Unsubscribe<KilledEnemySignal>(Init);
        _signalBus.Unsubscribe<LevelFailedSignal>(ShowRetryButton);
    }
}