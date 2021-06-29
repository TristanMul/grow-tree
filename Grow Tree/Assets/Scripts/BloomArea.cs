using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch"))
        {            Generator.instance._branches[int.Parse(other.name)].canBloom = true;
        }
        if (other.CompareTag("Attractor"))
        {
            other.GetComponent<Attractor>().killable = false;
        }
    }
}
