using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * @author Ciphered <https://ciphered.xyz
 * 
 * Generates a tree
 * Article link
 **/
public class Generator : MonoBehaviour
{

    /**
	 * Represents a branch 
	 **/
    public class Branch
    {
        public Vector3 _start;
        public Vector3 _end;
        public Vector3 _direction;
        public Branch _parent;
        public float _size;
        public float _lastSize;
        public List<Branch> _children = new List<Branch>();
        public List<Vector3> _attractors = new List<Vector3>();
        public int _verticesId; // the index of the vertices within the vertices array 
        public int _distanceFromRoot = 0;
        public bool _grown = false;
        public int _index;
        public bool _canGrow = true;
        public float _finalSize = 0f;
        public bool hitBarrier = false;
        public bool detached = false;
        public bool canBloom = false;

        public Branch(Vector3 start, Vector3 end, Vector3 direction, Branch parent = null)
        {
            _start = start;
            _end = end;
            _direction = direction;
            _parent = parent;
        }
    }

    public static Generator instance;

    [Header("Generation parameters")]
    public Vector3 _startPosition = new Vector3(0, 0, 0);
    [Range(0f, 0.5f)]
    public float _branchLength;
    [Range(0f, 1f)]
    public float _timeBetweenIterations = 0.5f;
    [Range(0f, 3f)]
    public float _attractionRange = 0.1f;
    [Range(0f, 2f)]
    public float _killRange = 0.5f;
    [Range(0f, 0.2f)]
    public float _randomGrowth = 0.1f;

    [Header("Mesh generation")]
    [Range(0, 20)]
    public int _radialSubdivisions = 10;
    [Range(0f, 1f), Tooltip("The size at the extremity of the branches")]
    public float _extremitiesSize = 0.05f;
    [Range(0f, 5f), Tooltip("Growth power, of the branches size")]
    public float _invertGrowth = 2f;
    public int indexCounter = 0;
    public int maxBranchCount;
    // the attractor points
    public List<Vector3> _attractors = new List<Vector3>();

    // a list of the active attractors 
    public List<int> _activeAttractors = new List<int>();

    // reference to the first branch 
    public Branch _firstBranch;

    // the branches 
    public List<Branch> _branches = new List<Branch>();
    public List<GameObject> _capsules = new List<GameObject>();

    // a list of the current extremities 
    public List<Branch> _extremities = new List<Branch>();
    public bool finishGrowing = false;
    public bool alternate;
    public bool started = false;
    public bool startedCamera = false;
    public bool movingCamera = false;
    public bool isSlicing = false;
    // the elpsed time since the last iteration, this is used for the purpose of animation
    float _timeSinceLastIteration = 0f;

    MeshFilter _filter;
    public Vector3[] currentVertices;

    //public List<Transform> customAtrractors = new List<Transform>();

    public GameObject otherAttractors;
    public GameObject CapsulesParent;
    //public List<Transform> otherAtrractorsList = new List<Transform>();

    [Header("Other")]
    [SerializeField] GameObject branchColliderPrefab;

    public VectorList attractorList;
    public List<Vector2> UVs = new List<Vector2>();

    //events
    public event Action OnStopGrowing;
    public event Action OnStartGrowing;

    //Rotate point
    GameObject rotatePoint;

    void Awake()
    {
        GenerateAttractors();
        // initilization 
        if (instance == null)
        {
            instance = this;
        }
        started = false;
    }

    void GenerateAttractors()
    {
        foreach (Transform t in otherAttractors.transform)
        {
            foreach (Transform c in t)
            {
                _attractors.Add(c.position);
            }
        }
    }

    /**
	 * Returns a 3D random vector of _randomGrowth magniture 
	 **/
    Vector3 RandomGrowthVector()
    {
        float alpha = UnityEngine.Random.Range(0f, Mathf.PI);
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        Vector3 pt = new Vector3(
            Mathf.Cos(theta) * Mathf.Sin(alpha),
            Mathf.Sin(theta) * Mathf.Sin(alpha),
            Mathf.Cos(alpha)
        );

        return pt * _randomGrowth;
    }

