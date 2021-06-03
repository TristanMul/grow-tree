using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    RaycastHit hit;
    [SerializeField] private Camera camera;
    int test = 0;
    [SerializeField] Generator generator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckClosestBranch();
    }

    void CheckClosestBranch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (/*Physics.SphereCast(camera.transform.position,10f, camera.transform.forward, out hit)*/ Physics.Raycast(ray, out hit))
            {
                Vector3 hitPosition = hit.point;
                //Instantiate(prefab, hitPosition, Quaternion.Euler(0, 0, 0));
                if (hit.transform.gameObject.GetComponent<CapsuleCollider>())
                {
                    Debug.Log(int.Parse(hit.transform.gameObject.name));
                    test = int.Parse(hit.transform.gameObject.name) + 1000;
                   Debug.Log(test);
                    generator._branches[int.Parse(hit.transform.gameObject.name)]._children.Clear();
                }
                /*foreach(Generator.Branch branch in )
                {

                }*/


            }
        }
    }
}
