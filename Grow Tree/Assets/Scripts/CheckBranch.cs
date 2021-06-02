using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBranch : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    RaycastHit hit;
    [SerializeField] private Camera camera;
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
            if(/*Physics.SphereCast(camera.transform.position,10f, camera.transform.forward, out hit)*/ Physics.Raycast(ray, out hit))
            {
                Debug.Log("hits");
                Vector3 hitPosition = hit.point;
            Instantiate(prefab,hitPosition,Quaternion.Euler(0,0,0));
                /*foreach(Generator.Branch branch in )
                {

                }*/


            }
        }
    }
}
