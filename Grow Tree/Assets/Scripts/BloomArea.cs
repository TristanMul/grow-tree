using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch"))
        {
            Debug.Log("Setting true");
            Generator.instance._branches[int.Parse(other.name)].canBloom = true;
        }
    }
}
