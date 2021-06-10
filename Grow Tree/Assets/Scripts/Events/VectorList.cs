using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VectorList : ScriptableObject
{
    private List<Vector3> objectList = new List<Vector3>();

    public List<Vector3> CurrentObjectList { get { return objectList; } }

    public void RegisterObject(Vector3 obj)
    {
        if (!objectList.Contains(obj))
            objectList.Add(obj);
    }

    public void UnregisterObject(Vector3 obj)
    {
        if (objectList.Contains(obj))
            objectList.Remove(obj);
    }

    public void ClearList()
    {
        objectList.Clear();
    }
}
