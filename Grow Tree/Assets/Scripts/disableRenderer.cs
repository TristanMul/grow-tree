using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableRenderer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
