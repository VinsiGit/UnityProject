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

    public GameObject birdPrefab; // Assign this in the Unity editor
    public int numberOfBirds = 10; // Adjust this as needed
    public float birdHeight = 50f; // Adjust this as needed

    public GameObject questPrefab;
    private GameObject player; // Assign this in the Unity editor
    private GameObject container;

    private List<Vector3> roadPositions = new List<Vector3>();
    private List<Vector3> grassPositions = new List<Vector3>();


    private bool[,] roadPositionsBool;
    private bool[,] carPositionsBool;

    public List<Vector3> RoadPositions { get; internal set; }

    void Start()
    {
        player = playerPrefab;

        roadPositionsBool = new bool[cityWidth, cityLength];

        GenerateCity();

        GeneratePlayer();
        // GenerateEnemies();
        // GeneratePickups();
        GenerateWalls();
        GenerateBirds(); // Add this line
        // InvokeRepeating("AddEnemy", 10.0f, 10.0f);

        // GenerateMinimapCamera(); // Add this line

    }
    void AddEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPosition = player.transform.position;
        Vector3 containerPosition = container.transform.position; // Assuming 'container' is your container GameObject

        Vector3 enemyPosition;
        do
        {
            enemyPosition = roadPositions[Random.Range(0, roadPositions.Count)];
        }
        while (Vector3.Distance(enemyPosition, playerPosition) < 15 && Vector3.Distance(enemyPosition, containerPosition) < 15);

        Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
    }

    void GenerateCity()
    {
        GenerateRoads();
        GenerateBuildings();

        GetComponent<NavMeshSurface>().BuildNavMesh();
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
                    grassPositions.Add(position);
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
        // Calculate the position next to the city wall
        Vector3 playerPosition = new Vector3(40, 0, 0);

        // Instantiate the player facing towards the city
        player.transform.position = playerPosition;
        player.transform.rotation = Quaternion.Euler(0, 90, 0);

        // Calculate the position for the container next to the player
        Vector3 containerPosition = playerPosition + new Vector3(5 * roadFrequencyX, 0, 0);

        // Instantiate the container
        container = Instantiate(containerPrefab, containerPosition, Quaternion.Euler(0, 90, 0));

        Vector3 questPosition = playerPosition + new Vector3(0, 0, 5);

        // Instantiate the container
        Instantiate(questPrefab, questPosition, Quaternion.Euler(0, 90, 0));

    }
    // void GeneratePlayer()
    // {
    //     if (grassPositions.Count > 0)
    //     {
    //         Vector3 playerPosition = grassPositions[Random.Range(0, grassPositions.Count)];
    //         player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);

    //         // Find a suitable position for the container
    //         Vector3 containerPosition = Vector3.zero;
    //         for (int i = 0; i < 100; i++) // Try 100 times
    //         {
    //             // Generate a random direction
    //             float xDirection = (Random.value < 0.5f) ? Random.Range(-5f, -2f) : Random.Range(2f, 5f);
    //             float zDirection = (Random.value < 0.5f) ? Random.Range(-5f, -2f) : Random.Range(2f, 5f);
    //             Vector3 direction = new Vector3(xDirection, 0, zDirection).normalized;
    //             // Calculate the container position
    //             containerPosition = playerPosition + direction * 10;

    //             // Check if the container position is on grass
    //             if (grassPositions.Contains(containerPosition))
    //             {
    //                 // The container position is suitable, exit the loop
    //                 break;
    //             }
    //         }

    //         GameObject container = Instantiate(containerPrefab, containerPosition, Quaternion.identity);
    //         container.transform.localScale = new Vector3(1, 1, 1); // Adjust this as needed
    //     }
    //     else if (roadPositions.Count > 0)
    //     {
    //         Vector3 playerPosition = roadPositions[Random.Range(0, roadPositions.Count)];
    //         player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);

    //         // Find a suitable position for the container
    //         Vector3 containerPosition = Vector3.zero;
    //         for (int i = 0; i < 100; i++) // Try 100 times
    //         {
    //             // Generate a random direction
    //             Vector3 direction = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)).normalized;

    //             // Calculate the container position
    //             containerPosition = playerPosition + direction * 5;

    //             // Check if the container position is on a road
    //             if (roadPositions.Contains(containerPosition))
    //             {
    //                 // The container position is suitable, exit the loop
    //                 break;
    //             }
    //         }

    //         GameObject container = Instantiate(containerPrefab, containerPosition, Quaternion.identity);
    //         container.transform.localScale = new Vector3(1, 1, 1); // Adjust this as needed
    //     }
    // }
    public void GenerateEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPosition = player.transform.position;
        Vector3 containerPosition = container.transform.position; // Assuming 'container' is your container GameObject

        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 enemyPosition;
                do
                {
                    enemyPosition = roadPositions[Random.Range(0, roadPositions.Count)];
                }
                while (Vector3.Distance(enemyPosition, playerPosition) < 10 && Vector3.Distance(enemyPosition, containerPosition) < 10);

                Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            }
        }
    }

    public void GeneratePickups()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPosition = player.transform.position;
        Vector3 containerPosition = container.transform.position; // Assuming 'container' is your container GameObject

        for (int i = 0; i < numberOfPickups; i++)
        {
            if (roadPositions.Count > 0)
            {
                Vector3 pickupPosition = roadPositions[Random.Range(0, roadPositions.Count)];
                Quaternion pickupRotation;
                do
                {
                    float offsetX = Random.Range(-4f, 4f);
                    float offsetZ = Random.Range(-4f, 4f);
                    pickupPosition += new Vector3(offsetX, 0.0f, offsetZ);

                    float randomYRotation = Random.Range(0f, 360f);
                    pickupRotation = Quaternion.Euler(0, randomYRotation, 0);

                }
                while (Vector3.Distance(pickupPosition, playerPosition) < 30 && Vector3.Distance(pickupPosition, containerPosition) < 30);


                Instantiate(pickupPrefab, pickupPosition, pickupRotation);
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
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        wallRenderer.material = wallMaterial;
        wallRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // Add this line
    }

    void GenerateBirds()
    {
        for (int i = 0; i < numberOfBirds; i++)
        {
            float xPosition = Random.Range(0, cityWidth * 10);
            float zPosition = Random.Range(0, cityLength * 10);
            Vector3 birdPosition = new Vector3(xPosition, birdHeight, zPosition);
            Instantiate(birdPrefab, birdPosition, Quaternion.identity);
        }
    }
    // void GenerateMinimapCamera()
    // {
    //     // Create a new camera for the minimap
    //     GameObject minimapCameraObject = new GameObject("MinimapCamera");
    //     Camera minimapCamera = minimapCameraObject.AddComponent<Camera>();

    //     // Set the camera's viewport rect to make the minimap smaller
    //     minimapCamera.rect = minimapViewportRect;

    //     // Set the camera's projection to orthographic and adjust its size
    //     minimapCamera.orthographic = true;
    //     minimapCamera.orthographicSize = Mathf.Max(cityWidth, cityLength) / 2;


    //     // Make the minimap camera a child of the player
    //     minimapCameraObject.transform.SetParent(player.transform);
    // }
}

