using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public class HighLight
    {
        public Generator.Branch branch;
        public GameObject circleGameObject;

        public HighLight(Generator.Branch _target, GameObject _circle)
        {
            circleGameObject = _circle;
            branch = _target;
        }
    }


    [SerializeField] GameObject highlightCircle;
    [SerializeField] private GameObject tutorialSwipe;
    [HideInInspector] public List<HighLight> highlights = new List<HighLight>();
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

    public void AddCircleFromWorldPos(Generator.Branch branch)
    {
        Debug.Log("Adding circle");
        GameObject newCircle = Instantiate(highlightCircle, transform);
        newCircle.transform.position = Camera.main.WorldToScreenPoint(branch._start);
        HighLight h = new HighLight(branch, newCircle);
        highlights.Add(h);
    }

    public void ClearCircles()
    {
        Debug.Log("Clearing circles");
        foreach (HighLight h in highlights)
        {
            Destroy(h.circleGameObject);
        }
        highlights.Clear();
    }

    /// <summary>
    /// Checks if any of the circles need to be removed
    /// </summary>
    /// <param name="branches"></param>
    public void UpdateCircles()
    {
        for (int i = highlights.Count-1; i >= 0; i--)//Reverse for loop so you can remove stuff
        {
            if (highlights[i].branch.detached)
            {
                Destroy(highlights[i].circleGameObject);
                highlights.Remove(highlights[i]);
            }
        }
    }

    }