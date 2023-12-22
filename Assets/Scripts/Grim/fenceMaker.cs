using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class fenceMaker : MonoBehaviour
{
    public GameObject fence; // The object you want to place
    public int fencesPerSide = 50; // The number of objects you want to place
    private Terrain terrain; // The terrain

    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();

        // Place fences on the top border
        PlaceFencesOnBorder(terrain.transform.position, new Vector3(1, 0, 0), 0);

        // Place fences on the bottom border
        PlaceFencesOnBorder(terrain.transform.position + new Vector3(0, 0, terrain.terrainData.size.z), new Vector3(1, 0, 0), 0);

        // Place fences on the left border
        PlaceFencesOnBorder(terrain.transform.position, new Vector3(0, 0, 1), 90);

        // Place fences on the right border
        PlaceFencesOnBorder(terrain.transform.position + new Vector3(terrain.terrainData.size.x, 0, 0), new Vector3(0, 0, 1), 90);
    }

    // Function to place fences on a specific border
    void PlaceFencesOnBorder(Vector3 start, Vector3 direction, int rotate)
    {
        // Calculate the spacing between fences
        float spacing = terrain.terrainData.size.x / fencesPerSide;

        for (int i = 0; i < fencesPerSide; i++)
        {
            // Calculate the position along the line
            float t = i / (float)(fencesPerSide - 1);
            Vector3 position = start + new Vector3(t * terrain.terrainData.size.x * direction.x, 0, t * terrain.terrainData.size.x * direction.z);

            // Get the y coordinate at the generated x and z coordinates
            float y = terrain.SampleHeight(new Vector3(position.x, 0, position.z));

            // Instantiate the object at the generated coordinates with a 90-degree rotation around the y-axis
            Instantiate(fence, new Vector3(position.x, y, position.z), Quaternion.Euler(0, rotate, 0));
        }
    }
}