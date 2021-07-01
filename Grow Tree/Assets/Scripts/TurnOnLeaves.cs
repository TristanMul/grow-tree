using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLeaves : MonoBehaviour
{
    [SerializeField] private List<GameObject> leaves;
    /*private List<int> chosenNumbers;
    [SerializeField] int minLeafCount, maxLeafCount;*/
    int chosenNumber;
    void Start()
    {
        chosenNumber = Random.Range(0, 3);
        leaves[chosenNumber].GetComponent<MeshRenderer>().enabled = false;
/*        foreach(GameObject leaf in leaves)
        {
            leaf.GetComponent<MeshRenderer>().enabled = false;
        }
        int amountOfLeaves = Random.Range(minLeafCount, maxLeafCount);
        Debug.Log("Leaves activated: " + amountOfLeaves);
        for (int i = 0; i < amountOfLeaves; i++)
        {
            chooseNumber();
        }
        for(int i = 0; i< amountOfLeaves; i++)
        {
            leaves[chosenNumbers[i]].GetComponent<MeshRenderer>().enabled = true;
        }
    }
    void chooseNumber()
    {
        int chosenNumber = Random.Range(0, leaves.Count - 1);

        if (chosenNumbers.Count > 0 || chosenNumbers.Contains(chosenNumber))
        {
            chooseNumber();
        }
        else
        {
            chosenNumbers.Add(chosenNumber);
            Debug.Log("Adding: " + chosenNumber);
        }*/
    }
}
