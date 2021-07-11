using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLeaves : MonoBehaviour
{
    [SerializeField] private List<GameObject> leaves;

    void Start()
    {
        int rdm = Random.Range(1, leaves.Count);
        for (int i = 0; i < rdm; i++)
        {
            int chosenNumber = Random.Range(0, leaves.Count);
            leaves[chosenNumber].GetComponent<MeshRenderer>().enabled = false;
            leaves[chosenNumber].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            leaves.Remove(leaves[chosenNumber].gameObject);
        }
    }
}
