using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
public class CreateCutBranch : MonoBehaviour
{

    List<Generator.Branch> branchesToDraw;
    Generator generator;

    MeshFilter meshFilter;
    Generator.Branch branch;
    Rigidbody rb;
    float angularVelRange = .25f;
    public void CreateMesh(List<Generator.Branch> Branches, Generator.Branch cutOffBranch, Generator _generator)
    {

        branchesToDraw = null;
        meshFilter = GetComponent<MeshFilter>();
        branch = cutOffBranch;
        cutOffBranch._parent = null;
        branchesToDraw = Branches;
        generator = _generator;
        MakeMesh();

        rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.angularVelocity = (new Vector3(0f, 0f, UnityEngine.Random.Range(-angularVelRange, angularVelRange)));
            rb.velocity = new Vector3(0f, 1f, 0f);
        }
        Highlighter.instance.UpdateCircles();

        //UnityEngine.Random.Range(-angularVelRange, angularVelRange)
    }

    void MakeMesh()
    {
        Vector3[] vertices;
        int[] triangles = new int[(branchesToDraw.Count + 1) * generator._radialSubdivisions * 6];


        vertices = generator.currentVertices;



        // faces construction; this is done in another loop because we need the parent vertices to be computed
        for (int i = 1; i < branchesToDraw.Count; i++)
        {
            Generator.Branch b = branchesToDraw[i];
            int fid = i * generator._radialSubdivisions * 2 * 3;
            // index of the bottom vertices 
            int bId = b._parent != null ? b._parent._verticesId : generator._branches.Count * generator._radialSubdivisions;
            // index of the top vertices 
            int tId = b._verticesId;

            // construction of the faces triangles
            for (int s = 0; s < generator._radialSubdivisions; s++)
            {
                // the triangles 
                triangles[fid + s * 6] = bId + s;
                triangles[fid + s * 6 + 1] = tId + s;
                if (s == generator._radialSubdivisions - 1)
                {
                    triangles[fid + s * 6 + 2] = tId;
                }
                else
                {
                    triangles[fid + s * 6 + 2] = tId + s + 1;
                }

                if (s == generator._radialSubdivisions - 1)
                {
                    // if last subdivision
                    triangles[fid + s * 6 + 3] = bId + s;
                    triangles[fid + s * 6 + 4] = tId;
                    triangles[fid + s * 6 + 5] = bId;
                }
                else
                {
                    triangles[fid + s * 6 + 3] = bId + s;
                    triangles[fid + s * 6 + 4] = tId + s + 1;
                    triangles[fid + s * 6 + 5] = bId + s + 1;
                }
            }
        }

        /*        for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] -= branch._start;
                }
                transform.position += branch._start;*/
        Mesh branchMesh = new Mesh();

        branchMesh.vertices = vertices;
        branchMesh.triangles = triangles;
        branchMesh.RecalculateNormals();
        meshFilter.mesh = branchMesh;
    }




}
