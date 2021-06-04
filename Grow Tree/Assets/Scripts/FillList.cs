using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillList : MonoBehaviour
{
    // Start is called before the first frame update
    private Generator _generator;

    void Start()
    {
        for (int i = 0; i < _generator._attractors.Count; i++)
        {
            Debug.Log(_generator._attractors[i]);
        }
        //_generator = GameObject.FindObjectsOfType<Generator>();
        //_generator.GetComponent<Generator>();
        //Debug.Log(_generator._attractors.Count);
        foreach (Transform t in transform)
        {
            _generator._attractors.Add(t.position);
         }
    }
}
