using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongShaderColor : MonoBehaviour
{
    Shader shader;
    // Start is called before the first frame update
    void Start()
    {
        shader = GetComponent<Renderer>().material.shader;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
