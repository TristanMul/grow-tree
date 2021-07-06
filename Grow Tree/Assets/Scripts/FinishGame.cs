using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameEvent winGame;
    [SerializeField] private GameEvent loseGame;
    [SerializeField] private GameObject[] flowers;
    [SerializeField] private GameObject[] leaves;
    private GameObject rockformation;
    [SerializeField] Generator generator;
    [SerializeField] private int flowerRatio;
    [SerializeField] private int clusterMin = 5;
    [SerializeField] private int clusterMax = 3;
    [SerializeField] private float maxDeviation;
    [SerializeField] private float maxAngle;
    public List<Vector3> attractorsInRange;
    bool coroutineActivated = false;
    bool wonGame = false;
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
        if (other.CompareTag("Branch") && !wonGame)
        {
            foreach (Transform children in rockformation.transform)
            {
                children.transform.GetChild(0).GetComponentInChildren<Collider>().enabled = false;
            }
            generator._attractors = attractorsInRange;
            wonGame = true;
            generator._killRange = 0.1f;
            //generator._invertGrowth = 1.6f;
            generator.alternate = false;
            winGame.Raise();
            generator._timeBetweenIterations = 0.05f;
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
        if ((growingBranches == 0 && Highlighter.instance.highlights.Count == 0))
        {
            yield return new WaitForSeconds(0.5f);
            if (growingBranches == 0 && !wonGame)
            {
                loseGame.Raise();
            }
        }
    }

    public IEnumerator waitSeconds()
    {
        yield return new WaitForSeconds(generator._timeBetweenIterations + 0.01f);
        Highlighter.instance.ClearCircles();
        yield return new WaitForSeconds(3f);
        generator.finishGrowing = true;
        StartCoroutine(growFlowers());
    }

    public IEnumerator growFlowers()
    {

        for (int i = generator._branches.Count - 1; i >= 0; i--)
        {
            if (generator._branches[i]._children.Count == 0 && generator._branches[i].canBloom)
            {
                int randomLeaves = Random.Range(0, leaves.Length -1);
                GameObject BunchOfLeaves;
                BunchOfLeaves = Instantiate(leaves[randomLeaves], generator._branches[i]._start, Quaternion.LookRotation(generator._branches[i]._direction));
                BunchOfLeaves.transform.Rotate(0, -90, 90);
                int randomNumber = Random.Range(clusterMin, clusterMax);
                for (int j = 0; j < randomNumber; j++)
                {
                    int randomItem = Random.Range(0, flowers.Length);
                    float x = Random.Range(-maxDeviation, maxDeviation);
                    float xAngle = Random.Range(-maxAngle, maxAngle);
                    float yAngle = Random.Range(-maxAngle, maxAngle);
                    float y = Random.Range(-maxDeviation, maxDeviation);
                    float randomSize = Random.Range(0.5f, 1f);
                    GameObject GeneratedFlower;
                    GeneratedFlower = Instantiate(flowers[randomItem], generator._branches[i]._end + new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
                    GeneratedFlower.transform.Rotate(xAngle, yAngle, 0);
                    GeneratedFlower.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                    yield return null/*new WaitForSeconds(0.01f / randomNumber)*/;
                }
            }
        }
    }
    public IEnumerator growLeaves()
    {
        for (int i = generator._branches.Count - 1; i >= 0; i--)
        {
            if (generator._branches[i]._children.Count == 0 && generator._branches[i].canBloom)
            {

                int randomNumber = Random.Range(clusterMin, clusterMax);
                for (int j = 0; j < randomNumber; j++)
                {
                    int randomItem = Random.Range(0, flowers.Length);
                    float x = Random.Range(-maxDeviation, maxDeviation);
                    float xAngle = Random.Range(-maxAngle, maxAngle);
                    float yAngle = Random.Range(-maxAngle, maxAngle);
                    float y = Random.Range(-maxDeviation, maxDeviation);
                    Instantiate(leaves[randomItem], generator._branches[i]._end + new Vector3(x, y, -0.5f), new Quaternion(xAngle, yAngle, 0, 0));
                    yield return new WaitForSeconds(randomNumber);
                }
            }
        }
    }
}
