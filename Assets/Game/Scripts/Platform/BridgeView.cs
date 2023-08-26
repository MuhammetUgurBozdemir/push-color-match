using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BridgeView : MonoBehaviour
{
    public Renderer bridgeRenderer;
    
    private Color32 _color;
    private Color32 _color2;
    private int _index;

    private bool _isEntered;

    private static readonly int Color1 = Shader.PropertyToID("_Color");

    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }


    public void Init(Color32 color, Color32 color2 , int platformIndex)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        
        _color = color;
        _color2 = color2;
        _index = platformIndex;

        mpb.SetColor(Color1, color);

        bridgeRenderer.SetPropertyBlock(mpb);
    }

    public void LockBridge()
    {
        _isEntered = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_isEntered) return;
            
            _signalBus.Fire(new BridgeTriggeredSignal(_color2,_color, _index));
            Debug.Log("Triggered");
        }
    }

   
}