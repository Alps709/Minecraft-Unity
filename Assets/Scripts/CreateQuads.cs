using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuads : MonoBehaviour {

	public Material cubeMaterial;

	enum CubeSide
	{
		BOTTOM,
		TOP,
		LEFT,
		RIGHT,
		FRONT,
		BACK
	};
	
	void CreateQuad()
	{
		//Create the new mesh object to be used for our quad
		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh";

	    //Create the arrays for all the information required for the quad
		Vector3[] vertices = new Vector3[4];
		Vector3[] normals = new Vector3[4];
		Vector2[] uvs = new Vector2[4];
		int[] triangles = new int[6];

		//Create all needed UVs for the quad
		Vector2 uv00 = new Vector2( 0f, 0f );
		Vector2 uv10 = new Vector2( 1f, 0f );
		Vector2 uv01 = new Vector2( 0f, 1f );
		Vector2 uv11 = new Vector2( 1f, 1f );

		//Create all needed vertices for the quad
		Vector3 p0 = new Vector3( -0.5f,  -0.5f,  0.5f );
		Vector3 p1 = new Vector3(  0.5f,  -0.5f,  0.5f );
		Vector3 p2 = new Vector3(  0.5f,  -0.5f, -0.5f );
		Vector3 p3 = new Vector3( -0.5f,  -0.5f, -0.5f );		 
		Vector3 p4 = new Vector3( -0.5f,   0.5f,  0.5f );
		Vector3 p5 = new Vector3(  0.5f,   0.5f,  0.5f );
		Vector3 p6 = new Vector3(  0.5f,   0.5f, -0.5f );
		Vector3 p7 = new Vector3( -0.5f,   0.5f, -0.5f );

		
		//Put the created values in the arrays
		vertices = new Vector3[] {p4, p5, p1, p0};
		normals = new Vector3[] {Vector3.forward, 
								 Vector3.forward, 
								 Vector3.forward, 
								 Vector3.forward};
		
		//Put the created values in the arrays
		uvs = new Vector2[] {uv11, uv01, uv00, uv10};
		triangles = new int[] {3, 1, 0, 3, 2, 1};

		//Set the meshes values to the ones we just created
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		 
		//Recalculates the bounding box that encompasses the mesh,
		//it's a local spacebox that sits inline with the axis and is used for rendering
		mesh.RecalculateBounds();
		
		//Create a new quad object and parent that to our quad
		GameObject quad = new GameObject("quad");
	    quad.transform.parent = this.gameObject.transform;
	    
	    //Add new meshfilter to our created quad and sets its mesh to the one we just created
     	MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;
		
		//Add renderer component so it can be rendered, also set the material
		MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = cubeMaterial;
	}

	// Use this for initialization
	void Start () {
		CreateQuad();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
