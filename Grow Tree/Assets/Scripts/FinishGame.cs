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
    [SerializeField] private int clusterMax = 3;
    [SerializeField] private float maxDeviation;
    private int growingBranches = 0;
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Wingame raised");
        winGame.Raise();
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
            //Debug.Log("Losegame raised");
            loseGame.Raise();
        }
    }
    public void Tester()
    {
        StartCoroutine(growFlowers());
    }

    public IEnumerator growFlowers()
    {
        for(int i=generator._extremities.Count -1; i >= 0; i--)
        {
            int randomNumber = Random.Range(2, clusterMax);
            for(int j=0; j<randomNumber; j++)
            {
            int randomItem = Random.Range(0, flowers.Length);
            float x = Random.Range(-maxDeviation, maxDeviation);
            float y = Random.Range(-maxDeviation, maxDeviation);
            Instantiate(flowers[randomItem], generator._extremities[i]._end + new Vector3(x,y,-0.5f), new Quaternion(0, 0, 0, 0));
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
