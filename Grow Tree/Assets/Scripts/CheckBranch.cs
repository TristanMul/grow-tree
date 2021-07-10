using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] GameObject cutBranchPrefab;
    [SerializeField] GameObject leaf;
    FinishGame finish;
    public GameObject breakParticles;
    RaycastHit hit;
    Generator generator;
    static Vector3 lastBranchCut;
    static bool routineActivated = false;
    public List<Generator.Branch> cutOffBranches = new List<Generator.Branch>();

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
            lastBranchCut = generator._branches[int.Parse(transform.gameObject.name)]._start;
            StartCoroutine(growLeaves(cutOffBranch));
            SliceOffBranch(cutOffBranch);
            DuplicateBranch(cutOffBranch);
            GameObject particles = Instantiate(breakParticles, gameObject.transform.position, Quaternion.identity);
            generator.maxBranchCount += cutOffBranches.Count;
            StartCoroutine(finish.CheckIfLost());
            //cutOffBranches.Clear();
        }
    }

    void SliceOffBranch(Generator.Branch cutOffBranch)
    {
        if (cutOffBranch == generator._firstBranch)
            return;
        cutOffBranch._parent._children.Remove(cutOffBranch);
        cutOffBranch._parent._canGrow = false;
        cutOffBranch._parent = null;
        cutOffBranches.Add(cutOffBranch);
        generator._capsules.Remove(generator._capsules[cutOffBranch._index]);
        generator._branches.Remove(cutOffBranch);
        ResetBranches();
        AddChildrenToList(cutOffBranch);
    }

    void DuplicateBranch(Generator.Branch cutOffBranch)
    {
        GameObject newBranch = Instantiate(cutBranchPrefab, generator.transform.position, Quaternion.identity) as GameObject;
        newBranch.GetComponent<CreateCutBranch>().CreateMesh(cutOffBranches, cutOffBranch, generator);
    }

    public IEnumerator growLeaves(Generator.Branch branch)
    {
        if (!routineActivated)
        {
            routineActivated = true;
            yield return new WaitForSeconds(0.15f);
            GameObject Leaf = Instantiate(leaf, lastBranchCut, Quaternion.identity) as GameObject;
            LeafManager.instance.leaves.Add(Leaf);
            Leaf.transform.Rotate(new Vector3(branch._direction.x, branch._direction.y, branch._direction.z + 180));
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
