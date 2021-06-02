using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSlicingLennart : MonoBehaviour
{
    Plane plane;

    [SerializeField] MeshFilter toCut;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane();
        plane.SetNormalAndPosition(transform.up, transform.position);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CutMesh();
        }
    }



    /// <summary>
    /// For now will remove the polygons that touch the plane
    /// </summary>
    void CutMesh()
    {
        Debug.Log("Shlashsh");

        bool[] pointAbovePlane = new bool[toCut.mesh.vertices.Length];

        for (int i = 0; i < toCut.mesh.vertices.Length; i++)
        {
            pointAbovePlane[i] = plane.GetSide(toCut.mesh.vertices[i] + toCut.transform.position);
        }
        Vector3[] oldVertices = toCut.mesh.vertices;
        int[] oldTriangles = toCut.mesh.triangles;

        List<int> newTriangles = new List<int>();
        List<int> slicedTriangles = new List<int>();
        List<Vector3> newVertices = new List<Vector3>();

        foreach (Vector3 item in oldVertices)
        {
            newVertices.Add(item);
        }

        for (int i = 0; i + 2 < toCut.mesh.triangles.Length; i += 3)
        {
            bool triangleInPlane = false;

            bool firstLineThroughPlane = false, secondLineThroughPlane = false, thirdLineThroughPlane = false;
            //Check wether lines go through the plane if they do the triangle goes through the plane
            if (pointAbovePlane[oldTriangles[i]] != pointAbovePlane[oldTriangles[i + 1]])
            {
                triangleInPlane = true;
                firstLineThroughPlane = true;
            }
            if (pointAbovePlane[oldTriangles[i]] != pointAbovePlane[oldTriangles[i + 2]])
            {
                triangleInPlane = true;
                secondLineThroughPlane = true;
            }
            if (pointAbovePlane[oldTriangles[i + 1]] != pointAbovePlane[oldTriangles[i + 2]])
            {
                triangleInPlane = true;
                thirdLineThroughPlane = true;
            }

            if (triangleInPlane)
            {
                //here will come the connecting triangle generation
                if (firstLineThroughPlane)
                {
                    //newVertices.Add();
                }
            }
            else
            {
                if (plane.GetSide(oldVertices[oldTriangles[i]] + toCut.transform.position))
                {
                    //adds the triangles to the sliced mesh
                    slicedTriangles.Add(toCut.mesh.triangles[i]);
                    slicedTriangles.Add(toCut.mesh.triangles[i + 1]);
                    slicedTriangles.Add(toCut.mesh.triangles[i + 2]);
                }
                else
                {
                    //readds the triangles that remain
                    newTriangles.Add(toCut.mesh.triangles[i]);
                    newTriangles.Add(toCut.mesh.triangles[i + 1]);
                    newTriangles.Add(toCut.mesh.triangles[i + 2]);
                }
            }
        }
        toCut.mesh.Clear();
        toCut.mesh.vertices = newVertices.ToArray();
        toCut.mesh.triangles = newTriangles.ToArray();
        toCut.mesh.RecalculateNormals();


        //Adds the sliced mesh to the world
        GameObject slicedObject = Instantiate(toCut.gameObject);//instantiates a copy of the object
        MeshFilter slicedMesh = slicedObject.GetComponent<MeshFilter>();//gets the meshfilter of the object
        slicedMesh.mesh.Clear();
        slicedMesh.mesh.vertices = oldVertices;//adds the same vertices to the mesh
        slicedMesh.mesh.triangles = slicedTriangles.ToArray();//Adds the triangles removed from the other object
        slicedMesh.mesh.RecalculateNormals();//makes sure the lighting works correctly
    }
}
