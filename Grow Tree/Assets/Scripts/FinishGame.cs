using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameEvent winGame;
    [SerializeField] private GameEvent loseGame;
    [SerializeField] private GameObject[] flowers;
    [SerializeField] Generator generator;
    [SerializeField] private int flowerRatio;
    [SerializeField] private int clusterMin = 5;
    [SerializeField] private int clusterMax = 3;
    [SerializeField] private float maxDeviation;
    bool coroutineActivated = false;
    private int growingBranches = 0;

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
<<<<<<< HEAD
            
            winGame.Raise();
            generator._timeBetweenIterations = 0.05f;
=======
        winGame.Raise();
        Highlighter.instance.ClearCircles();
        generator._timeBetweenIterations = 0.05f;
>>>>>>> parent of 92a0ab8 (Blooming changes)
        }
    }

    public IEnumerator CheckIfLost()
    {
        yield return new WaitForSeconds(generator._timeBetweenIterations + 0.01f);

        growingBranches = 0;
        foreach (Generator.Branch branch in generator._extremities)
        {
            if (branch._canGrow)
            {
                growingBranches++;
            }
        }

        if (growingBranches == 0 && Highlighter.instance.highlights.Count == 0 && winGame != null)
        {
            yield return new WaitForSeconds(0.5f);
            if (growingBranches == 0)
            {
                loseGame.Raise();
            }
        }
    }
    public IEnumerator waitSeconds()
    {
<<<<<<< HEAD
        yield return new WaitForSeconds(generator._timeBetweenIterations + 0.01f);
        Highlighter.instance.ClearCircles();
=======
        yield return new WaitForSeconds(1f);
>>>>>>> parent of 92a0ab8 (Blooming changes)
        generator.finishGrowing = true;
        StartCoroutine(growFlowers());
    }
    public IEnumerator growFlowers()
    {

        for (int i=generator._branches.Count-1; i>=0; i--)
        {
<<<<<<< HEAD
            if(generator._branches[i]._children.Count == 0)
            {
=======
            /*if(generator._branches[i]._children.Count == 0)
            {*/
>>>>>>> parent of 92a0ab8 (Blooming changes)
                int randomNumber = Random.Range(clusterMin, clusterMax);
                for (int j = 0; j < randomNumber; j++)
                {
                    int randomItem = Random.Range(0, flowers.Length);
                    float x = Random.Range(-maxDeviation, maxDeviation);
                    float y = Random.Range(-maxDeviation, maxDeviation);
                    Instantiate(flowers[randomItem], generator._branches[i]._end + new Vector3(x, y, -0.5f), new Quaternion(0, 0, 0, 0));
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
