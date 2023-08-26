using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class PlatformView : MonoBehaviour
{
    [SerializeField] private BridgeView leftBridge;  
    [SerializeField] private BridgeView rightBridge;


    [SerializeField] private Transform startPlane;
    [SerializeField] private Transform bridgeStartPos;

    public Vector3 bridgeStart => bridgeStartPos.position;
    public int bridgePartAmount;
    
    private int _index;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }


    public void InitPlatformView(PlatformData data, int pos)
    {
        _signalBus.Subscribe<BridgeTriggeredSignal>(LockBridge);
        
        transform.position = new Vector3(0,0,pos);

        _index = data.platformIndex;
        bridgePartAmount = data.bridgePartAmount;
        leftBridge.Init(data.secondGroupColor,data.firstGroupColor,_index);
        rightBridge.Init(data.firstGroupColor,data.secondGroupColor,_index);
    }

    private void LockBridge(BridgeTriggeredSignal signal)
    {
        if (signal.Index == _index)
        {
            leftBridge.LockBridge();
            rightBridge.LockBridge();
        }
    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<BridgeTriggeredSignal>(LockBridge);
        Destroy(gameObject);
    }
    
}

