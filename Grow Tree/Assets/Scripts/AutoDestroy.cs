using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyWait());
    }

    IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
