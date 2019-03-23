using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    public GameObject block;
    public int worldSize = 2;

    public IEnumerator BuildWorld()
    {
        //Loop that builds a giant cube of size worldSize from smaller cubes
        for (int zIndex = 0; zIndex < worldSize; zIndex++)
        {
            for (int yIndex = 0; yIndex < worldSize; yIndex++)
            {
                for (int xIndex = 0; xIndex < worldSize; xIndex++)
                {
                    //Creates new block in the world
                    Vector3 pos = new Vector3(xIndex, yIndex, zIndex);
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);
                    
                    //Gives block the name of its position in the world
                    cube.name = xIndex + " " + yIndex + " " + zIndex;
                    
                    //Sets all cubes to use the same instance of a material, so it isn't using a unique material for each one
                    cube.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
                }
                yield return null;
            }
        }
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
