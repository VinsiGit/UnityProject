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
    public GameObject containerPrefab;
    public Material wallMaterial;
    public GameObject pickupPrefab;
    public int numberOfPickups = 5;

    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;

    public GameObject openSpacePrefab;
    public int cityWidth = 10;
    public int cityLength = 10;
    public int roadFrequencyX = 5; // Frequency of roads in the X direction
    public int roadFrequencyZ = 5; // Frequency of roads in the Z direction

    public float minimapCameraHeightMultiplier = 1.5f; // Add this line at the top of your class

    public Rect minimapViewportRect = new Rect(0.75f, 0.75f, 0.2f, 0.2f); // Adjust this as needed
    private GameObject player; // Assign this in the Unity editor


    private List<Vector3> roadPositions = new List<Vector3>();
    private bool[,] roadPositionsBool;
    void Start()
    {
        roadPositionsBool = new bool[cityWidth, cityLength];

        GenerateCity();

        GeneratePlayer();
        GenerateEnemies();
        GeneratePickups();
        GenerateWalls();

        GenerateMinimapCamera(); // Add this line

    }
    void Update()
    {
        // Update the minimap camera's position
        Vector3 playerPosition = player.transform.position;
        float cameraHeight = Mathf.Max(cityWidth, cityLength) * minimapCameraHeightMultiplier;
        Camera minimapCamera = GameObject.Find("MinimapCamera").GetComponent<Camera>();
        minimapCamera.transform.position = new Vector3(playerPosition.x, cameraHeight, playerPosition.z);
        minimapCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
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
            player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);

            Vector3 cubePosition = playerPosition + new Vector3(1, 0.5f, 1); // Adjust this offset as needed
            GameObject cube = Instantiate(containerPrefab, cubePosition, Quaternion.identity);
            cube.transform.localScale = new Vector3(2, 1, 1); // Adjust this as needed
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

    void GenerateWalls()
    {
        int size = 50;
        int cityWidthNew = cityWidth * 10;
        int cityLengthNew = cityLength * 10;
        int startX = -5;
        int startZ = -5;
        // Create the four walls
        CreateWall(new Vector3(startX, size / 2, startZ + cityLengthNew / 2), new Vector3(1, size, cityLengthNew));
        CreateWall(new Vector3(startX + cityWidthNew, size / 2, startZ + cityLengthNew / 2), new Vector3(1, size, cityLengthNew));
        CreateWall(new Vector3(startX + cityWidthNew / 2, size / 2, startZ), new Vector3(cityWidthNew, size, 1));
        CreateWall(new Vector3(startX + cityWidthNew / 2, size / 2, startZ + cityLengthNew), new Vector3(cityWidthNew, size, 1));
    }

    void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().material = wallMaterial;
    }

    void GenerateMinimapCamera()
    {
        // Create a new camera for the minimap
        GameObject minimapCameraObject = new GameObject("MinimapCamera");
        Camera minimapCamera = minimapCameraObject.AddComponent<Camera>();

        // Set the camera's viewport rect to make the minimap smaller
        minimapCamera.rect = minimapViewportRect;

        // Set the camera's projection to orthographic and adjust its size
        minimapCamera.orthographic = true;
        minimapCamera.orthographicSize = Mathf.Max(cityWidth, cityLength) / 2;


        // Make the minimap camera a child of the player
        minimapCameraObject.transform.SetParent(player.transform);
    }
}
