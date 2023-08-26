using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Player;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private int _leftHealtCount = 3;

    private SignalBus _signalBus;
    private LevelModel _levelModel;
    private ApplicationController _applicationController;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private PlayerView playerView;

    [Inject]
    private void Construct(SignalBus signalBus,
        LevelModel levelModel,
        ApplicationController applicationController,
        EnemyController enemyController)
    {
        _signalBus = signalBus;
        _levelModel = levelModel;
        _applicationController = applicationController;
    }


    public void Init(Vector3 spawnPos)
    {
        _signalBus.Subscribe<EnemyTouchedPlayerSignal>(GetHitFromEnemy);
        _signalBus.Subscribe<KilledEnemySignal>(CalculateScore);
        _signalBus.Subscribe<BridgeTriggeredSignal>(SetColorOfView);
        transform.position = spawnPos;
        _leftHealtCount = 3;
        
        playerView.SetColor(Color.white);
    }

    private void GetHitFromEnemy()
    {
        _leftHealtCount--;
        _signalBus.Fire(new LeftHealthSignal(_leftHealtCount));
        
        if (_leftHealtCount == 0)
        {
            _signalBus.Fire<LevelFailedSignal>();
        }
    }

    private void SetColorOfView(BridgeTriggeredSignal signal)
    {
        playerView.SetColor(signal.Color2);
    }

    private void CalculateScore()
    {
        _levelModel.Score++;
    }
    void Update()
    {
        if (transform.position.y < -2)
        {
            Time.timeScale = 0;
            _signalBus.Fire<LevelFailedSignal>();
        }
        
        if (!Input.GetMouseButtonUp(0)) return;
        
        Collider[] cols = Physics.OverlapSphere(transform.position, 5);

        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out Rigidbody rb))
            {
                if(rb == _rb) continue;
                rb.AddExplosionForce(3000, transform.position, 5);
            }
        }
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyTouchedPlayerSignal>(GetHitFromEnemy);
        _signalBus.Unsubscribe<KilledEnemySignal>(CalculateScore); 
        _signalBus.Unsubscribe<BridgeTriggeredSignal>(SetColorOfView);
    }
}