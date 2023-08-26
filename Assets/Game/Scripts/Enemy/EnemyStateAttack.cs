using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using UnityEngine;
using Zenject;

public class EnemyStateAttack : IEnemyState
{
    private SignalBus _signalBus;
    private EnemyFacade _enemyFacade;
    private EnemyView _enemyView;

    [Inject]
    private void Construct(SignalBus signalBus, EnemyFacade enemyFacade, EnemyView enemyView)
    {
        _signalBus = signalBus;
        _enemyFacade = enemyFacade;
        _enemyView = enemyView;
    }

    public void OnEnterState()
    {
        _signalBus.Fire(new EnemyTouchedPlayerSignal(_enemyFacade));
        _enemyFacade.Death();
    }

    public void OnExitState()
    {
      
    }

    public void Tick()
    {
       
    }

    public void FixedTick()
    {
    }
}