using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Enemy;
using UnityEngine;
using Zenject;


public class EnemyController : IInitializable
{
    private List<EnemyFacade> enemies = new List<EnemyFacade>();
    private List<PlatformView> _platformViews = new List<PlatformView>();

    #region Injection

    private readonly ApplicationSettings _settings;
    private readonly LevelModel _levelModel;
    private readonly LevelSettings _levelSettings;
    private readonly EnemyFacade.Factory _factory;
    private SignalBus _signalBus;

    public EnemyController(ApplicationSettings settings,
        LevelModel levelModel,
        LevelSettings levelSettings,
        EnemyFacade.Factory factory, SignalBus signalBus)
    {
        _settings = settings;
        _levelModel = levelModel;
        _levelSettings = levelSettings;
        _factory = factory;
        _signalBus = signalBus;
    }

    #endregion

    public void Initialize()
    {
    }

    public void Init(List<Vector3> platformPositions, PlayerController playerController,
        List<PlatformView> platformViews,LevelsData currentLevel)
    {
        _signalBus.Subscribe<KilledEnemySignal>(RemoveKilledEnemyAndCheckPlatformState);
        _signalBus.Subscribe<EnemyTouchedPlayerSignal>(RemoveTouchedEnemy);

        _platformViews = platformViews;

        foreach (PlatformData platformData in currentLevel.platformData)
        {
            var pos = platformPositions[platformData.platformIndex];

            for (int i = 0; i < platformData.firstGroupAmount; i++)
            {
                var args = new EnemyFacade.Args(pos, platformData.firstGroupColor, platformData.platformIndex,
                    playerController);
                var firstGroupOfEnemy = _factory.Create(args);
                enemies.Add(firstGroupOfEnemy);
            }

            for (int i = 0; i < platformData.secondGroupAmount; i++)
            {
                var args = new EnemyFacade.Args(pos, platformData.secondGroupColor, platformData.platformIndex,
                    playerController);
                var secondGroupOfEnemy = _factory.Create(args);
                enemies.Add(secondGroupOfEnemy);
            }
        }
    }

    private void RemoveKilledEnemyAndCheckPlatformState(KilledEnemySignal signal)
    {
        enemies.Remove(signal.EnemyFacade);

        var enemy = signal.EnemyFacade.CheckCommonEnemy(enemies);

        if (!enemy)
        {
            var enemyTransforms = enemies.Where(x => x._index == signal.EnemyFacade._index).ToList();
            BuildBridgeWithEnemies(enemyTransforms, signal.EnemyFacade._index).Forget();
        }
    }

    private void RemoveTouchedEnemy(EnemyTouchedPlayerSignal signal)
    {
        enemies.Remove(signal.EnemyFacade);

        var enemy = signal.EnemyFacade.CheckCommonEnemy(enemies);

        if (!enemy)
        {
            var enemyTransforms = enemies.Where(x => x._index == signal.EnemyFacade._index).ToList();
            BuildBridgeWithEnemies(enemyTransforms, signal.EnemyFacade._index).Forget();
        }
    }

    private async UniTask BuildBridgeWithEnemies(List<EnemyFacade> _enemies, int index)
    {
        if (index + 1 == _platformViews.Count)
        {
            _signalBus.Fire<LevelEndSignal>();
            Time.timeScale = 0;
            return;
        }

        var startPos = _platformViews[index].bridgeStart;

        foreach (EnemyFacade enemyFacade in _enemies)
        {
            enemyFacade.SetCollisionState(false);
            enemyFacade.transform.DOMove(startPos, 1);
            enemyFacade.transform.DORotate(new Vector3(0, 0, 90), 1);
            enemyFacade.transform.DOScale(new Vector3(1, 2, 1),1);
            startPos.z += .5f;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1.1f));


        if (_enemies.Count < _platformViews[index + 1].bridgePartAmount)
        {
            _signalBus.Fire<LevelFailedSignal>();
            Time.timeScale = 0;
        }
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<KilledEnemySignal>(RemoveKilledEnemyAndCheckPlatformState);
        _signalBus.Unsubscribe<EnemyTouchedPlayerSignal>(RemoveTouchedEnemy);

        foreach (EnemyFacade enemyFacade in enemies)
        {
            if (enemyFacade != null)
                enemyFacade.Dispose();
        }

        enemies.Clear();
    }
}