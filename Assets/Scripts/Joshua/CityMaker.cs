using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.UI;
public class CityMaker : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs
    public GameObject roadPrefab;
    public GameObject intersectionPrefab;
    public GameObject playerPrefab;
    public GameObject pickupPrefab;

    public int numberOfPickups = 5;
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;

    public GameObject openSpacePrefab;
    public int cityWidth = 10;
    public int cityLength = 10;
    public int roadFrequencyX = 5; // Frequency of roads in the X direction
    public int roadFrequencyZ = 5; // Frequency of roads in the Z direction
    private List<Vector3> roadPositions = new List<Vector3>();
    private bool[,] roadPositionsBool;
    void Start()
    {
        roadPositionsBool = new bool[cityWidth, cityLength];

        GenerateCity();

        GeneratePlayer();
        GenerateEnemies();
        GeneratePickups();
    }

    void GenerateCity()
    {
        GenerateRoads();
        GetComponent<NavMeshSurface>().BuildNavMesh();
        GenerateBuildings();
    }

    void GenerateRoads()
    {
        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityLength; z++)
            {
                Vector3 position = new Vector3(x, 0, z) * 10.0f;
                GameObject instance;

                if (x % roadFrequencyX == 0 && z % roadFrequencyZ == 0)
                {
                    Instantiate(intersectionPrefab, position, Quaternion.identity);
                    roadPositions.Add(position);
                    roadPositionsBool[x, z] = true;
                }
                else if (x % roadFrequencyX == 0 || z % roadFrequencyZ == 0)
                {
                    instance = Instantiate(roadPrefab, position, Quaternion.identity);
                    if (z % roadFrequencyZ == 0)
                    {
                        instance.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    roadPositions.Add(position);
                    roadPositionsBool[x, z] = true;
                }
                else
                { roadPositionsBool[x, z] = false; }

            }
        }
    }



    void GenerateBuildings()
    {
        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityLength; z++)
            {
                // Skip if a road already exists at this position
                if (roadPositionsBool[x, z]) continue;

                Vector3 position = new Vector3(x, 0, z) * 10.0f;
                GameObject instance;

                float noise = Mathf.PerlinNoise(x / 10.0f, z / 10.0f);

                if (noise < 0.3f)
                {
                    // Open space (park, plaza, etc.)
                    Instantiate(openSpacePrefab, position, Quaternion.identity);
                    roadPositions.Add(position);
                }
                else
                {
                    // Building instantiation
                    instance = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], position, Quaternion.identity);

                    // Building rotation
                    List<Vector3> adjacentRoads = FindAdjacentRoads(x, z);
                    if (adjacentRoads.Count > 0)
                    {
                        Vector3 directionToRoad = adjacentRoads[Random.Range(0, adjacentRoads.Count)];
                        Vector3 buildingForward = new Vector3(-directionToRoad.z, directionToRoad.y, directionToRoad.x);
                        instance.transform.rotation = Quaternion.LookRotation(buildingForward, Vector3.up);
                    }
                }
            }
        }
    }

    List<Vector3> FindAdjacentRoads(int x, int z)
    {
        // Directions for North, South, East, West
        Vector3[] directions = new Vector3[]
        {
        new Vector3(-1, 0, 0), // West
        new Vector3(1, 0, 0),  // East
        new Vector3(0, 0, -1), // South
        new Vector3(0, 0, 1)   // North
        };

        List<Vector3> adjacentRoads = new List<Vector3>();

        foreach (Vector3 direction in directions)
        {
            int newX = x + (int)direction.x;
            int newZ = z + (int)direction.z;


            // Ensure the new position is within the city bounds
            if (newX >= 0 && newX < cityWidth && newZ >= 0 && newZ < cityLength && roadPositionsBool[newX, newZ])
            {
                adjacentRoads.Add(direction);
            }
        }

        return adjacentRoads;
    }


    void GeneratePlayer()
    {
        if (roadPositions.Count > 0)
        {

            Vector3 playerPosition = roadPositions[Random.Range(0, roadPositions.Count)];
            GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            player.tag = "Player";

            Vector3 cubePosition = playerPosition + new Vector3(1, 0.5f, 1); // Adjust this offset as needed
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = cubePosition;
            cube.transform.localScale = new Vector3(2, 1, 1); // Adjust this as needed

            cube.tag = "Container";
        }
    }
    void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 enemyPosition = roadPositions[Random.Range(0, roadPositions.Count)];
                Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            }
        }
    }

    void GeneratePickups()
    {
        for (int i = 0; i < numberOfPickups; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 pickupPosition = roadPositions[Random.Range(0, roadPositions.Count)];

                float offsetX = Random.Range(-5f, 5f);
                float offsetZ = Random.Range(-5f, 5f);
                pickupPosition += new Vector3(offsetX, 0.5f, offsetZ);

                Instantiate(pickupPrefab, pickupPosition, Quaternion.identity);
            }
        }
    }
}

