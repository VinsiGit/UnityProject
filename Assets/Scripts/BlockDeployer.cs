using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeployer : MonoBehaviour
{
    public GameObject objectToPlace; // The object you want to place
    public int numberOfObjects = 50; // The number of objects you want to place
    private Terrain terrain; // The terrain

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate a random x and z coordinate within the terrain boundaries
            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);

            // Get the y coordinate at the generated x and z coordinates
            float y = terrain.SampleHeight(new Vector3(x, 0, z));

            // Instantiate the object at the generated coordinates
            Instantiate(objectToPlace, new Vector3(x, y, z), Quaternion.identity);
        }
    }
}