using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int chunkSizeX = 0;
    public int chunkSizeY = 0;
    public int chunkSizeZ = 0;
    
    public Material cubeMaterial;
    public Block[,,] chunkData;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
        chunkData = new Block[sizeX, sizeY, sizeZ];
        
        //Create the blocks
        for (int zIndex = 0; zIndex < sizeZ; zIndex++)
        {
            for (int yIndex = 0; yIndex < sizeY; yIndex++)
            {
                for (int xIndex = 0; xIndex < sizeX; xIndex++)
                {
                    Vector3 pos = new Vector3(xIndex, yIndex, zIndex);
                    chunkData[xIndex, yIndex, zIndex] = new Block(pos, this.gameObject, cubeMaterial);
                }
            }
        }
        
        //Draw the blocks
        for (int zIndex = 0; zIndex < sizeZ; zIndex++)
        {
            for (int yIndex = 0; yIndex < sizeY; yIndex++)
            {
                for (int xIndex = 0; xIndex < sizeX; xIndex++)
                {
                    chunkData[xIndex, yIndex, zIndex].Draw();
                    yield return null;
                }
            }
        }
        
        //Combine all the cubes into a single mesh
        CombineQuads();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildChunk(chunkSizeX, chunkSizeY, chunkSizeZ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void CombineQuads()
    {
        ///Combine all children meshes
		
        //Create arrays for the mesh filters and mesh instances that need to be combined
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] meshesToCombine = new CombineInstance[meshFilters.Length];
		
        for (int i = 0; i < meshFilters.Length; i++)
        {
            //Set the array to have the mesh values
            meshesToCombine[i].mesh = meshFilters[i].sharedMesh;
			
            //Convert local vertices co-ords to worldspace co-ords
            meshesToCombine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }
		
        //Create new mesh on the parent object
        MeshFilter mf = (MeshFilter) this.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();
		
        //Add combined meshes of the children to the parent's mesh
        mf.mesh.CombineMeshes(meshesToCombine);
		
        //Create renderer for the parent object (which is now all the combined quad meshes)
        MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = cubeMaterial;
		
        //delete all the original uncombined quads
        foreach (Transform quad in this.transform)
        {
            Destroy(quad.gameObject);
        }
    }
}
