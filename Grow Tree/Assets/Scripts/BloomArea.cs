using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomArea : MonoBehaviour
{
    FinishGame finish;

    private void Start()
    {
        finish = GameObject.Find("Sun").GetComponent<FinishGame>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch"))
        {
            Generator.instance._branches[int.Parse(other.name)].canBloom = true;
        }
        if (other.CompareTag("Attractor"))
        {
            Debug.Log("adding attractor");
            finish.attractorsInRange.Add(other.transform.position);
        }
    }
}
