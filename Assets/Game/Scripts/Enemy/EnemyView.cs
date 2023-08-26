using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Zenject;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    [SerializeField] private AIPath _ai;

    public void SetColor(Color32 color)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor(Color1, color);

        renderer.SetPropertyBlock(mpb);
    }


    public void SetAIDisable()
    {
        _ai.enabled = false;
    }
    public void SetDestination(Transform target, bool canMove)
    {
        if (canMove)
        {
            _ai.canMove = true;
            _ai.destination = target.position;
            _ai.enabled = true;
            
        }
        else
        {
            _ai.destination = transform.position;
        }
    }
}