using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongShaderColor : MonoBehaviour
{
    [SerializeField] Shader normalShader;
    [SerializeField] Shader collidedShader;
    Generator generator;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<Generator>();
        renderer = GetComponent<Renderer>();

        generator.OnStopGrowing += SwitchToCollision;
        generator.OnStartGrowing += SwitchToNormal;
        SwitchToNormal();
        
    }

    void SwitchToNormal()
    {
        renderer.material.shader = normalShader;
    }

    void SwitchToCollision()
    {
        renderer.material.shader = collidedShader;
    }
}
