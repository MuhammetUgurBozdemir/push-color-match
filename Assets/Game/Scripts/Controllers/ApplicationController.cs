using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Game.Scripts.Player;
using UnityEngine;
using Zenject;

public class ApplicationController : IInitializable, IDisposable
{
    private List<Vector3> _platformPositions = new List<Vector3>();
    private List<PlatformView> _platforms = new List<PlatformView>();

    private PlayerController _playerController;
    private GameView _gameView;

    #region Injection

    private readonly ApplicationSettings _settings;
    private readonly LevelModel _levelModel;
    private readonly LevelSettings _levelSettings;
    private EnemyController _enemyController;
    private DiContainer _diContainer;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private SignalBus _signalBus;

    public ApplicationController(ApplicationSettings settings,
        LevelModel levelModel,
        LevelSettings levelSettings,
        DiContainer diContainer,
        EnemyController enemyController,
        [Inject(Id = "cinemachineVirtualCam")] CinemachineVirtualCamera cinemachineVirtualCamera,
        SignalBus signalBus)
    {
        _settings = settings;
        _levelModel = levelModel;
        _levelSettings = levelSettings;
        _diContainer = diContainer;
        _enemyController = enemyController;
        _cinemachineVirtualCamera = cinemachineVirtualCamera;
        _signalBus = signalBus;
    }

    #endregion


    public void Initialize()
    {
        _gameView = _diContainer.InstantiatePrefabForComponent<GameView>(_settings.gameView);
        _playerController = _diContainer.InstantiatePrefabForComponent<PlayerController>(_settings.playerMovement);
        _levelModel.LoadData();

        
        
        InitCurrentLevel().Forget();
    }

    private void InitCurrentLevelFromSignal()
    {
        Dispose();
        InitCurrentLevel().Forget();
    }

    public async UniTask InitCurrentLevel()
    {
        _gameView.Initialize();
        
        

        var currentLevel = _levelSettings.GetCurrentLevel(_levelModel.CurrentLevel);
        var pos = 0;
        var bridgeAmountPart = 0;
        
        _platformPositions.Clear();
        _platforms.Clear();

        foreach (PlatformData platformData in currentLevel.platformData)
        {
            var platform = _diContainer.InstantiatePrefabForComponent<PlatformView>(_settings.platformView);

            if (_platformPositions.Count > 0)
            {
                bridgeAmountPart =
                    currentLevel.platformData[platformData.platformIndex - 1].firstGroupAmount >
                    currentLevel.platformData[platformData.platformIndex - 1].secondGroupAmount
                        ? currentLevel.platformData[platformData.platformIndex - 1].firstGroupAmount
                        : currentLevel.platformData[platformData.platformIndex - 1].secondGroupAmount;

                platformData.bridgePartAmount = bridgeAmountPart;
            }

            pos = _platformPositions.Count > 0 ? pos + 25 + bridgeAmountPart / 2 : 0;
            _platformPositions.Add(new Vector3(0, 0, pos));
            
            if (!_platforms.Contains(platform))
                _platforms.Add(platform);

            platform.InitPlatformView(platformData, pos);
        }

        _playerController.Init(_levelSettings.playerSpawnPos);

        _cinemachineVirtualCamera.m_Follow = _playerController.transform;
        _cinemachineVirtualCamera.m_LookAt = _playerController.transform;

        _enemyController.Init(_platformPositions, _playerController, _platforms , currentLevel);

        await UniTask.Delay(TimeSpan.FromSeconds(.2f));

        AstarPath.active.Scan();
    }

    public Vector3 GetCurrentBridgeStartPos(int index)
    {
        return _platforms[index].bridgeStart;
    }

    public void InitNextLevel()
    {
        Dispose();
        _levelModel.CurrentLevel++;
        InitCurrentLevel().Forget();
    }

    public void Dispose()
    {
        foreach (var platformView in _platforms)
        {
            if (platformView != null)
                platformView.Dispose();
        }

        _platforms.Clear();
        _platformPositions.Clear();
        _enemyController.Dispose();
        _playerController.Dispose();
        _gameView.Dispose();
    }
}