using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameEvent winGame;
    [SerializeField] private GameEvent loseGame;
    private GameObject rockformation;
    [SerializeField] private GameObject[] flowers;
    [SerializeField] Generator generator;
    [SerializeField] private int flowerRatio;
    [SerializeField] private int clusterMin = 5;
    [SerializeField] private int clusterMax = 3;
    [SerializeField] private float maxDeviation;
    bool coroutineActivated = false;
    private int growingBranches = 0;

    public void Start()
    {
        rockformation = GameObject.Find("Rock formation");
    }
    public void Test()
    {
        if (!coroutineActivated)
        {
            coroutineActivated = true;
            StartCoroutine(waitSeconds());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Branch"))
        {
<<<<<<< Updated upstream
        winGame.Raise();
        Highlighter.instance.ClearCircles();
        generator._timeBetweenIterations = 0.05f;
=======
            foreach (Transform children in rockformation.transform)
            {
                children.transform.GetChild(0).GetComponentInChildren<Collider>().enabled = false;
            }
            generator.BranchHitBarrier();
            winGame.Raise();
            generator._timeBetweenIterations = 0.05f;
>>>>>>> Stashed changes
        }
    }

    public IEnumerator CheckIfLost()
    {
        yield return new WaitForSeconds(0.5f);
        growingBranches = 0;
        foreach (Generator.Branch branch in generator._extremities)
        {
            if (branch._canGrow)
            {
                growingBranches++;
            }
        }
        Debug.Log(growingBranches);
        if (growingBranches == 0)
        {
            loseGame.Raise();
        }
    }
    public IEnumerator waitSeconds()
    {
<<<<<<< Updated upstream
=======
        yield return new WaitForSeconds(generator._timeBetweenIterations + 0.01f);
        Highlighter.instance.ClearCircles();
>>>>>>> Stashed changes
        yield return new WaitForSeconds(1f);
        generator.finishGrowing = true;
        StartCoroutine(growFlowers());
    }
    public IEnumerator growFlowers()
    {

        for (int i=/*generator._branches.Count-1*/generator._extremities.Count-1; i>=0; i--)
        {
<<<<<<< Updated upstream
            /*if(generator._branches[i]._children.Count == 0)
            {*/
=======
            if(generator._branches[i]._children.Count == 0 && generator._branches[i].canBloom)
            {
>>>>>>> Stashed changes
                int randomNumber = Random.Range(clusterMin, clusterMax);
                for (int j = 0; j < randomNumber; j++)
                {
                    int randomItem = Random.Range(0, flowers.Length);
                    float x = Random.Range(-maxDeviation, maxDeviation);
                    float y = Random.Range(-maxDeviation, maxDeviation);
                    Instantiate(flowers[randomItem], /*generator._branches[i]._end*/generator._extremities[i]._end + new Vector3(x, y, -0.5f), new Quaternion(0, 0, 0, 0));
                }
                yield return new WaitForSeconds(0.01f);
           /* }*/
        }
    }
}
