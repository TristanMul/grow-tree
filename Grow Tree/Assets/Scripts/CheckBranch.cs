using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] private FinishGame loseGame;
    [SerializeField] private GameObject prefab;
    RaycastHit hit;
    [SerializeField] Generator generator;

    List<Generator.Branch> cutOffBranches;

    [SerializeField] GameObject cutBranchPrefab;
    [SerializeField] GameObject sliceTrailPrefab;
    GameObject sliceTrail;
    bool isSlicing = false;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Slicer")){
            GetComponent<Collider>().enabled = false;

            Generator.Branch cutOffBranch = generator._branches[int.Parse(transform.gameObject.name)];
            SliceOffBranch(cutOffBranch);
            DuplicateBranch(cutOffBranch);

            cutOffBranches.Clear();

        }
    }

    void SliceBranch()
    {
        
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPosition = hit.point;
            if (hit.transform.gameObject.GetComponent<CapsuleCollider>() && generator._branches[int.Parse(hit.transform.gameObject.name)] != generator._firstBranch)
            {
                hit.transform.GetComponent<Collider>().enabled = false;
                Generator.Branch cutOffBranch = generator._branches[int.Parse(hit.transform.gameObject.name)];
                SliceOffBranch(cutOffBranch);
                DuplicateBranch(cutOffBranch);

                cutOffBranches.Clear();
            }
        }
        
    }

    void StartSlicing(){
        isSlicing = true;
    }

    void StopSlicing(){
        isSlicing = false;
    }

    void SliceOffBranch(Generator.Branch cutOffBranch){
        cutOffBranch._parent._children.Remove(cutOffBranch);
        cutOffBranch._parent._canGrow = false;
        cutOffBranch._parent = null;

        cutOffBranches = new List<Generator.Branch>();
        cutOffBranches.Add(cutOffBranch);
        generator._capsules.Remove(generator._capsules[cutOffBranch._index]);
        generator._branches.Remove(cutOffBranch);

        ResetBranches();
        AddChildrenToList(cutOffBranch);
    }

    void DuplicateBranch(Generator.Branch cutOffBranch){
        GameObject newBranch = Instantiate(cutBranchPrefab, generator.transform.position, Quaternion.identity);
        newBranch.GetComponent<CreateCutBranch>().CreateMesh(cutOffBranches, cutOffBranch, generator);
    }

    private void AddChildrenToList(Generator.Branch branch)
    {
        if (branch._children != null)
        {
            for (int i = 0; i < branch._children.Count; i++)
            {
                generator._capsules[branch._children[i]._index].transform.GetComponent<Collider>().enabled = false;
                generator._capsules.Remove(generator._capsules[branch._children[i]._index]);
                generator._branches.Remove(branch._children[i]);
                ResetBranches();
                cutOffBranches.Add(branch._children[i]);
                AddChildrenToList(branch._children[i]);
            }
        }
        else
        {
            generator._capsules[branch._index].transform.GetComponent<Collider>();
            generator._capsules.Remove(generator._capsules[branch._index]);
            generator._branches.Remove(branch);
            generator._extremities.Remove(branch);
            ResetBranches();
            cutOffBranches.Add(branch);
        }
    }

    private void ResetBranches()
    {
        foreach (Generator.Branch b in generator._extremities)
        {
            b._grown = true;
        }
        generator._extremities.Clear();
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
