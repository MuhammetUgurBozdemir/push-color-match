using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    
    

    public void SetColor(Color32 color)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor(Color1, color);

        renderer.SetPropertyBlock(mpb);
    }
}
