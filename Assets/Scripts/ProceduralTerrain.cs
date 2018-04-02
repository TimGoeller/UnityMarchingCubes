using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Jobs;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour {
    
    private MeshFilter _filter;

    private Mesh _localMesh;

    private bool _jobStarted = false;

    private float[,,] _data;

    private const int Size = 40;

    private const float Threshold = 0.1f;

    private readonly FastNoise _noise = new FastNoise();
    
    TerrainGenerationJob Terragen = new TerrainGenerationJob();

    // Use this for initialization
    private void Start () {
        _localMesh = new Mesh();
        _filter = GetComponent<MeshFilter>();
        _data = new float[Size,Size,Size];
        //SetData();      
        
    }
	
	// Update is called once per frame
    private void Update () {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!_jobStarted)
            {
                RenderTerrain();
            }
            
        }
        if (Terragen != null)
        {
            if (Terragen.Update())
            {
               
                _jobStarted = false;
                _localMesh.Clear();
                _localMesh.vertices = Terragen.Output._newVertices.ToArray();
                _localMesh.triangles = Terragen.Output._newTriangles.ToArray();
                _localMesh.normals =  Terragen.Output._newNormals.ToArray();
                _filter.mesh = _localMesh;        
                GetComponent<MeshRenderer>().material = Resources.Load<Material>("New Material");
            }
        }
	}

    private void RenderTerrain()
    {
        Terragen = new TerrainGenerationJob();
        Terragen.Input = new TerrainGenerationJob.JobInput();
        Terragen.Input.Size = 50;
        Terragen.Input.Threshold = 0.1f;
        Terragen.Input.Seed = UnityEngine.Random.Range(0,10000);
        Terragen.Start();
        _jobStarted = true;
        
    }
}
