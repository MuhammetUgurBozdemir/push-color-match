using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Scripts.Enemy
{
    public class EnemyFacade : MonoBehaviour, IInitializable, IPoolable<EnemyFacade.Args, IMemoryPool>
    {
        public int _index;
        private Color32 _color;
        private IMemoryPool _pool;
        private bool _isDespawned;

        [SerializeField] private Collider collider;
        [SerializeField] private Rigidbody rigidbody;

        [HideInInspector] public PlayerController playerController;

        #region Injection

        private EnemyStateManager _enemyStateManager;
        private EnemyView _enemyView;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(EnemyStateManager enemyStateManager,
            EnemyView enemyView,
            SignalBus signalBus)
        {
            _enemyStateManager = enemyStateManager;
            _enemyView = enemyView;
            _signalBus = signalBus;
        }

        #endregion

        public void Initialize()
        {
        }

        public void OnSpawned(Args args, IMemoryPool pool)
        {
            Idle();

            _enemyView.SetAIDisable();

            _isDespawned = false;

            _pool = pool;

            playerController = args.PlayerController;
            
            

            transform.position = new Vector3(Random.Range(-4, 5), 0f, args.InitialPos.z - Random.Range(-2, 3));

            _signalBus.Subscribe<BridgeTriggeredSignal>(SetStateFromSignal);
            _signalBus.Subscribe<LevelFailedSignal>(Idle);

            _enemyView.SetColor(args.Color);

            _index = args.PlatformIndex;
            _color = args.Color;

            collider.isTrigger = true;
        }

        public void Idle()
        {
            _enemyStateManager.CurrentState = EnemyState.Idle;
        }

        public void Attack()
        {
            _enemyStateManager.CurrentState = EnemyState.Attack;
        }

        private void SetStateFromSignal(BridgeTriggeredSignal signal)
        {
            var signalColor = ColorUtility.ToHtmlStringRGB(signal.Color);
            var color = ColorUtility.ToHtmlStringRGB(_color);

            if (signalColor == color && signal.Index == _index)
            {
                rigidbody.isKinematic = false;
                Move();
            }

            if (signal.Index > _index)
            {
                Death();
            }
        }

        public void Death()
        {
            _enemyStateManager.CurrentState = EnemyState.Death;
        }

        public void Move()
        {
            _enemyStateManager.CurrentState = EnemyState.MoveToTarget;
        }

        public void OnDespawned()
        {
            _signalBus.Unsubscribe<BridgeTriggeredSignal>(SetStateFromSignal);
            _signalBus.Unsubscribe<LevelFailedSignal>(Idle);

            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            SetCollisionState(false);
        }

        public void Despawn()
        {
            if (_isDespawned) return;
            _isDespawned = true;
            
            _pool.Despawn(this);
        }

        public void Dispose()
        {
            Despawn();
        }

        public void ExplodeIt()
        {
            rigidbody.AddExplosionForce(500, transform.position, 5);
            Death();
        }


        public void SetCollisionState(bool state)
        {
            collider.isTrigger = state;
        }
        public bool CheckCommonEnemy(List<EnemyFacade> enemyFacades)
        {
           var enemy = enemyFacades.Find(x => x._color.CheckColor() == _color.CheckColor()  && x._index == _index);
           return enemy;
        }

      

        public class Factory : PlaceholderFactory<Args, EnemyFacade>
        {
        }

        public class Pool : MonoPoolableMemoryPool<Args, IMemoryPool, EnemyFacade>
        {
        }

        public readonly struct Args
        {
            public readonly Vector3 InitialPos;
            public readonly Color32 Color;
            public readonly int PlatformIndex;
            public readonly PlayerController PlayerController;

            public Args(Vector3 initialPos, Color32 color, int platformIndex,
                PlayerController playerController) : this()
            {
                InitialPos = initialPos;
                Color = color;
                PlatformIndex = platformIndex;
                PlayerController = playerController;
            }
        }
    }
}