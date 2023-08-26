using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class EnemyStateManager: IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private EnemyState _currentState = EnemyState.Idle;
        private bool _isDisposed;

        public  EnemyState CurrentState
        {
            get => _currentState;
            set
            {
                if (_isDisposed || _currentState == value) return; 
                _states[(int)_currentState].OnExitState();
                _currentState = value;
                _states[(int)_currentState].OnEnterState();
            }
        }
        
        private readonly IEnemyState[] _states;

        public EnemyStateManager(EnemyIdleState enemyIdleStateStateIdle
            , EnemyStateAttack enemyStateAttack
            , EnemyStateDeath enemyStateDeath
            , EnemyStateFollow enemyStateFollow,
            EnemyStateMoveToTarget enemyStateMoveToTarget)
        {
            _states = new IEnemyState[5];
            _states[(int)EnemyState.Idle] = enemyIdleStateStateIdle;
            _states[(int)EnemyState.Attack] = enemyStateAttack;
            _states[(int)EnemyState.Death] = enemyStateDeath;
            _states[(int)EnemyState.Follow] = enemyStateFollow;
            _states[(int)EnemyState.MoveToTarget] = enemyStateMoveToTarget;
        }
        
        public void Initialize()
        {
            _states[(int)_currentState].OnEnterState();
        }
        
        public void Tick()
        {
            _states[(int)_currentState].Tick();
        }

        public void FixedTick()
        {
            _states[(int)_currentState].FixedTick();
        }

        public void Dispose()
        {
            _states[(int)_currentState].OnExitState();
            _isDisposed = true;
        }
    }
}
