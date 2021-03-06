﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    enum CubeSide
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        FRONT,
        BACK
    };

    public bool isSolid;
    private GameObject parent;
    private Vector3 position;
    private Material cubeMaterial;

    public Block(Vector3 pos, GameObject parent, Material material)
    {
	    isSolid = true;
        position = pos;
        this.parent = parent;
        cubeMaterial = material;
    }
    
    void CreateQuad(CubeSide side)
	{
		//Create the new mesh object to be used for our quad
		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh" + side.ToString();

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

		//Create all possible vertices for the quad (it will make 8 vertices (one for each corner of a cube), that multiple quads will use, to combine and make a cube)
		Vector3 p0 = new Vector3( -0.5f,  -0.5f,  0.5f );
		Vector3 p1 = new Vector3(  0.5f,  -0.5f,  0.5f );
		Vector3 p2 = new Vector3(  0.5f,  -0.5f, -0.5f );
		Vector3 p3 = new Vector3( -0.5f,  -0.5f, -0.5f );		 
		Vector3 p4 = new Vector3( -0.5f,   0.5f,  0.5f );
		Vector3 p5 = new Vector3(  0.5f,   0.5f,  0.5f );
		Vector3 p6 = new Vector3(  0.5f,   0.5f, -0.5f );
		Vector3 p7 = new Vector3( -0.5f,   0.5f, -0.5f );

		//Switch that chooses which quad it needs to make
		switch(side)
        		{
        			case CubeSide.TOP:
        				vertices = new Vector3[] {p7, p6, p5, p4};
        				normals = new Vector3[] {Vector3.up, Vector3.up, 
        											Vector3.up, Vector3.up};
        				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
        				triangles = new int[] {3, 1, 0, 3, 2, 1};
        			break;
        			
                    case CubeSide.BOTTOM:
	                    vertices = new Vector3[] {p0, p1, p2, p3};
	                    normals = new Vector3[] {Vector3.down, Vector3.down, 
		                    Vector3.down, Vector3.down};
	                    uvs = new Vector2[] {uv11, uv01, uv00, uv10};
	                    triangles = new int[] { 3, 1, 0, 3, 2, 1};
	                break;
                    
        			case CubeSide.LEFT:
        				vertices = new Vector3[] {p7, p4, p0, p3};
        				normals = new Vector3[] {Vector3.left, Vector3.left, 
        											Vector3.left, Vector3.left};
        				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
        				triangles = new int[] {3, 1, 0, 3, 2, 1};
        			break;
        			
        			case CubeSide.RIGHT:
        				vertices = new Vector3[] {p5, p6, p2, p1};
        				normals = new Vector3[] {Vector3.right, Vector3.right, 
        											Vector3.right, Vector3.right};
        				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
        				triangles = new int[] {3, 1, 0, 3, 2, 1};
        			break;
        			
        			case CubeSide.FRONT:
        				vertices = new Vector3[] {p4, p5, p1, p0};
        				normals = new Vector3[] {Vector3.forward, Vector3.forward, 
        											Vector3.forward, Vector3.forward};
        				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
        				triangles = new int[] {3, 1, 0, 3, 2, 1};
        			break;
        			
        			case CubeSide.BACK:
        				vertices = new Vector3[] {p6, p7, p3, p2};
        				normals = new Vector3[] {Vector3.back, Vector3.back, 
        											Vector3.back, Vector3.back};
        				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
        				triangles = new int[] {3, 1, 0, 3, 2, 1};
        			break;
        		}

		//Set the meshes values to the ones we just created
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		 
		//Recalculates the bounding box that encompasses the mesh,
		//it's a local spacebox that sits inline with the axis and is used for rendering
		mesh.RecalculateBounds();
		
		//Create a new quad object and parent that to our quad
		GameObject quad = new GameObject("Quad");
		quad.transform.position = position;
	    quad.transform.parent = parent.transform;
	    
	    //Add new meshfilter to our created quad and sets its mesh to the one we just created
     	MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;
		
		//Add renderer component so it can be rendered, also set the material
		MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = cubeMaterial;
	}

    public bool HasSolidNeighbour(int neighbourX, int neighbourY, int neighbourZ)
    {
	    Block[,,] chunk = parent.GetComponent<Chunk>().chunkData;
	
	    //Checks whether it can find a neighbour
	    try
	    {
		    //Found a neighbour
		    return chunk[neighbourX, neighbourY, neighbourZ].isSolid;
	    }
	    catch(System.IndexOutOfRangeException ex){}

	    //Couldn't find a neighbour
	    return false;
    }

    public void Draw()
    {
	    //Do all checks for non-solid neighbours and draw the quads that have them
	    if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
	    {
		    CreateQuad(CubeSide.FRONT);   
	    }
	    
	    if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
	    {
		    CreateQuad(CubeSide.BACK);   
	    }
	    
	    if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
	    {
		    CreateQuad(CubeSide.TOP);   
	    }
	    
	    if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
	    {
		    CreateQuad(CubeSide.BOTTOM);   
	    }
	    
	    if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
	    {
		    CreateQuad(CubeSide.LEFT);   
	    }
	    
	    if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
	    {
		    CreateQuad(CubeSide.RIGHT);   
	    }
    }
}
