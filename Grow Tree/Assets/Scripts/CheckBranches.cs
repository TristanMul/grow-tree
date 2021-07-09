using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranches : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Colliding");
        if (other.CompareTag("Branch"))
        {
            Debug.Log("Check if branch");
            
//           Debug.Log("Getting checkbranch: " + other.GetComponent<CheckBranch>());
            Debug.Log("Getting list: " + other.GetComponent<CheckBranch>().cutOffBranches.Count);
            if (other.gameObject.GetComponent<CheckBranch>().cutOffBranches.Contains(Generator.instance._branches[int.Parse(other.transform.gameObject.name)]))
            {
                Debug.Log("Adding to list");
                other.GetComponent<CheckBranch>().addedLeaves.Add(this.gameObject);
                LeafManager.instance.leaves.Remove(this.gameObject);
        }
        other.GetComponent<CheckBranch>().cutOffBranches.Clear();
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
