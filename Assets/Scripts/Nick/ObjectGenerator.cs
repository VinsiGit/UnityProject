using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public GameObject objectPrefab1;
    public GameObject objectPrefab2;
    public GameObject objectPrefab3;
    public int selectedInteger = 10;
    private float timeSinceLastGenerate = 0f;
    private float generateInterval = 3f;
    private bool logicActive = false;

    void Update()
    {
        if (logicActive);

            timeSinceLastGenerate += Time.deltaTime;

            if (timeSinceLastGenerate >= generateInterval)
            {
                GenerateObject();
                timeSinceLastGenerate = 0f;
                selectedInteger -= 1;

                if (selectedInteger <= 0)
                {
                    Debug.Log("Selected integer reached zero or less. Do something else here.");
                }
            }
    }

    void GenerateObject()
    {
        Vector3 playerPosition = transform.position;
        Vector3 spawnPosition = playerPosition - transform.forward * 2f + transform.up * 1.5f;

        GameObject selectedObject = GetRandomObject();
        GameObject instantiatedObject = Instantiate(selectedObject, spawnPosition, Quaternion.identity);

        // Ignore collision between the player and generated objects
        Physics.IgnoreCollision(GetComponent<Collider>(), instantiatedObject.GetComponent<Collider>());
    }

    GameObject GetRandomObject()
    {
        int randomIndex = Random.Range(1, 4);

        switch (randomIndex)
        {
            case 1:
                return objectPrefab1;
            case 2:
                return objectPrefab2;
            case 3:
                return objectPrefab3;
            default:
                Debug.LogError("Invalid random index");
                return null;
        }
    }
}
