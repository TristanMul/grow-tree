﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
public class CreateCutBranch : MonoBehaviour
{

    Generator.Branch branch;
    Generator generator;

    MeshFilter meshFilter;
    public void CreateMesh(Generator.Branch _branch, Generator _generator)
    {
        meshFilter = GetComponent<MeshFilter>();


        branch = _branch;
        generator = _generator;
        MakeMesh();
    }

    void MakeMesh()
    {

        float size = 0f;
        if (branch._children.Count == 0)
        {
            size = generator._extremitiesSize;
        }
        else
        {
            foreach (Generator.Branch bc in branch._children)
            {
                size += Mathf.Pow(bc._size, generator._invertGrowth);
            }
            size = Mathf.Pow(size, 1f / generator._invertGrowth);
        }
        branch._size = size;

        Vector3[] vertices = new Vector3[(branch._children.Count + 2) * generator._radialSubdivisions];
        int[] triangles = new int[branch._children.Count + 1 * generator._radialSubdivisions * 6];
        List<Generator.Branch> branchesToDraw = new List<Generator.Branch>();
        foreach (Generator.Branch br in branch._children)
        {
            branchesToDraw.Add(br);
        }
        branchesToDraw.Add(branch);

        int i = 0;
        foreach (Generator.Branch b in branchesToDraw)
        {
            // the index position of the vertices
            int vid = generator._radialSubdivisions * i;
            b._verticesId = vid;

            // quaternion to rotate the vertices along the branch direction
            Quaternion quat = Quaternion.FromToRotation(Vector3.up, b._direction);

            // construction of the vertices 
            for (int s = 0; s < generator._radialSubdivisions; s++)
            {
                // radial angle of the vertex
                float alpha = ((float)s / generator._radialSubdivisions) * Mathf.PI * 2f;

                // radius is hard-coded to 0.1f for now
                Vector3 pos = new Vector3(Mathf.Cos(alpha) * b._size, 0, Mathf.Sin(alpha) * b._size);
                pos = quat * pos; // rotation

                /*                // if the branch is an extremity, we have it growing slowly
                                if (b._children.Count == 0 && !b._grown)
                                {
                                    pos += b._start + (b._end - b._start) * _timeSinceLastIteration / _timeBetweenIterations;
                                }
                                else
                                {
                                    pos += b._end;
                                }*/

                vertices[vid + s] = pos - transform.position; // from tree object coordinates to [0; 0; 0]

                // if this is the tree root, vertices of the base are added at the end of the array 
                try
                {
                    if (b._parent == null)
                    {
                        vertices[generator._branches.Count * generator._radialSubdivisions + s] = b._start + new Vector3(Mathf.Cos(alpha) * b._size, 0, Mathf.Sin(alpha) * b._size) - transform.position;
                    }
                }
                catch(IndexOutOfRangeException exeption)
                {
                    Debug.LogError(exeption.Data);
                }
            }

            i++;
        }

        foreach (Generator.Branch b in branchesToDraw)
        {
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
            i++;
        }

        Mesh branchMesh = new Mesh();

        branchMesh.vertices = vertices;
        branchMesh.triangles = triangles;
        branchMesh.RecalculateNormals();
        meshFilter.mesh = branchMesh;
    }




}
