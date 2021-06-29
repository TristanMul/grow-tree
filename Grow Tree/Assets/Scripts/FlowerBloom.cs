using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBloom : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    [SerializeField] private List<GameObject> petals;
    [SerializeField] private GameObject fallingLeaves;
    private void Awake()
    {
        int randomColor = Random.Range(0, colors.Count);
        foreach(GameObject petal in petals)
        {

        petal.GetComponent<SpriteRenderer>().color = colors[randomColor];
        }
       fallingLeaves.GetComponent<ParticleSystem>().startColor = colors[randomColor];
    }
}
