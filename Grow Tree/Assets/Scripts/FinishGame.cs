using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameEvent winGame;
    [SerializeField] private GameEvent loseGame;
    [SerializeField] private GameObject flower;
    [SerializeField] Generator generator;
    [SerializeField] private int flowerRatio;
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
        StartCoroutine(growExtremities());
    }

    public IEnumerator growExtremities()
    {
        foreach(Generator.Branch branch in generator._extremities)
        {
            Instantiate(flower, branch._end, new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
