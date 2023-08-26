using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Game.Scripts.Signals;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    #region Injection

    private ApplicationSettings _applicationSettings;

    [Inject]
    private void Construct(ApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings;
    }

    #endregion

    public override void InstallBindings()
    {
        GameSignalsInstaller.Install(Container);

        Container.Bind<LevelModel>().AsSingle();

        Container.BindInterfacesAndSelfTo<ApplicationController>().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyController>().AsSingle();


        InstallEnemies();
    }

    private void InstallEnemies()
    {
        Container.BindFactory<EnemyFacade.Args, EnemyFacade, EnemyFacade.Factory>()
            .FromPoolableMemoryPool<EnemyFacade.Args, EnemyFacade, EnemyFacade.Pool>(poolBinder => poolBinder
                .WithInitialSize(20)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<EnemyInstaller>(_applicationSettings.enemyFacade)
                .UnderTransformGroup("Enemies"));
    }
}