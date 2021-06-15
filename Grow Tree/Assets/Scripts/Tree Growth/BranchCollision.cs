using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCollision : MonoBehaviour
{
    int index;
    Collider collider;
    GameObject pingPongBranch;
    // Start is called before the first frame update
    void Start()
    {
        index = int.Parse(gameObject.name);
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
/*        Debug.Log(other.gameObject.tag);*/
        if(other.gameObject.tag == "Barrier")
        {
            Generator.instance.StopBranchGrowing(index);


            if (pingPongBranch)
            {
                GameObject newBranch = Instantiate(pingPongBranch);
                CreateCutBranch cutBranch = newBranch.GetComponent<CreateCutBranch>();
                //cutBranch.CreateMesh();
            }
        }
    }
}
