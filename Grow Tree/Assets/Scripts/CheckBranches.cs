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
            gameObject.GetComponent<Animator>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }

    public void StartWait()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }
}
