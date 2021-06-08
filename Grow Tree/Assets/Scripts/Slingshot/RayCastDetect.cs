using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray checkRay = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(checkRay, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward); 

            if (hit.collider.CompareTag("Tree"))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
