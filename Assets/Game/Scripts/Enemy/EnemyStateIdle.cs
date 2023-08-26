using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using UnityEngine;
using Zenject;

public class EnemyIdleState : IEnemyState
{
    private SignalBus _signalBus;
    private EnemyFacade _enemyFacade;
    [Inject]
    private void Construct(SignalBus signalBus,EnemyFacade enemyFacade)
    {
        _signalBus = signalBus;
        _enemyFacade = enemyFacade;
    }

    public void OnEnterState()
    {
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
