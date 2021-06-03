using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    RaycastHit hit;
    [SerializeField] private Camera camera;
    int test = 0;
    [SerializeField] Generator generator;

    List<Generator.Branch> cutOffBranches;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckClosestBranch();
    }

    void CheckClosestBranch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPosition = hit.point;
                if (hit.transform.gameObject.GetComponent<CapsuleCollider>())
                {
                    hit.transform.GetComponent<Collider>().enabled = false;
                    Generator.Branch cutOffBranch = generator._branches[int.Parse(hit.transform.gameObject.name)];
                    cutOffBranch._parent._children.Remove(cutOffBranch);
                    cutOffBranch._parent = null;
                    cutOffBranches = new List<Generator.Branch>();
                    cutOffBranches.Add(cutOffBranch);
                    generator._capsules.Remove(generator._capsules[cutOffBranch._index]);
                    generator._branches.Remove(cutOffBranch);
                    ResetBranches();
                    AddChildrenToList(cutOffBranch);

                    cutOffBranches.Clear();
                    


                }
            }
        }
    }

    private void AddChildrenToList(Generator.Branch branch){
        if(branch._children != null){
            for (int i = 0; i < branch._children.Count; i++)
            {
                Debug.Log("capsule count: " + generator._capsules.Count + " & " + "index: " + branch._children[i]._index + " & " + "indexcounter: " + generator.indexCounter);
                generator._capsules[branch._children[i]._index].transform.GetComponent<Collider>().enabled = false;
                generator._capsules.Remove(generator._capsules[branch._children[i]._index]);
                generator._branches.Remove(branch._children[i]);
                ResetBranches();
                cutOffBranches.Add(branch._children[i]);
                AddChildrenToList(branch._children[i]);
            }
        }
        else{
            generator._capsules[branch._index].transform.GetComponent<Collider>();
            generator._capsules.Remove(generator._capsules[branch._index]);
            generator._branches.Remove(branch);
            generator._extremities.Remove(branch);
            ResetBranches();
            cutOffBranches.Add(branch);
        }
    }

    private void ResetBranches(){
        generator.indexCounter = generator._branches.Count;

        for (int i = 0; i < generator._branches.Count; i++)
        {
            generator._branches[i]._index = i;
        }

        for (int i = 0; i < generator._capsules.Count; i++)
        {
            generator._capsules[i].gameObject.name = i.ToString();
        }
        
    }
}
