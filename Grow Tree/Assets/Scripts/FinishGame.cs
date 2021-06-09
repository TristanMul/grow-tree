using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private GameEvent winGame;
    [SerializeField] private GameEvent loseGame;
    [SerializeField] Generator generator;
    private int growingBranches = 0;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wingame raised");
        winGame.Raise();
    }

    public IEnumerator CheckIfLost()
    {
        yield return new WaitForSeconds(0.5f);
            growingBranches = 0;
        foreach(Generator.Branch branch in generator._extremities)
        {
            Debug.Log("Checking branch");
            if (branch._canGrow)
            {
                Debug.Log("Branch can grow");
                growingBranches++;
            }
        }
        Debug.Log(growingBranches);
        if (growingBranches == 0)
        {
            Debug.Log("Losegame raised");
            loseGame.Raise();
        }
    }
}
