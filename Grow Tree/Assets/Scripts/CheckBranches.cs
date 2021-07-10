using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranches : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CutBranch"))
        {
            transform.parent = other.transform.parent;
            LeafManager.instance.leaves.Remove(this.gameObject);
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
