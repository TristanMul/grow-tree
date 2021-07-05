using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] GameObject cutBranchPrefab;
    [SerializeField] GameObject leaves;
    FinishGame finish;
    public GameObject breakParticles;
    RaycastHit hit;
    Generator generator;
    bool routineActivated = false;
    List<Generator.Branch> cutOffBranches;


    private void OnEnable()
    {
        finish = GameObject.Find("Sun").GetComponent<FinishGame>();
        generator = GameObject.Find("Tree").GetComponent<Generator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slicer"))
        {
            GetComponent<Collider>().enabled = false;
            Generator.Branch cutOffBranch = generator._branches[int.Parse(transform.gameObject.name)];
            StartCoroutine(growLeaves());
            SliceOffBranch(cutOffBranch);
            DuplicateBranch(cutOffBranch);
            GameObject particles = Instantiate(breakParticles, gameObject.transform.position, Quaternion.identity);
            generator.maxBranchCount += cutOffBranches.Count;
            StartCoroutine(finish.CheckIfLost());
            cutOffBranches.Clear();
        }
    }

    public void CutBranch()
    {
        GetComponent<Collider>().enabled = false;

        Generator.Branch cutOffBranch = generator._branches[int.Parse(transform.gameObject.name)];
        SliceOffBranch(cutOffBranch);
        DuplicateBranch(cutOffBranch);

        cutOffBranches.Clear();
    }

    void SliceOffBranch(Generator.Branch cutOffBranch)
    {
        if (cutOffBranch == generator._firstBranch) return;

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

    void DuplicateBranch(Generator.Branch cutOffBranch)
    {
        GameObject newBranch = Instantiate(cutBranchPrefab, generator.transform.position, Quaternion.identity);
        newBranch.GetComponent<CreateCutBranch>().CreateMesh(cutOffBranches, cutOffBranch, generator);
    }
    public IEnumerator growLeaves()
    {
        if (!routineActivated)
        {
            routineActivated = true;
            GameObject Leaves = Instantiate(leaves, generator._branches[int.Parse(transform.gameObject.name)]._start, Quaternion.identity);
            Leaves.transform.Rotate(new Vector3(0, 180, -90));
            Debug.Log(routineActivated);
            yield return new WaitForSeconds(1f);
            Debug.Log("Waited");
            routineActivated = false;
        }
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
        branch.detached = true;
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
