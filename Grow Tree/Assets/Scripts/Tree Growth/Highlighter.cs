using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] GameObject highlightCircle;
    List<GameObject> circles = new List<GameObject>();
    Camera cam;
    public static Highlighter instance;
    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        Generator.instance.OnStartGrowing += ClearCircles;
    }


    public void AddCircleFromScreenPos(Vector3 position)
    {
        GameObject newCircle = Instantiate(highlightCircle, transform);
        newCircle.transform.position = position;
        circles.Add(newCircle);
    }

    public void AddCircleFromWorldPos(Vector3 position)
    {
        GameObject newCircle = Instantiate(highlightCircle, transform);
        newCircle.transform.position = Camera.main.WorldToScreenPoint(position);
        circles.Add(newCircle);
    }

    public void ClearCircles()
    {
        Debug.Log("clear circles");
        foreach(GameObject circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();
    }
}
