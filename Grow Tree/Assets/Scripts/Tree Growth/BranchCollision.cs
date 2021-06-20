using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCollision : MonoBehaviour
{
    int index;
    Collider collider;
    GameObject pingPongBranch;
    FinishGame finish;

    // Start is called before the first frame update
    void Start()
    {
        finish = GameObject.Find("Sun").GetComponent<FinishGame>();
        index = int.Parse(gameObject.name);
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*Debug.Log(other.gameObject.tag);*/
        if (other.gameObject.tag == "Barrier")
        {
            Generator.instance.StopBranchGrowing(index);
            StopCoroutine(finish.CheckIfLost());
        }
    }
}
