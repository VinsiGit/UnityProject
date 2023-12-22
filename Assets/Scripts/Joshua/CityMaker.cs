using System.Collections.Generic;
using UnityEngine;

public class CityMaker : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject roadPrefab;
    public GameObject intersectionPrefab;
    public GameObject playerPrefab;

    public int cityWidth = 10;
    public int cityLength = 10;
    public int roadFrequency = 3;
    public int minRoadFrequency = 2;
    public int maxRoadFrequency = 5;
    public float roadProbability = 0.3f;

    private List<Vector3> roadPositions = new List<Vector3>();

    void Start()
    {

        for (int x = 0; x < cityWidth; x++)
        {
            for (int z = 0; z < cityLength; z++)
            {
                Vector3 position = new Vector3(x, 0, z) * 10.0f;
                GameObject instance;

                if (x % roadFrequency == 0 && z % roadFrequency == 0)
                {
                    Instantiate(intersectionPrefab, position, Quaternion.identity);
                    roadPositions.Add(position);
                }
                else if (x % roadFrequency == 0 || z % roadFrequency == 0)
                {
                    instance = Instantiate(roadPrefab, position, Quaternion.identity);
                    if (z % roadFrequency == 0)
                    {
                        instance.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    roadPositions.Add(position);
                }
                else
                {
                    instance = Instantiate(buildingPrefab, position, Quaternion.identity);
                    float randomScale = Random.Range(0.8f, 1.2f);
                    instance.transform.localScale = new Vector3(instance.transform.localScale.x, instance.transform.localScale.y * randomScale, instance.transform.localScale.z);
                }
            }
        }

        if (roadPositions.Count > 0)
        {
            Vector3 playerPosition = roadPositions[Random.Range(0, roadPositions.Count)];
            Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        }
    }
}