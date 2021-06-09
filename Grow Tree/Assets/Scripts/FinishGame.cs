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
        winGame.Raise();
    }

    public void CheckIfLost()
    {
            growingBranches = 0;
        foreach(Generator.Branch branch in generator._extremities)
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
}
