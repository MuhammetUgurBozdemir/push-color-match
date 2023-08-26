using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using UnityEngine;
using Zenject;

public class EnemyStateMoveToTarget : IEnemyState
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
         _enemyFacade.SetCollisionState(false);
    }

    public void OnExitState()
    {
        _enemyView.SetDestination(null,false);
    }

    public void Tick()
    {
        if (_enemyView.transform.position.y < -1)
        {
            _enemyFacade.Death();
            _signalBus.Fire(new KilledEnemySignal(_enemyFacade));
            return;
        }

        var distance = Vector3.Distance(_enemyView.transform.position, _enemyFacade.playerController.transform.position);
        if (distance < .7f)
        {
            _enemyFacade.Attack();
        }

        _enemyView.SetDestination(_enemyFacade.playerController.transform ,true);
    }

    public void FixedTick()
    {
         
    }
}