    // Start is called before the first frame update
    void Start()
    {
        _filter = GetComponent<MeshFilter>();
        // we generate the first branch 
        _firstBranch = new Branch(_startPosition, _startPosition + new Vector3(0, _branchLength, 0), new Vector3(0, 1, 0));
        _firstBranch._index = indexCounter;
        indexCounter++;
        _branches.Add(_firstBranch);
        _extremities.Add(_firstBranch);
        AddCapsule(_firstBranch);
        rotatePoint = new GameObject("Rotate point");
        rotatePoint.transform.position = _startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        branchHitBarrier = BranchHitBarrier();

        if (!branchHitBarrier && !finishGrowing && (_branches.Count < maxBranchCount || !alternate)&& !isSlicing && started && _attractors.Count > 0)
        {
            _timeSinceLastIteration += Time.deltaTime;

            // we check if we need to run a new iteration 
            if (_timeSinceLastIteration > _timeBetweenIterations)
            {

                _timeSinceLastIteration = 0f;

                // we parse the extremities to set them as grown 
                foreach (Branch b in _extremities)
                {
                    b._grown = true;
                }

                // we remove the attractors in kill range
                for (int i = _attractors.Count - 1; i >= 0; i--)
                {
                    foreach (Branch b in _branches)
                    {
                        if (Vector3.Distance(b._end, _attractors[i]) < _killRange)
                        {
                            _attractors.Remove(_attractors[i]);
                            break;
                        }
                    }
                }

                if (_attractors.Count > 0)
                {
                    // we clear the active attractors
                    _activeAttractors.Clear();
                    foreach (Branch b in _branches)
                    {
                        b._attractors.Clear();
                        attractorList.ClearList();
                    }

                    // each attractor is associated to its closest branch, if in attraction range
                    int ia = 0;
                    foreach (Vector3 attractor in _attractors)
                    {
                        float min = 999999f;
                        Branch closest = null; // will store the closest branch
                        foreach (Branch b in _branches)
                        {
                            float d = Vector3.Distance(b._end, attractor);
                            if (d < _attractionRange && d < min)
                            {

                                min = d;
                                closest = b;
                            }
                        }
                        // if a branch has been found, we add the attractor to the branch
                        if (closest != null)
                        {
                            if (!closest._canGrow)
                            {
                                for (int i = _attractors.Count - 1; i >= 0; i--)
                                {
                                    closest._attractors.Remove(_attractors[i]);
                                    attractorList.UnregisterObject(_attractors[i]);
                                }
                            }
                            else
                            {
                                closest._attractors.Add(attractor);
                                attractorList.RegisterObject(attractor);
                                _activeAttractors.Add(ia);
                            }
                        }
                        ia++;
                    }

                    // if at least an attraction point has been found, we want our tree to grow towards it
                    if (_activeAttractors.Count != 0)
                    {
                        // because new extremities will be set here, we clear the current ones
                        _extremities.Clear();
                        // new branches will be added here
                        List<Branch> newBranches = new List<Branch>();
                        foreach (Branch b in _branches)
                        {
                            // if the branch has attraction points, we grow towards them
                            if (b._attractors.Count > 0)
                            {
                                // we compute the direction of the new branch
                                Vector3 dir = new Vector3(0, 0, 0);
                                foreach (Vector3 attr in b._attractors)
                                {
                                    dir += (attr - b._end).normalized;
                                }
                                dir /= b._attractors.Count;
                                // random growth
                                dir += RandomGrowthVector();
                                dir.Normalize();

                                // our new branch grows in the correct direction
                                Branch nb;

                                if (b._children.Count > 0)
                                {
                                    nb = new Branch(b._end, b._end + dir * (_branchLength * .1f), dir, b);
                                }
                                else
                                {
                                    nb = new Branch(b._end, b._end + dir * _branchLength, dir, b);
                                }
                                nb._index = indexCounter;
                                indexCounter++;
                                //Debug.Log(indexCounter);
                                nb._distanceFromRoot = b._distanceFromRoot + 1;
                                b._children.Add(nb);
                                newBranches.Add(nb);
                                AddCapsule(nb);
                                _extremities.Add(nb);
                            }
                            else
                            {
                                // if no attraction points, we only check if the branch is an extremity
                                if (b._children.Count == 0 && b._canGrow)
                                {
                                    _extremities.Add(b);
                                }
                            }
                        }

                        // we merge the new branches with the previous ones
                        _branches.AddRange(newBranches);
                    }
                }
                else
                {
                    // we grow the extremities of the tree
                    for (int i = 0; i < _extremities.Count; i++)
                    {
                        Branch e = _extremities[i];
                        // the new branch starts where the extremity ends
                        Vector3 start = e._end;
                        // we add randomness to the direction
                        Vector3 dir = e._direction + RandomGrowthVector();
                        // we add the direction multiplied by the branch length to get the end point
                        Vector3 end;
                        end = e._end + dir * _branchLength;
                        // a new branch can be created with the same direction as its parent
                        if (e._canGrow)
                        {
                            Branch nb = new Branch(start, end, dir, e);
                            nb._index = indexCounter;
                            indexCounter++;
                            //Debug.Log(indexCounter);
                            // the current extrimity has a new child
                            e._children.Add(nb);

                            // let's add the new branch to the list and set it as the new extremity 
                            _branches.Add(nb);
                            AddCapsule(nb);
                            _extremities[i] = nb;
                        }
                    }
                }
            }

        }

        if (_extremities.Count >= 0 && !finishGrowing)
            ToMesh();
    }

