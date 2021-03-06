using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCollision : MonoBehaviour
{
    int index;
    Collider collider;
    FinishGame finish;
    [SerializeField] private GameEvent loseGame;

    // Start is called before the first frame update
    void Start()
    {
        finish = GameObject.Find("Sun").GetComponent<FinishGame>();
        index = int.Parse(gameObject.name);
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Barrier")
        {
            StopCoroutine(finish.CheckIfLost());
            int growingBranches = 0;
            foreach (Generator.Branch branch in Generator.instance._extremities)
            {
                if (branch._canGrow)
                {
                    growingBranches++;
                }
            }
            if(growingBranches == 1)
            {
                loseGame.Raise();
            }
            Generator.instance.StopBranchGrowing(index);
        }
    }
}