    /**
	 * Creates a mesh from the branches list
	 **/
    void ToMesh()
    {
        Mesh treeMesh = new Mesh();

        // we first compute the size of each branch 
        for (int i = _branches.Count - 1; i >= 0; i--)
        {
            float size = 0f;
            Branch b = _branches[i];
            if (b._children.Count == 0 && b._canGrow)
            {
                size = _extremitiesSize;
            }
            else
            {
                foreach (Branch bc in b._children)
                {
                    size += Mathf.Pow(bc._size, _invertGrowth);
                }
                size = Mathf.Pow(size, 1f / _invertGrowth);
            }
            if (b._size < size)
            {
                b._size = size;
            }
        }

        //Debug.Log(_branches.Count);
        Vector3[] vertices = new Vector3[(_branches.Count + 1) * _radialSubdivisions];
        int[] triangles = new int[_branches.Count * _radialSubdivisions * 6];

        // construction of the vertices 
        for (int i = 0; i < _branches.Count; i++)
        {
            Branch b = _branches[i];

            // the index position of the vertices
            int vid = _radialSubdivisions * i;
            b._verticesId = vid;

            Quaternion quat;

            // quaternion to rotate the vertices along the branch direction
            quat = Quaternion.FromToRotation(Vector3.up, b._direction);

            // construction of the vertices 
            for (int s = 0; s < _radialSubdivisions; s++)
            {
                // radial angle of the vertex
                float alpha = ((float)s / _radialSubdivisions) * Mathf.PI * 2f;

                // radius is hard-coded to 0.1f for now
                Vector3 pos;
                pos = new Vector3(Mathf.Cos(alpha) * b._size, 0, Mathf.Sin(alpha) * b._size);
                
                pos = quat * pos; // rotation


                // if the branch is an extremity, we have it growing slowly
                if (b._children.Count == 0 && !b._grown)
                {
                    pos += b._start + (b._end - b._start) * _timeSinceLastIteration / _timeBetweenIterations;
                }
                else
                {
                    pos += b._end;
                }

                vertices[vid + s] = pos - transform.position; // from tree object coordinates to [0; 0; 0]

                // if this is the tree root, vertices of the base are added at the end of the array 
                if (b._parent == null)
                {
                    vertices[_branches.Count * _radialSubdivisions + s] = b._start + new Vector3(Mathf.Cos(alpha) * b._size, 0, Mathf.Sin(alpha) * b._size) - transform.position;
                }
            }
        }

        // faces construction; this is done in another loop because we need the parent vertices to be computed
        for (int i = 0; i < _branches.Count; i++)
        {
            Branch b = _branches[i];
            int fid = i * _radialSubdivisions * 2 * 3;
            // index of the bottom vertices 
            int bId = b._parent != null ? b._parent._verticesId : _branches.Count * _radialSubdivisions;
            // index of the top vertices 
            int tId = b._verticesId;

            // construction of the faces triangles
            for (int s = 0; s < _radialSubdivisions; s++)
            {
                // the triangles 
                triangles[fid + s * 6] = bId + s;
                triangles[fid + s * 6 + 1] = tId + s;
                if (s == _radialSubdivisions - 1)
                {
                    triangles[fid + s * 6 + 2] = tId;
                }
                else
                {
                    triangles[fid + s * 6 + 2] = tId + s + 1;
                }

                if (s == _radialSubdivisions - 1)
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
        treeMesh.vertices = vertices;
        currentVertices = vertices;
        treeMesh.triangles = triangles;
        treeMesh.RecalculateNormals();
        UVs.Clear();
        
        for (int i = 0; i < vertices.Length; i++)
        {
            UVs.Add(new Vector2(vertices[i].x - (int)vertices[i].x, vertices[i].y - (int)vertices[i].y));
        }
        treeMesh.SetUVs(0, UVs);

        _filter.mesh = treeMesh;

    }

    void OnDrawGizmos()
    {
        // we draw the branches 
        foreach (Branch b in _branches)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(b._start, b._end);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(b._end, 0.05f);
            Gizmos.DrawSphere(b._start, 0.05f);
        }
    }

    private void AddCapsule(Branch b)
    {
        GameObject newBranch = Instantiate(branchColliderPrefab);
        newBranch.transform.position = new Vector3((b._start.x + b._end.x) / 2, (b._start.y + b._end.y) / 2, (b._start.z + b._end.z) / 2);
        newBranch.transform.localScale = new Vector3(0.1f, Vector2.Distance(b._start, b._end) - 0.02f, 0.1f);
        newBranch.transform.up = b._direction.normalized;
        newBranch.name = b._index.ToString();
        _capsules.Add(newBranch);
        newBranch.transform.parent = CapsulesParent.transform;
    }

    public bool branchHitBarrier;
    /// <summary>
    /// Checks if any branches have hit a barrier
    /// </summary>
    /// <returns></returns>
    public bool BranchHitBarrier()
    {
        bool toReturn = false;
        foreach (Branch branch in _branches)
        {
            if (branch.hitBarrier)
            {
                toReturn = true;

            }
        }
        if (branchHitBarrier && !toReturn)
        {
            OnStartGrowing?.Invoke();
        }
        return toReturn;
    }

    public void StopBranchGrowing(int index)
    {
        _branches[index]._canGrow = false;
        _branches[index].hitBarrier = true;
        _branches[index]._finalSize = _branches[index]._size;
        OnStopGrowing?.Invoke();
        Highlighter.instance.AddCircleFromWorldPos(_branches[index]);
    }
}
